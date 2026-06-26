
using System;
using HarmonyLib;
using Vintagestory.API.Client;
using Vintagestory.API.Server;
using Vintagestory.API.Common;

namespace BetterNutrition;

public class BetterNutritionModSystem : ModSystem
{
    private Harmony? _harmony;
    private static BetterNutritionModSystem? Instance { get; set; }
    public override void Start(ICoreAPI api)
    {
        Instance = this;
        if (!BetterNutritionConfig.IsLoaded)
        {
            BetterNutritionConfig.LoadConfig(Mod, api);
        }
        Log("Begin Loading");
        //harmony patching
        _harmony = new Harmony(Mod.Info.ModID);
        ApplyPatches(api);
       Log("Finished Loading");
    }
    
    public override double ExecuteOrder()
    {
        return 0.5;
    }

    public override void StartServerSide(ICoreServerAPI api)
    {
        //Load Systems and listeners
        if (BetterNutritionConfig.Config.Nutrition.Enable)
        {
            Log("Enabling Nutrition System");
            NutritionSystem nutritionSystem = new NutritionSystem(api);
            api.Event.RegisterGameTickListener(nutritionSystem.PlayerNutritionUpdateCheck, (int) (BetterNutritionConfig.Config.Nutrition.UpdateFrequency*1000f));
        }

        if (BetterNutritionConfig.Config.Regeneration.Enable)
        {
            Log("Enabling Regeneration System");
            RegenerationSystem regenerationSystem = new RegenerationSystem(api);
            api.Event.RegisterGameTickListener(regenerationSystem.OnTick, (int) (BetterNutritionConfig.Config.Regeneration.TickRate*1000f));
        }
        api.Event.PlayerJoin += BetterNutritionModSystem.OnPlayerJoin;
        api.Event.PlayerLeave += BetterNutritionModSystem.OnPlayerLeave;
        api.Event.PlayerRespawn += BetterNutritionModSystem.OnPlayerRespawn;

    }

    public override void StartClientSide(ICoreClientAPI api) 
    {

    }
    public override void Dispose()
    {
        _harmony?.UnpatchAll(Mod.Info.ModID);
    }
    
    private void ApplyPatches(ICoreAPI api)
    {
        Mod.Logger.Notification("Patching Game");
        _harmony.PatchCategory("brassbrewerybetternutrition.base");
        IntegrationController.StartXSkillsIntegration(api);
        IntegrationController.StartHydrateDydrate(api);
        if (api.Side == EnumAppSide.Client)
        {
            if (BetterNutritionConfig.Config.Client.Enable)
            {
               _harmony.PatchCategory("brassbrewerybetternutrition.client");
            }
        }
        //future mod patches here
    }

    public static void Log(String message)
    {
        Instance?.Mod.Logger.Notification(message);
    }
    
    public static void OnPlayerJoin(IServerPlayer player)
    {
        SatietyBonusSystem.OnPlayerJoin(player);
        if (BetterNutritionConfig.Config.Nutrition.Enable)
        {
            NutritionSystem.UpdateNutritionStats(player);
        }
    }

    public static void OnPlayerLeave(IServerPlayer player)
    {
        if (BetterNutritionConfig.Config.Nutrition.Enable)
        {
            NutritionSystem.ResetPlayerStats(player);
        }
    }

    public static void OnPlayerRespawn(IServerPlayer player)
    {
        if (BetterNutritionConfig.Config.Nutrition.Enable)
        {
            NutritionSystem.ResetPlayerStats(player);
        }
    }
}