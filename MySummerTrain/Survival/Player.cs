using System;
using System.Collections.Generic;
using System.Reflection;
using HarmonyLib;
using UnityModManagerNet;

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
            { SurvivalStatType.Stress,    new() { Current = 100f, Maximum = 100f, Drain = 2f } },
            { SurvivalStatType.Urine,     new() { Current = 0f,   Maximum = 100f, Drain = 0f } },
            { SurvivalStatType.Fatigue,   new() { Current = 100f, Maximum = 100f, Drain = 0f } },
            { SurvivalStatType.Dirtiness, new() { Current = 100f, Maximum = 100f, Drain = 10f } },
            { SurvivalStatType.Alcohol,   new() { Current = 0f,   Maximum = 200f, Drain = 15f } }
        };
    }
}