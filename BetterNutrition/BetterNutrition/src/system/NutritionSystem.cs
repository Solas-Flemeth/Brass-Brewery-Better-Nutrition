using System;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.Server;

namespace BetterNutrition;

public class NutritionSystem(ICoreServerAPI api)
{
    private static  BetterNutritionConfigData config = BetterNutritionConfig.Config;
    public void PlayerNutritionUpdateCheck(float time)
    {
        if (!config.Nutrition.Enable)
        {
            return;
        }
        
        foreach (IServerPlayer player in api.World.AllOnlinePlayers)
        {
            UpdateNutritionStats(player);
        }
        return;
    }

    public static void UpdateNutritionStats(IServerPlayer serverPlayer,bool force = false)
    {
        EntityPlayer? player = serverPlayer.Entity;
        if (player == null)
        {
            return;
        }
        float fruitPercentage = player.Attributes.GetFloat("betterNutrition-fruit");
        float grainPercentage = player.Attributes.GetFloat("betterNutrition-grain");
        float vegetablePercentage = player.Attributes.GetFloat("betterNutrition-vegetable");
        float proteinPercentage = player.Attributes.GetFloat("betterNutrition-protein");
        float dairyPercentage = player.Attributes.GetFloat("betterNutrition-dairy");
        float hungerPercentage = player.Attributes.GetFloat("betterNutrition-hunger");
        var nutrition = config.Nutrition;
        if (!force)
        {
            float minUpdatePercentage = nutrition.UpdateThreshold;
            bool fruitUpdate = Math.Abs(player.Attributes.GetFloat("betterNutrition-fruit-old")-fruitPercentage) > minUpdatePercentage;
            bool grainUpdate = Math.Abs(player.Attributes.GetFloat("betterNutrition-grain-old")-grainPercentage) > minUpdatePercentage;
            bool vegetableUpdate = Math.Abs(player.Attributes.GetFloat("betterNutrition-vegetable-old")- vegetablePercentage) > minUpdatePercentage;
            bool proteinUpdate = Math.Abs(player.Attributes.GetFloat("betterNutrition-protein-old")-proteinPercentage) > minUpdatePercentage;
            bool dairyUpdate = Math.Abs(player.Attributes.GetFloat("betterNutrition-dairy-old")-dairyPercentage) > minUpdatePercentage;
            bool hungerUpdate = Math.Abs(player.Attributes.GetFloat("betterNutrition-hunger-old")-hungerPercentage) > minUpdatePercentage 
                                       && nutrition.Starvation.MinimumFoodToStarve > hungerPercentage && nutrition.Starvation.Enable;
            if (!(fruitUpdate || grainUpdate || vegetableUpdate || proteinUpdate || dairyUpdate || hungerUpdate))
            {
                return;   //exit method if there is nothing at update threshold
            }
        }
        //update Vanilla stats
        float starvationEffectiveness =  nutrition.Starvation.Enable && hungerPercentage < nutrition.Starvation.MinimumFoodToStarve ? 1f - hungerPercentage/nutrition.Starvation.MinimumFoodToStarve : 0.0f;
        
        if(nutrition.WalkSpeed.Enable) 
            player.Stats.Set("walkspeed", "betternutrition-food", CalculateStarvationNutritionModifier(nutrition.WalkSpeed, fruitPercentage, grainPercentage, vegetablePercentage, proteinPercentage, dairyPercentage, starvationEffectiveness)); 
        
        if(nutrition.Hunger.Enable)     
            player.Stats.Set("hungerrate", "betternutrition-food", CalculateStarvationNutritionModifier(nutrition.Hunger, fruitPercentage, grainPercentage, vegetablePercentage, proteinPercentage, dairyPercentage, starvationEffectiveness));
        
        if(nutrition.MiningSpeed.Enable)
            player.Stats.Set("miningSpeedMul", "betternutrition-food",  CalculateStarvationNutritionModifier(nutrition.MiningSpeed, fruitPercentage, grainPercentage, vegetablePercentage, proteinPercentage, dairyPercentage, starvationEffectiveness));
        
        if (nutrition.MeleeDamage.Enable)
            player.Stats.Set("MeleeWeaponsDamage", "betternutrition-food", CalculateStarvationNutritionModifier(nutrition.MeleeDamage, fruitPercentage, grainPercentage, vegetablePercentage, proteinPercentage, dairyPercentage, starvationEffectiveness));
        // XSkills
        if (IntegrationController.XSkillsEnabled)//update if additional XSkill items added
        {
            player.Stats.Set("expMult", "betternutrition",CalculateNutritionModifier(config.Integration.XSkills.XSkillMultiplier, fruitPercentage, grainPercentage, vegetablePercentage, proteinPercentage, dairyPercentage), true);
        } 
        //Hydrate Dydrate
        if (IntegrationController.HydrateOrDiedrateEnabled)
        {
            if (config.Integration.HydrateOrDiedrate.CoolingMultiplier.Enable)
            {
                player.Stats.Set("HoD:CoolingMul", "betternutrition",CalculateNutritionModifier(config.Integration.HydrateOrDiedrate.CoolingMultiplier, fruitPercentage, grainPercentage, vegetablePercentage, proteinPercentage, dairyPercentage), true);
            }

            if (config.Integration.HydrateOrDiedrate.ThirstRate.Enable)
            {
                player.Stats.Set("HoD:ThirstRateMul", "betternutrition",CalculateNutritionModifier(config.Integration.HydrateOrDiedrate.ThirstRate, fruitPercentage, grainPercentage, vegetablePercentage, proteinPercentage, dairyPercentage), true);
            }
        }
        //UpdateStats to compare later
        player.Attributes.SetFloat("betterNutrition-fruit-old", fruitPercentage);
        player.Attributes.SetFloat("betterNutrition-grain-old", grainPercentage);
        player.Attributes.SetFloat("betterNutrition-vegetable-old", vegetablePercentage);
        player.Attributes.SetFloat("betterNutrition-protein-old", proteinPercentage);
        player.Attributes.SetFloat("betterNutrition-dairy-old", dairyPercentage);
        player.Attributes.SetFloat("betterNutrition-hunger-old", hungerPercentage);
    }

