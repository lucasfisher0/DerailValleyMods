using System;
using System.Collections.Generic;
using System.Reflection;
using DV.Localization;
using DV.UI.Manual;
using DV.UI;
using DV.UI;
using DV.UIFramework;
using DV.Utils;
using HarmonyLib;
using I2.Loc;

using System.IO;
using TMPro;
using UnityEngine.UIElements;
using UnityEngine;
using UnityModManagerNet;

using MySummerTrain.Utility;

using ElementType = DV.UI.CanvasController.ElementType;

namespace MySummerTrain.UI;

[HarmonyPatch(typeof(GameUISetup), "Start")]
public class GameUISetup_Patch
{
	public static GameObject? Desktop;
	public static GameObject? VR;
	public static GameObject? Common;

	static void Postfix(ref GameUISetup __instance)
	{
		Desktop = __instance.nonVR;
		VR = __instance.vr;
		Common = __instance.common;

		if (VRManager.IsVREnabled())
		{
			Logging.Warning("VR is not currently supported, custom UI not being initialized.");
			return;
		}

		__instance.transform.parent.GetChild(0);
	}
}
