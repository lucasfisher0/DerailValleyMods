using System;
using System.Collections.Generic;
using System.Reflection;
using HarmonyLib;
using UnityModManagerNet;

namespace MySummerTrain.Survival;

public enum SurvivalStatType
{
    Thirst,
    Hunger,
    Stress,
    Urine,
    Fatigue,
    Dirtiness,
    Alcohol
}

public struct SurvivalStat
{
    // Current Value
    public float Current;

    // Maximum Value
    public float Maximum;

    // Drain per in-game hour
    public float Drain;
}