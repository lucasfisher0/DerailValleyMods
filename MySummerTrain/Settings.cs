using System;
using System.Reflection;
using DVModApi;
using HarmonyLib;
using UnityEngine;
using UnityModManagerNet;

using ModEntry = UnityModManagerNet.UnityModManager.ModEntry;

namespace MySummerTrain;

public class Settings : UnityModManager.ModSettings
{
    public bool enableSurvival;

    public KeyBinding keyUrinate = new() { keyCode = KeyCode.P };

}

public class IngameSettings : DVModSettings
{
    public static ModSettingsSlider myTestSlider;

    public static void MyModSettings()
    {
        myTestSlider = DVModSettings.AddSlider("myTestSlider", "This is Slider (float)", "This is float slider between 5 and 15 with default value 6.5f", 5f, 15f, 6.5f);

        //DVModSettings.AddText("");
        //DVModSettings.
    }
    
}