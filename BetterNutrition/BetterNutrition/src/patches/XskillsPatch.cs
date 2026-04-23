using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using HarmonyLib;
using Vintagestory.API.Common;
using XSkills;

namespace BetterNutrition;


public static class XskillsPatch
{
    static bool patched = false;
    public static void Apply(Harmony harmony, ICoreAPI api)
    {
        Type type = typeof(XSkills.Survival);
        var method = AccessTools.Method(type, nameof(Survival.OnHugeStomach));
        if (method == null)
            return;
        HarmonyMethod transpilerPatch = new HarmonyMethod(typeof(XskillsPatch), nameof(Transpiler));
        transpilerPatch.category = "brassbrewerybetternutrition";
        MethodInfo info = harmony.Patch(method, transpiler: transpilerPatch );
        
    }
    public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
    {
        float saturationValue = BetterNutritionConfig.Config.AdditionalSatietyBonus;
        foreach (var code in instructions)
        {
            if (code.opcode == OpCodes.Ldc_I4 && code.operand is int i && i == 1500)
            {
                yield return new CodeInstruction(OpCodes.Ldc_I4, i + (int) saturationValue);
                continue;
            }
            if (code.opcode == OpCodes.Ldc_R4 && code.operand is float f && f == 1500f)
            {
                yield return new CodeInstruction(OpCodes.Ldc_R4, f + saturationValue);
                continue;
            }
            yield return code;
        }
    }
}