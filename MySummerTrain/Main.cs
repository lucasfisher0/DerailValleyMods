using System;
using System.Reflection;
using DVModApi;
using HarmonyLib;
using UnityEngine;
using UnityModManagerNet;

using ModEntry = UnityModManagerNet.UnityModManager.ModEntry;

namespace MySummerTrain;

public static class Main
{
	public static Settings? settings;
	private static Harmony? _harmony;


	// Unity Mod Manage Wiki: https://wiki.nexusmods.com/index.php/Category:Unity_Mod_Manager
	private static bool Load(ModEntry modEntry)
	{
		try
		{
			_harmony ??= new Harmony(modEntry.Info.Id);
			_harmony.PatchAll(Assembly.GetExecutingAssembly());

			// Other plugin startup logic
			modEntry.OnGUI = OnGUI;
			modEntry.OnSaveGUI = OnSaveGUI;
			DVModAPI.Setup(modEntry, FunctionType.OnGameLoad, OnGameLoad);
		}
		catch (Exception ex)
		{
			modEntry.Logger.LogException($"Failed to load {modEntry.Info.DisplayName}:", ex);
			_harmony!.UnpatchAll(modEntry.Info.Id);
			return false;
		}

		return true;
	}

	static void OnGUI(ModEntry modEntry)
	{
		GUILayout.Label("TODO: Settings");
		// GUILayout.Label("Maximum number of locomotives:     (Default = 30");
		// settings.LocoLimit = (int)GUILayout.HorizontalSlider(settings.LocoLimit, 1, 50);
		// GUILayout.Label($"Current limit: {settings.LocoLimit}");

		// GUILayout.RepeatButton
	}

	static void OnSaveGUI(ModEntry modEntry)
	{
		settings!.Save(modEntry);
	}

	private static void OnGameLoad()
	{
		//Do something after Loading screen finishes
	}

	private static void OnMenuLoad(bool returnedToMenu)
	{
		//Do something when main menu is fully loaded
	}

	private static void OnSave()
	{
		//Do something when game saves
	}
}