    /// <summary>
    /// Removes all player stats from the mod related to nutrition modifiers. The health modifier is not included but should update to be correct if the mod is uninstalled as its part of the overrode method
    /// </summary>
    /// <param name="player"></param>
    public static void ResetPlayerStats(IServerPlayer player)
    {
        player.Entity.Stats.Remove("walkspeed", "betternutrition-food");
        player.Entity.Stats.Remove("hungerrate", "betternutrition-food");
        player.Entity.Stats.Remove("miningSpeedMul", "betternutrition-food");
        player.Entity.Stats.Remove("MeleeWeaponsDamage", "betternutrition-food");
        player.Entity.Stats.Remove("expMult", "betternutrition-food");
        player.Entity.Stats.Remove("HoD:CoolingMu", "betternutrition-food");
        player.Entity.Stats.Remove("HoD:ThirstRateMul", "betternutrition-food");
        //let game handle health calculation as it will do the same without the mod installed
    }
    /// <summary>
    /// Calculates Nutrition modifiers for a character's <see cref="INutritionModifier"/> 
    /// </summary>
    /// <param name="modifier"></param>
    /// <param name="fruit"></param>
    /// <param name="grain"></param>
    /// <param name="vegetable"></param>
    /// <param name="protein"></param>
    /// <param name="dairy"></param>
    /// <returns></returns>
    private static float CalculateNutritionModifier(INutritionModifier modifier, float fruit, float grain, float vegetable, float protein, float dairy)
    {
        return fruit * modifier.Fruit + grain * modifier.Grain + vegetable * modifier.Vegetable + protein * modifier.Protein + dairy * modifier.Dairy;
    }
    /// <summary>
    ///  Calculates Nutrition modifiers for a character's <see cref="IStarvationNutritionModifier"/> 
    /// </summary>
    /// <param name="modifier"></param>
    /// <param name="fruit"></param>
    /// <param name="grain"></param>
    /// <param name="vegetable"></param>
    /// <param name="protein"></param>
    /// <param name="dairy"></param>
    /// <param name="starvation"></param>
    /// <returns></returns>
    private static float CalculateStarvationNutritionModifier(IStarvationNutritionModifier modifier, float fruit, float grain, float vegetable, float protein, float dairy, float starvation)
    { 
        return CalculateNutritionModifier(modifier, fruit, grain, vegetable, protein, dairy) + modifier.Base + starvation * modifier.Starvation;
    }
}