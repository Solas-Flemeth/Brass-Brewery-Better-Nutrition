using Vintagestory.API.Common;
using Vintagestory.API.Server;
using Vintagestory.GameContent;

namespace BetterNutrition;

public class RegenerationSystem(ICoreServerAPI api)
{
    private static BetterNutritionConfigData config = BetterNutritionConfig.Config;
    private static DamageSource _healSource = new DamageSource() { Source = EnumDamageSource.Internal, Type = EnumDamageType.Heal };
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
            return;
        }
    }

    public static void NaturalHealTick(IServerPlayer serverPlayer)
    {
        EntityPlayer player = serverPlayer.Entity;
        EntityBehaviorHealth health= player.GetBehavior<EntityBehaviorHealth>(); 
        if (health == null)
            return; 
        
        float maxHealth = player.GetBehavior<EntityBehaviorHealth>()!.MaxHealth;
        float currentHealth = health.Health;
        if (currentHealth < maxHealth && player.Alive)
        {
            if (player.Attributes.GetFloat("betterNutrition-hunger") > config.Regeneration.MinimumSatiety)
            {
                player.ReceiveDamage(_healSource, player.Controls.FloorSitting ? config.Regeneration.RestingRegenerationRate * maxHealth : config.Regeneration.RegenerationRate * maxHealth);
                
            }
        }
    }
}