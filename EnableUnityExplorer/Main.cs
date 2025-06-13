
using System;
using HarmonyLib;
using UnityModManagerNet;
using UnityExplorer;

using ModEntry = UnityModManagerNet.UnityModManager.ModEntry;
using UnityEngine;

namespace EnableUnityExplorer;

static class Main
{
    private static ModEntry? _modEntry;
    private static string _modPrefix = string.Empty;

    public static bool Load(ModEntry modEntry)
    {
        _modEntry = modEntry;
        _modPrefix = $"[{modEntry.Info.DisplayName}]:";

        try
        {
            ExplorerStandalone.CreateInstance(LogHandler);
            modEntry.Logger.Log($"{_modPrefix} loaded.");
        }
        catch (Exception ex)
        {
            modEntry.Logger.LogException($"{_modPrefix} Failed to load:", ex);
            return false;
        }

        return true;
    }

    public static void LogHandler(string message, LogType type)
    {
        switch (type)
        {
            case LogType.Error:
            case LogType.Assert:
            case LogType.Exception:
                _modEntry!.Logger.Error($"{_modPrefix} {message}");
                break;
            case LogType.Warning:
                _modEntry!.Logger.Warning($"{_modPrefix} {message}");
                break;
            case LogType.Log:
            default:
                _modEntry!.Logger.Log($"{_modPrefix} {message}");
                break;
        }
    }
}