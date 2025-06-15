using System;
using System.Collections.Generic;
using System.Reflection;
using DV.UI.Manual;
using HarmonyLib;
using UnityModManagerNet;
using System.IO;
using DV.UI;
using DV.Localization;
using I2.Loc;

namespace MySummerTrain.UI;

[HarmonyPatch(typeof(ManualDataLoader), nameof(ManualDataLoader.GetLocalizedData))]
public static class ManualDataLoader_Patch
{
    static void Postfix(ref ManualDisplayData __result)
    {
        var modDirectory = Main.mod.Path;
        Main.mod.Logger.Log($"Looking for manual files: {modDirectory}");
        if (string.IsNullOrWhiteSpace(modDirectory))
            return;

        // Localization options
        string langCode = string.IsNullOrWhiteSpace(LocalizationManager.CurrentLanguageCode)
            ? "en"
            : LocalizationManager.CurrentLanguageCode.ToLowerInvariant();

        langCode = (langCode == "zh-ch") ? "zh-hans" : langCode;
        langCode = (langCode == "zh-tw") ? "zh-hant" : langCode;

        foreach (var manualFile in Directory.EnumerateFiles(modDirectory, "manual*.json", SearchOption.AllDirectories))
        {
            try
            {
                Main.mod.Logger.Log($"Loading custom manual file: {manualFile}");
                var manualDir = Directory.GetParent(manualFile);
                ManualMetadata manualMetadata = ManualMetadata.FromJson(File.ReadAllText(manualFile));

                // Load strings from the same folder
                var stringsFile = Path.Combine(manualDir.FullName, langCode) + ".json";
                ManualStrings? manualStrings = null;
                ManualStrings? fallbackStrings = null;

                if (!File.Exists(Path.Combine(manualDir.FullName, "en") + ".json"))
                {
                    Main.mod.Logger.Warning($"Could not find English strings! Skipping manual...");
                    continue;
                }

                if (!string.Equals(langCode, "en", StringComparison.InvariantCultureIgnoreCase) && File.Exists(stringsFile))
                {
                    manualStrings = ManualStrings.FromJson(File.ReadAllText(stringsFile));
                    fallbackStrings = ManualStrings.FromJson(File.ReadAllText(Path.Combine(manualDir.FullName, "en") + ".json"));
                }
                else
                {
                    stringsFile = Path.Combine(manualDir.FullName, "en") + ".json";
                    manualStrings = ManualStrings.FromJson(File.ReadAllText(stringsFile));
                    fallbackStrings = manualStrings;
                }

                manualStrings.meta ??= [];
                fallbackStrings.meta ??= [];

                // TODO: SAFETY CHECK, there cannot be a new node with an existing key name.

                // Add manual to root
                var copyDataMethod = typeof(ManualDisplayData).GetMethod("CopyDataToTreeNodes", BindingFlags.NonPublic | BindingFlags.Instance);
                copyDataMethod!.Invoke(__result, [manualMetadata.tree, __result.root, manualStrings, fallbackStrings]);
                __result.root.children.Add(manualMetadata.tree);

                var prevNextMethod = typeof(ManualDisplayData).GetMethod("CalculatePreviousNext", BindingFlags.NonPublic | BindingFlags.Instance);
                prevNextMethod!.Invoke(__result, [__result.root]);

                Main.mod.Logger.Log($"Manual {manualFile} successfully added.");
            }
            catch (Exception ex)
            {
                Main.mod.Logger.LogException($"Error adding manual {manualFile}:", ex);
                continue;
            }
        }
    }
}
