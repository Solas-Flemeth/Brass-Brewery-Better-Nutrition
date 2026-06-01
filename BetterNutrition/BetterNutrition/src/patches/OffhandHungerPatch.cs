
using HarmonyLib;
using Vintagestory.API.Common;
namespace BetterNutrition;

[HarmonyPatchCategory("brassbrewerybetternutrition.base")]
internal static class OffhandHungerPatch
{
    private static readonly BetterNutritionConfigData _config = BetterNutritionConfig.Config;
    [HarmonyPrefix]
    [HarmonyPatch(typeof(EntityStats), nameof(EntityStats.Set))]
    public static bool PrefixEntityStatsSet(string category, string code, float value, bool persistent = false)
    {
        return !(!_config.Misc.OffHandHungerPenalty && category.Equals("hungerrate")  && code.Equals("offhanditem"));
    }
}