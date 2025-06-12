
using System;
using System.Reflection;
using HarmonyLib;
using UnityModManagerNet;
using DV.Localization;

using ModEntry = UnityModManagerNet.UnityModManager.ModEntry;
using FastTravelDestination = DV.Teleporters.FastTravelDestination;
using FastTravelData = DV.UI.FastTravelData;

namespace FastTravelModelName;

[EnableReloading]
static class FastTravelModelName
{
    public static bool Load(ModEntry modEntry)
    {
        Harmony? harmony = new Harmony(modEntry.Info.Id);
        try
        {
            harmony = new Harmony(modEntry.Info.Id);
            harmony.PatchAll(Assembly.GetExecutingAssembly());
            modEntry.Logger.Log($"{modEntry.Info.DisplayName} loaded.");
        }
        catch (Exception ex)
        {
            modEntry.Logger.LogException($"Failed to load {modEntry.Info.DisplayName}:", ex);
            harmony?.UnpatchAll(modEntry.Info.Id);
            return false;
        }

        return true;
    }

    static bool Unload(ModEntry modEntry)
    {
        modEntry.Logger.Log("Map restart still required to regenerate overview book textures.");
        Harmony harmony = new Harmony(modEntry.Info.Id);
        harmony.UnpatchAll();

        return true;
    }
}

[HarmonyPatch(typeof(FastTravelController), "ExtractFastTravelData")]
public static class FastTravelPatch
{
    static void Postfix(ref FastTravelData __result, FastTravelDestination fastTravelMarker)
    {
        if (__result.isDestinationLoco)
        {
            if (fastTravelMarker.playerTeleportAnchor.TryGetComponent<TrainCar>(out var component))
            {
                string modelLabel = LocalizationAPI.L(component.carLivery.parentType.localizationKey);
                __result.destinationName = $"{__result.destinationName} [{modelLabel}]";
            }
        }
    }
}