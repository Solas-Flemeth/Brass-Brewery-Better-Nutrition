

using System;
using HarmonyLib;
using Vintagestory.API.Common;
using Vintagestory.GameContent;
namespace BetterNutrition;

[HarmonyPatchCategory("brassbrewerybetternutrition")]
internal static class BehaviorHungerPatch
{
    private static readonly BetterNutritionConfigData _config = BetterNutritionConfig.Config;

    //credit to Xandu for referencing their Xskills code
    [HarmonyPostfix]
    [HarmonyPatch(typeof(EntityBehaviorHunger), nameof(EntityBehaviorHunger.Initialize))]
    public static void AfterInitialize(EntityBehaviorHunger __instance)
    {
        if (__instance.entity is EntityPlayer == false || _config.AdditionalSatietyBonus == 0f)
        {
            Console.WriteLine("No tweaking to hunger for");
            return;
        }
        bool fullFood = __instance.Saturation == __instance.MaxSaturation;
        __instance.MaxSaturation = 1500f + _config.AdditionalSatietyBonus;
        if (fullFood)
        {
            __instance.Saturation = __instance.MaxSaturation;
        }
        float adjustedSaturation = (__instance.Saturation / 1500f) * __instance.MaxSaturation;
        __instance.FruitLevel *= adjustedSaturation;
        __instance.GrainLevel *= adjustedSaturation;
        __instance.VegetableLevel *= adjustedSaturation;
        __instance.ProteinLevel *= adjustedSaturation;
        __instance.DairyLevel *= adjustedSaturation;
        __instance.UpdateNutrientHealthBoost();
    }
    
    
    [HarmonyPostfix]
    [HarmonyPatch(typeof(EntityBehaviorHunger), nameof(EntityBehaviorHunger.UpdateNutrientHealthBoost) )]
    public static void AfterUpdateNutrientHealthBoost(EntityBehaviorHunger __instance)
    {
        if ( __instance.entity is not EntityPlayer player) return;
        //prep math
        float fruitPercentage =  __instance.FruitLevel /  __instance.MaxSaturation;
        float grainPercentage =  __instance.GrainLevel /  __instance.MaxSaturation;   
        float vegetablePercentage =  __instance.VegetableLevel /  __instance.MaxSaturation;
        float proteinPercentage =  __instance.ProteinLevel /  __instance.MaxSaturation;
        float dairyPercentage =  __instance.DairyLevel /  __instance.MaxSaturation;
        float hungerPercentage =  __instance.Saturation /  __instance.MaxSaturation;  
        //starvation logic
        float starvationEffectiveness = 0f;
        bool isStarving = _config.MinimumFoodToStarve >= hungerPercentage;
        if ( _config.StarvationMode && _config.MinimumFoodToStarve >= hungerPercentage)
        {
            starvationEffectiveness =  1f-(__instance.Saturation / (_config.MinimumFoodToStarve *  __instance.MaxSaturation));
        }
        //walk speed
        if (_config.NutritionImpactsWalkSpeed)
        {
            float walkSpeedModifier = _config.BaseWalkSpeed 
                                      + _config.StarvationWalkSpeed * starvationEffectiveness 
                                      + fruitPercentage * _config.FruitWalkSpeed 
                                      + grainPercentage * _config.GrainWalkSpeed 
                                      + vegetablePercentage * _config.VegetableWalkSpeed 
                                      + dairyPercentage * _config.DairyWalkSpeed 
                                      + proteinPercentage * _config.ProteinWalkSpeed;
            player.Stats.Set("walkspeed", "betternutrition-food", walkSpeedModifier);
        }
        //hunger rate
        if (_config.NutritionImpactsHungerRate)
        {
            float hungerRateModifier = _config.BaseHungerRate + _config.StarvationHungerRate *  starvationEffectiveness;
                hungerRateModifier += fruitPercentage * _config.FruitHungerRate 
                                      + grainPercentage * _config.GrainHungerRate 
                                      + vegetablePercentage * _config.VegetableHungerRate
                                      + dairyPercentage * _config.DairyHungerRate
                                      + proteinPercentage * _config.ProteinHungerRate;
            player.Stats.Set("hungerrate", "betternutrition-food", hungerRateModifier);
        }
        //Mining speed
        if (_config.NutritionImpactsMiningSpeed)
        {
            float miningSpeedModifier = _config.BaseMiningSpeed 
                                        + _config.StarvationMiningSpeed *  starvationEffectiveness 
                                        + fruitPercentage * _config.FruitMiningSpeed 
                                        + grainPercentage * _config.GrainMiningSpeed 
                                        + vegetablePercentage * _config.VegetableMiningSpeed 
                                        + dairyPercentage * _config.DairyMiningSpeed 
                                        + proteinPercentage * _config.ProteinMiningSpeed;
            player.Stats.Set("miningSpeedMul", "betternutrition-food", miningSpeedModifier);
        }
        //Melee Damage
        if (_config.NutritionImpactsMeleeDamage)
        {
            float meleeWeaponsDamageModifier = _config.BaseMeleeDamage 
                                               + _config.StarvationMeleeDamage *  starvationEffectiveness
                                               + fruitPercentage * _config.FruitMeleeDamage
                                               + grainPercentage * _config.GrainMeleeDamage
                                               + vegetablePercentage * _config.VegetableMeleeDamage
                                               + dairyPercentage * _config.DairyMeleeDamage
                                               + proteinPercentage * _config.ProteinMeleeDamage;
            player.Stats.Set("MeleeWeaponsDamage", "betternutrition-food", meleeWeaponsDamageModifier);
        }
        //override health
        if (_config.OverrideNutritionHealthBonus)
        {
            float healthOverrideModifier = _config.BaseHealth 
                                           + _config.StarvationHealth * starvationEffectiveness 
                                           + fruitPercentage * _config.FruitHealth 
                                           + grainPercentage * _config.GrainHealth 
                                           + vegetablePercentage * _config.VegetableHealth 
                                           + dairyPercentage * _config.DairyHealth 
                                           + proteinPercentage * _config.ProteinHealth;
            player.GetBehavior<EntityBehaviorHealth>()!.SetMaxHealthModifiers("nutrientHealthMod", healthOverrideModifier);
        }
    }
}