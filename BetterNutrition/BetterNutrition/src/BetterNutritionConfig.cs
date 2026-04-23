using System;
using System.IO;
using HarmonyLib;
using Vintagestory.API.Common;

namespace BetterNutrition;


public class BetterNutritionConfigData
{
    public String ConfigVersion { get; set; } = BetterNutritionConfig.CurrentConfigVersion;
    public bool NutritionImpactsWalkSpeed { get; set; } = true;
    //Walk speed
    public float FruitWalkSpeed { get; set; } = 0.1f;
    public float GrainWalkSpeed { get; set; } = 0.05f;
    public float VegetableWalkSpeed { get; set; } = 0.025f;
    public float ProteinWalkSpeed { get; set; } = 0.05f;
    public float DairyWalkSpeed { get; set; } = 0.025f;
    public float BaseWalkSpeed { get; set; } = -0.1f;
    public float StarvationWalkSpeed { get; set; } = -0.2f;
    
    //Hunger Rate
    public bool NutritionImpactsMiningSpeed { get; set; } = true;
    public float FruitHungerRate { get; set; } = -0.05f;
    public float GrainHungerRate { get; set; } = -0.075f;
    public float VegetableHungerRate { get; set; } = -0.075f;
    public float ProteinHungerRate { get; set; } = -0.05f;
    public float DairyHungerRate { get; set; } = -0.1f;
    public float BaseHungerRate { get; set; } = 0.1f;
    public float StarvationHungerRate { get; set; } = -0.25f;
    
    //Mining Speed
    public bool NutritionImpactsHungerRate { get; set; } = true;
    public float FruitMiningSpeed { get; set; } = 0.05f;
    public float GrainMiningSpeed { get; set; } = 0.15f;
    public float VegetableMiningSpeed { get; set; } = 0.1f;
    public float ProteinMiningSpeed { get; set; } = 0.1f;
    public float DairyMiningSpeed { get; set; } = 0.05f;
    public float BaseMiningSpeed { get; set; } = -0.15f;
    public float StarvationMiningSpeed { get; set; } = -0.33f;
    
    
    //Melee Damage; 
    public bool NutritionImpactsMeleeDamage { get; set; } = true;
    public float FruitMeleeDamage { get; set; } = 0.075f;
    public float GrainMeleeDamage { get; set; } = 0.05f;
    public float VegetableMeleeDamage { get; set; } = 0.05f;
    public float ProteinMeleeDamage { get; set; } = 0.15f;
    public float DairyMeleeDamage { get; set; } = 0.075f;
    public float BaseMeleeDamage { get; set; } = -0.15f;
    public float StarvationMeleeDamage { get; set; } = -0.3f;
    
    //Health Overrides; 
    public bool OverrideNutritionHealthBonus { get; set; } = true;
    public float FruitHealth { get; set; } = 3.0f;
    public float GrainHealth{ get; set; } = 2f;
    public float VegetableHealth { get; set; } = 4f;
    public float ProteinHealth { get; set; } = 2f;
    public float DairyHealth { get; set; } = 3f;
    public float BaseHealth { get; set; } = 0f;
    public float StarvationHealth { get; set; } = -7.5f;
    //starvationMode
    public bool StarvationMode { get; set; } = true;

    public float MinimumFoodToStarve
    {
        get;
        set => field = Math.Clamp(value, 0.0f, 1.0f);
    } = 0.5f;

    //Misc
    public bool OffHandHungerPenalty { get; set; } = false;
    //public bool SittingReducesHunger { get; set; } = true;
    //public float SittingHungerReduction { get; set; } = -0.2f;
    public float AdditionalSatietyBonus { get; set; } = 500f;
}

public class BetterNutritionConfig
{
    public static String CurrentConfigVersion { get; set; } = "0.2.1";
    public static BetterNutritionConfig Instance { get; set; } = new BetterNutritionConfig();
    public static BetterNutritionConfigData Config;
    //overall
    public static void LoadConfig(Mod mod, ICoreAPI api) 
    {
        try
        {
            Config = api.LoadModConfig<BetterNutritionConfigData>("BrassBrewery-BetterNutrition.json");
            if (Config == null)
            {
                Config = new BetterNutritionConfigData();
            }
            api.StoreModConfig<BetterNutritionConfigData>(Config, "BrassBrewery-BetterNutrition.json");
        }
        catch (Exception e)
        {
            //Couldn't load the mod config... Create a new one with default settings, but don't save it.
            mod.Logger.Error("Could not load config 'BrassBrewery-BetterNutrition'! Loading default settings instead.");
            mod.Logger.Error(e);
            Config = new BetterNutritionConfigData();
        }
    }

}