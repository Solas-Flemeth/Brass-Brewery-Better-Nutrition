using System;
using HarmonyLib;
using Vintagestory.API.Client;
using Vintagestory.API.Server;
using Vintagestory.API.Common;

namespace BetterNutrition;

public class BetterNutritionModSystem : ModSystem
{
    private Harmony? _harmony;
    public override void Start(ICoreAPI api)
    {
        Mod.Logger.Notification("Loading Mod: " + Mod.Info.ModID);
        //load config
        BetterNutritionConfig.LoadConfig(Mod, api);
        //harmony patching
        _harmony = new Harmony(Mod.Info.ModID);
        ApplyPatches(_harmony, api);
        Mod.Logger.Notification("Mod Loaded: " + Mod.Info.ModID);
    }
    
    public override double ExecuteOrder()
    {
        return 0.5;
    }

    public override void StartServerSide(ICoreServerAPI api)
    {
        api.Event.PlayerCreate += UpdateBonusSatiety.OnPlayerCreate;
        api.Event.PlayerJoin += UpdateBonusSatiety.OnPlayerJoin;
    }

    public override void StartClientSide(ICoreClientAPI api)
    {
        
    }
    public override void Dispose()
    {
        _harmony?.UnpatchAll(Mod.Info.ModID);
    }
    
    private void ApplyPatches(Harmony harmony, ICoreAPI api)
    {
        Mod.Logger.Notification("Patching Game");
        _harmony.PatchAll();
        /*
        if (api.ModLoader.IsModEnabled("xskills") || api.ModLoader.IsModEnabled("xskillsfork"))
        {
            Mod.Logger.Notification("Mod XSkills Detect: Patching HugeStomach for compatibility");
        }
        */
        //future mod patches here
    }
}