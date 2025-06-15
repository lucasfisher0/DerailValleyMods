using System;
using System.Reflection;
using DVModApi;
using HarmonyLib;
using UnityEngine;
using UnityModManagerNet;

using ModEntry = UnityModManagerNet.UnityModManager.ModEntry;

namespace MySummerTrain;

public delegate void GameLoadDelegate();
public delegate void MenuLoadDelegate(bool returnedToMenu);
public delegate void SaveDelegate();

public static class Main
{
	public static ModEntry mod { get; private set; }= null!;
	private static Harmony _harmony = null!;
	private static SettingsManager _settings = new();

	public static GameLoadDelegate OnGameLoaded;
	public static MenuLoadDelegate OnMenuLoaded;
	public static SaveDelegate OnSave;

	// Unity Mod Manage Wiki: https://wiki.nexusmods.com/index.php/Category:Unity_Mod_Manager
	private static bool Load(ModEntry modEntry)
	{
		_harmony ??= new Harmony(modEntry.Info.Id);
		mod = modEntry;

		try
		{
			_harmony.PatchAll(Assembly.GetExecutingAssembly());

			OnGameLoaded = () => { };
			OnMenuLoaded = (bool returnedToMenu) => { };
			OnSave = () => { };

			DVModAPI.Setup(mod, FunctionType.OnMenuLoad, BroadcastMenuLoaded);
			DVModAPI.Setup(mod, FunctionType.OnGameLoad, BroadcastGameLoaded);
			DVModAPI.Setup(mod, FunctionType.OnSave, BroadcastSave);
			DVModAPI.Setup(mod, FunctionType.ModSettings, SettingsManager.Initialize);
		}
		catch (Exception ex)
		{
			mod.Logger.LogException($"Failed to load {mod.Info.DisplayName}:", ex);
			_harmony.UnpatchAll(mod.Info.Id);
			return false;
		}

		return true;
	}

	private static void BroadcastMenuLoaded(bool returnedToMenu) => OnMenuLoaded(returnedToMenu);
	private static void BroadcastGameLoaded() => OnGameLoaded();
	private static void BroadcastSave() => OnSave();
}
