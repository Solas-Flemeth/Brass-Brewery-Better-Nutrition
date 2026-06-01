using Vintagestory.API.Common;

namespace BetterNutrition;

public static class IntegrationController
{
    /// <summary>
    /// Checks if integration with XSkills should be active
    /// </summary>
    public static bool XSkillsEnabled { get; set; } = false;
    
    /// <summary>
    /// Checks if integration with Combat Overhaul should be active
    /// </summary>
    public static bool CombatOverhaulEnabled { get; set; } = false;
    
    /// <summary>
    /// Checks if integration with HydrateOrDiedrate should be active
    /// </summary>
    public static bool HydrateOrDiedrateEnabled { get; set; } = false;

    public static bool StartXSkillsIntegration(ICoreAPI api)
    {
        if (api.ModLoader.IsModEnabled("xlib") || api.ModLoader.IsModEnabled("xlibfork"))
        {
            if (BetterNutritionConfig.Config.Integration.XSkills.Enable && BetterNutritionConfig.Config.Integration.XSkills.XSkillMultiplier.Enable)
            {
                XSkillsEnabled = true;
                BetterNutritionModSystem.Log("Mod XLib Detect: Enable XSkills Integration");
                return XSkillsEnabled;
            }
            else
            {
               BetterNutritionModSystem.Log("Mod XLib Detect: Config has integration disabled");
            }
        }
        XSkillsEnabled = false;
        return XSkillsEnabled;
    }

    public static bool StartHydrateDydrate(ICoreAPI api)
    {
        if (api.ModLoader.IsModEnabled("hydrateordiedrate"))
        {
            if (BetterNutritionConfig.Config.Integration.HydrateOrDiedrate.Enable && (
                    BetterNutritionConfig.Config.Integration.HydrateOrDiedrate.CoolingMultiplier.Enable || BetterNutritionConfig.Config.Integration.HydrateOrDiedrate.ThirstRate.Enable))
            {
                HydrateOrDiedrateEnabled = true;
                BetterNutritionModSystem.Log("Mod Hydrate Or Diedrate Detected: Enabled Hydrate or Diedrate Integration");
                return HydrateOrDiedrateEnabled;
            }
            else
            {
                BetterNutritionModSystem.Log("Mod Hydrate Or Diedrate Detected: Config has integration disabled");
            }
        }
        HydrateOrDiedrateEnabled = false;
        return HydrateOrDiedrateEnabled;
    }
}