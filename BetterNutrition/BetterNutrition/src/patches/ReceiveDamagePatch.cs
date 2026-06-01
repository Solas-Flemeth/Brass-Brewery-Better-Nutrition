using HarmonyLib;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;

namespace BetterNutrition;


[HarmonyPatchCategory("brassbrewerybetternutrition.base")]
public class ReceiveDamagePatch
{
    private static readonly BetterNutritionConfigData _config = BetterNutritionConfig.Config;
    [HarmonyPrefix]
    [HarmonyPatch(typeof(Entity), nameof(Entity.ReceiveDamage))]
    public static void BeforeReceiveDamage(DamageSource damageSource, ref float damage)
    {
        if (damageSource.Type == EnumDamageType.Hunger && damage == 0.125f)
        {
            damage = _config.Misc.VanillaStarvationDamage;
        }
    }
}