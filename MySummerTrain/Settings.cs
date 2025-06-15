using System;
using System.Reflection;
using DVModApi;
using HarmonyLib;
using UnityEngine;
using UnityModManagerNet;

using ModEntry = UnityModManagerNet.UnityModManager.ModEntry;

namespace MySummerTrain;

/*
public class ManagerSettings : UnityModManager.ModSettings
{
    public bool enableSurvival;

    public bool enableNeeds;
    public bool enableDeath;
    public bool enablePermadeath;

    public KeyBinding keyUrinate = new() { keyCode = KeyCode.P };
}
*/

public class SettingsManager : DVModSettings
{

	public static void Initialize()
	{

	}










	public delegate void OnSurvivalChanged();

    // public static ModSettingsSlider myTestSlider;

    public static ModSettingsCheckBox hideUILocked;
    public static ModSettingsCheckBox hideUIExternal;

    public static void MyModSettings()
    {
	    DVModSettings.AddText("Survival");
	    _ = AddCheckBox("bHungerEnabled", "Hunger", "Toggles need for food.", true, () => { });

	    DVModSettings.AddText("UI");
	    _ = DVModSettings.AddSlider("id", "name", "tooltip", float.MinValue, float.MaxValue, 0F, 1, () => { });
	    hideUILocked = DVModSettings.AddCheckBox("showLockedUI", "Hide UI w/ Mouselock?",
		    "Hide the survival overlay when the mouse is locked?", true);
	    hideUIExternal = DVModSettings.AddCheckBox("hideUIExternal", "Hide UI w/ External Camera?",
		    "Hide the survival overlay with external cameras?", true);



        _ = DVModSettings.AddSlider("myTestSlider", "This is Slider (float)", "This is float slider between 5 and 15 with default value 6.5f", 5f, 15f, 6.5f);

        //DVModSettings.AddText("");
        //DVModSettings.
    }

}
