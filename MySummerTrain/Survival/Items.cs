using System;
using System.Collections.Generic;
using System.Reflection;
using HarmonyLib;
using UnityModManagerNet;

namespace MySummerTrain.Survival;

public interface ISurvivalItem
{
    public bool IsEdible { get; set; }
    public bool LeavesTrash { get; set; }

    public Dictionary<SurvivalStatType, SurvivalStat> IngestionStats { get; set; }
}