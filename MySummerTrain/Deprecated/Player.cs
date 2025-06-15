using System;
using System.Collections.Generic;
using System.Reflection;
using HarmonyLib;
using UnityModManagerNet;
using UnityEngine;

namespace MySummerTrain.Survival;

public static class PlayerSurvival
{
    public static Dictionary<SurvivalStatType, SurvivalStat>? Stats;

    public static void Initialize()
    {
        Stats = new()
        {
            { SurvivalStatType.Thirst,    new() { Current = 100f, Maximum = 100f, Drain = 4f } },
            { SurvivalStatType.Hunger,    new() { Current = 100f, Maximum = 100f, Drain = 8f } },
            { SurvivalStatType.Stress,    new() { Current = 0f,   Maximum = 100f, Drain = 2f } },
            { SurvivalStatType.Urine,     new() { Current = 0f,   Maximum = 100f, Drain = 0f } },
            { SurvivalStatType.Fatigue,   new() { Current = 0f,   Maximum = 100f, Drain = 0f } },
            { SurvivalStatType.Dirtiness, new() { Current = 0f,   Maximum = 100f, Drain = -10f } },
            { SurvivalStatType.Alcohol,   new() { Current = 0f,   Maximum = 200f, Drain = 15f } }
        };
    }
}

[HarmonyPatch(typeof(PlayerCameraSwitcher), "PlayerChanged")]
public class PlayerCameraSwitcher_Patch
{
    private void Postfix(PlayerCameraSwitcher __instance)
    {
        var scripts = Traverse.Create<PlayerCameraSwitcher>()
                       .Field<List<MonoBehaviour>>("playerScripts");

        var scriptsList = scripts.Value; // Is this passed by reference? Do 

    }

}