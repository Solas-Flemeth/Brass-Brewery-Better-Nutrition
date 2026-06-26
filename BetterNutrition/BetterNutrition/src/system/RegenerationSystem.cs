using System;
using Vintagestory.API.Common;
using Vintagestory.API.Server;
using Vintagestory.GameContent;

namespace BetterNutrition;

public class RegenerationSystem(ICoreServerAPI api)
{
    private static BetterNutritionConfigData config = BetterNutritionConfig.Config;
    private static DamageSource _healSource = new()
    {
        Source = EnumDamageSource.Internal,
        Type = EnumDamageType.Heal,
        DamageTier = 0,
        Duration = TimeSpan.FromSeconds(config.Regeneration.TickRate),
        TicksPerDuration = config.Regeneration.TickRate*3f>=1f ? (int) (config.Regeneration.TickRate*3f) : 1
    };
    public void OnTick(float delta)
    {
        {
            if (!config.Regeneration.Enable)
            {
                return;
            }
            foreach (IServerPlayer player in api.World.AllOnlinePlayers)
            {
                NaturalHealTick(player);
            }
        }
    }

    public static void NaturalHealTick(IServerPlayer serverPlayer)
    {
        EntityPlayer? player = serverPlayer.Entity;
        if (player.SidedProperties == null) //ensure getBehavior doesnt break
        {
            return;  
        }
        EntityBehaviorHealth? health = player?.GetBehavior<EntityBehaviorHealth>();
        if (health == null)
        {
            return; 
        }
        float maxHealth = health.MaxHealth;
        float currentHealth = health.Health;
        if (!(currentHealth < maxHealth))
        {
            return;
        }
        if (player.Attributes.GetFloat("betterNutrition-hunger") > config.Regeneration.MinimumSatiety)
        {   
            player.ReceiveDamage(_healSource, player.Controls.FloorSitting ? config.Regeneration.RestingRegenerationRate * maxHealth : config.Regeneration.RegenerationRate * maxHealth);
            if (config.Regeneration.SatietyCost > 0)
            {
                player.GetBehavior<EntityBehaviorHunger>()?.ConsumeSaturation(config.Regeneration.SatietyCost);
            }
        }
    }
}