using System.Reflection;
using HarmonyLib;
using Vintagestory.API.Client;
using Vintagestory.API.Datastructures;
using Vintagestory.Client.NoObf;

namespace BetterNutrition;

[HarmonyPatchCategory("brassbrewerybetternutrition.client")]
public static class PatchClientStatBar
{
    private static readonly BetterNutritionConfigData _config = BetterNutritionConfig.Config;
    private static readonly FieldInfo GuiElementStatBarColorField = typeof(GuiElementStatbar).GetField("color", BindingFlags.Instance | BindingFlags.NonPublic);
    
    [HarmonyPostfix]
    [HarmonyPatch(typeof(HudStatbar), "UpdateSaturation")]
    public static void ModifyHungerBarDisplay(HudStatbar __instance)
    {
        ICoreClientAPI? capi = (ICoreClientAPI)AccessTools.Field(typeof(HudStatbar), "capi").GetValue(__instance);
        GuiElementStatbar? saturationBar = (GuiElementStatbar)AccessTools.Field(typeof(HudStatbar), "saturationbar").GetValue(__instance);
        if (saturationBar == null)
        {
            BetterNutritionModSystem.Log("Error: Can't get saturation bar");
            return;
        }
        ITreeAttribute? treeAttribute = capi?.World.Player.Entity.WatchedAttributes.GetTreeAttribute("hunger");
        if (treeAttribute == null)
        {
            BetterNutritionModSystem.Log("Error: Can't find hunger tree");
            return;
        }
        float? saturation = treeAttribute.TryGetFloat("currentsaturation");
        float? maxSaturation = treeAttribute.TryGetFloat("maxsaturation");
        if (saturation == null || maxSaturation == null)
        {
            BetterNutritionModSystem.Log("Error: Can't find saturation totals");
            return;
        }
        if (_config.Client.ShowStarvation)
        {
            float? fruitDelay = treeAttribute.TryGetFloat("saturationlossdelayfruit");
            float?  vegetableDelay = treeAttribute.TryGetFloat("saturationlossdelayvegetable");
            float? proteinDelay = treeAttribute.TryGetFloat("saturationlossdelayprotein");
            float?  grainDelay = treeAttribute.TryGetFloat("saturationlossdelaygrain");
            float? dairyDelay = treeAttribute.TryGetFloat("saturationlossdelaydairy");
            if (fruitDelay + vegetableDelay + proteinDelay + grainDelay + dairyDelay > 0f)
            {
                setColor(saturationBar, SaturationDelayColor);
                return;
            }
        }
        if (_config.Nutrition.Starvation.Enable && _config.Client.ShowStarvation)
        {
            float? hungerPercentage = saturation/maxSaturation;
            float? starvationEffectivness = _config.Nutrition.Starvation.Enable && hungerPercentage < _config.Nutrition.Starvation.MinimumFoodToStarve ? 1f - (saturation / (_config.Nutrition.Starvation.MinimumFoodToStarve * maxSaturation)) : 0.0f;
            if (starvationEffectivness > 0.0f)
            {
                setColor(saturationBar, StarvationGradientCalculator(starvationEffectivness));
                return;
            }
        }
        setColor(saturationBar, GuiStyle.FoodBarColor);
    }

    private static void setColor(GuiElementStatbar? statbar, double[] color)
    {
        //BetterNutritionModSystem.Log("Changing Hunger Color");
        if (statbar == null)
        {
            return;
        }
        GuiElementStatBarColorField.SetValue(statbar, color);
    }
    
    private static double[] SaturationDelayColor = new double[4]
    {
        1.0,
        0.827,
        0.129,
        1.0
    };
    private static double[] StarvationBarColor = new double[4]
    {
        0.600,
        0.211,
        0.0,
        1.0
    };

    private static double[] StarvationGradientCalculator(float? starvationEffectivness)
    {
        double[] foodBarColor = GuiStyle.FoodBarColor;
        if (starvationEffectivness == null)
        {
            return GuiStyle.FoodBarColor;
        }
        double red = (double)(foodBarColor[0] - (foodBarColor[0]-StarvationBarColor[0])*starvationEffectivness)!;
        double green = (double)(foodBarColor[1] - (foodBarColor[1]-StarvationBarColor[1])*starvationEffectivness)!;
        double blue = (double)(foodBarColor[2] - (foodBarColor[2]-StarvationBarColor[2])*starvationEffectivness)!;
        return [red, green, blue, 1.0];
    }
}