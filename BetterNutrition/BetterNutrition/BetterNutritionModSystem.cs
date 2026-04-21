using System;
using HarmonyLib;
using Vintagestory.API.Client;
using Vintagestory.API.Server;
using Vintagestory.API.Config;
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
        _harmony.PatchAll();
        Mod.Logger.Notification("Mod Loaded: " + Mod.Info.ModID);
    }

    public override void StartServerSide(ICoreServerAPI api)
    {
    }

    public override void StartClientSide(ICoreClientAPI api)
    {
        
    }
    public override void Dispose()
    {
        _harmony?.UnpatchAll(Mod.Info.ModID);
    }
}