using System;
using Vintagestory.API.Common;

namespace BetterNutrition;

public class BetterNutritionConfigData
{
    public NutritionSettings Nutrition { get; set; } = new NutritionSettings();
    public RegenerationSettings Regeneration { get; set; } = new RegenerationSettings();
    public MiscSettings Misc { get; set; } = new MiscSettings();
    public IntegrationSettings Integration { get; set; } = new IntegrationSettings();
}

public class BetterNutritionConfig
{
    public static bool IsLoaded { get; private set; } = false;
    public static BetterNutritionConfig Instance { get; set; } = new BetterNutritionConfig();
    public static BetterNutritionConfigData Config { get; private set; }
    //overall
    public static void LoadConfig(Mod mod, ICoreAPI api) 
    {
        try
        {
            Config = api.LoadModConfig<BetterNutritionConfigData>("BrassBrewery-BetterNutrition.json");
            if (Config == null)
            {
                Config = new BetterNutritionConfigData();
                BetterNutritionModSystem.Log("Configuration file 'BrassBrewery-BetterNutrition' could not be found. Creating new one.");
            }
            api.StoreModConfig<BetterNutritionConfigData>(Config, "BrassBrewery-BetterNutrition.json");
            IsLoaded = true;
        }
        catch (Exception e)
        { 
            BetterNutritionModSystem.Log("Could not load config 'BrassBrewery-BetterNutrition'. Loading default settings instead.");
            mod.Logger.Error(e);
            Config = new BetterNutritionConfigData();
            IsLoaded = true;
        }
    }
}