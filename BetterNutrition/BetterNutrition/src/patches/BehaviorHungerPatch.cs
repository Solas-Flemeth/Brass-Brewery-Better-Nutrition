using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using HarmonyLib;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.GameContent;
namespace BetterNutrition;

[HarmonyPatchCategory("brassbrewerybetternutrition.base")]
internal static class BehaviorHungerPatch
{
        
    private static readonly BetterNutritionConfigData _config = BetterNutritionConfig.Config;
    [HarmonyPrefix]
    [HarmonyPatch(typeof(EntityBehaviorHunger), nameof(EntityBehaviorHunger.UpdateNutrientHealthBoost) )]
    public static bool  ReplaceUpdateNutrientHealthBoost(EntityBehaviorHunger __instance)
    {
        if ( __instance.entity is not EntityPlayer player)
            return true;
        var nutrition = _config.Nutrition;
        float maxSaturation = 1f / __instance.MaxSaturation;
        float fruitPercentage =  Math.Clamp( __instance.FruitLevel * maxSaturation, 0.0f, 1.0f);
        float grainPercentage =  Math.Clamp( __instance.GrainLevel * maxSaturation, 0.0f, 1.0f);   
        float vegetablePercentage =  Math.Clamp( __instance.VegetableLevel *  maxSaturation, 0.0f, 1.0f);
        float proteinPercentage =  Math.Clamp( __instance.ProteinLevel *  maxSaturation, 0.0f, 1.0f);
        float dairyPercentage =  Math.Clamp( __instance.DairyLevel *  maxSaturation, 0.0f, 1.0f);
        float hungerPercentage =  Math.Clamp( __instance.Saturation *  maxSaturation, 0.0f, 1.0f); 
        float starvationEffectivness = nutrition.Starvation.Enable && hungerPercentage < nutrition.Starvation.MinimumFoodToStarve ? 1f - (__instance.Saturation / (nutrition.Starvation.MinimumFoodToStarve * __instance.MaxSaturation)) : 0.0f;
        //cache in data tree for other methods
        player.Attributes.SetFloat("betterNutrition-fruit", fruitPercentage);
        player.Attributes.SetFloat("betterNutrition-grain", grainPercentage);
        player.Attributes.SetFloat("betterNutrition-vegetable", vegetablePercentage);
        player.Attributes.SetFloat("betterNutrition-protein", proteinPercentage);
        player.Attributes.SetFloat("betterNutrition-dairy", dairyPercentage);
        player.Attributes.SetFloat("betterNutrition-hunger", hungerPercentage);
        //
        if (nutrition.Health.Enable){
            player.GetBehavior<EntityBehaviorHealth>()!.SetMaxHealthModifiers("nutrientHealthMod", CalculateStarvationNutritionModifier(nutrition.Health, fruitPercentage, grainPercentage, vegetablePercentage, proteinPercentage, dairyPercentage, starvationEffectivness));
        }
        else
        { //perform simplified vanilla logic
            player.GetBehavior<EntityBehaviorHealth>()!.SetMaxHealthModifiers("nutrientHealthMod", 2.5f * (fruitPercentage + grainPercentage + vegetablePercentage + proteinPercentage + dairyPercentage)); 
        }
        return false;
    }
    
    private static float CalculateNutritionModifier(INutritionModifier modifier, float fruit, float grain, float vegetable, float protein, float dairy)
    {
        return fruit * modifier.Fruit + grain * modifier.Grain + vegetable * modifier.Vegetable + protein * modifier.Protein + dairy * modifier.Dairy;
    }
    
    private static float CalculateStarvationNutritionModifier(IStarvationNutritionModifier modifier, float fruit, float grain, float vegetable, float protein, float dairy, float starvation)
    { 
        return CalculateNutritionModifier(modifier, fruit, grain, vegetable, protein, dairy) + modifier.Base + starvation * modifier.Starvation;
    }
}