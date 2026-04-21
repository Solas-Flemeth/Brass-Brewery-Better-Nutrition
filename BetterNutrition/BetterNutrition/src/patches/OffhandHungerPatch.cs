using System;
using HarmonyLib;
using Vintagestory.API.Common;

namespace BetterNutrition;

[HarmonyPatchCategory("brassbrewerybetternutrition")]
internal static class OffhandHungerPatch
{
    private static readonly BetterNutritionConfigData _config = BetterNutritionConfig.Config;
    [HarmonyPrefix]
    [HarmonyPatch(typeof(EntityStats), nameof(EntityStats.Set))]
    public static bool BeforeUpdateNutrientHealthBoost(string category, string code, float value, bool persistent = false)
    {
        return !(!_config.OffHandHungerPenalty && category.Equals("hungerrate")  && code.Equals("offhanditem"));
    }
}