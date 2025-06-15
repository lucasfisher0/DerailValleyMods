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
using UnityEngine.UI;
using UnityModManagerNet;

using MySummerTrain.Utility;
using UnityEngine.UI;
using ElementType = DV.UI.CanvasController.ElementType;

namespace MySummerTrain.UI;

// TODO: possibly make this a SingletonBehaviour?
public class SurvivalUIOld : MonoBehaviour
{















	// Called from GameUISetup.Start postfix with [NewCanvasController]
	// public static void Initialize(GameObject gameCanvas)



	public static void Initialize(GameObject gameCanvas)
	{

		Resources.Load(""),
		const string expectedName = "[NewCanvasController]";
		if (gameCanvas.name != expectedName)
			Logging.Warning($"SurvivalUI.Initialize called with object \"{gameCanvas.name}\", expected \"{expectedName}\"");

		GameObject canvasObject = new GameObject("SurvivalCanvas", typeof(Canvas), typeof(CanvasScaler), typeof(GridLayoutGroup));

		// Transform
		var rectTransform = canvasObject.GetComponent<RectTransform>();
		rectTransform.SetParent(gameCanvas.transform);
		rectTransform.anchorMin = Vector2.up;
		rectTransform.anchorMax = Vector2.up;
		rectTransform.anchoredPosition = new Vector2(100f, -40f);
		//rectTransform.offsetMax = Vector2.zero;
		//rectTransform.offsetMin = Vector2.zero;
		rectTransform.localScale = Vector3.one;
		rectTransform.localRotation = Quaternion.identity;

		// Scaling
		var canvasScaler = canvasObject.GetComponent<CanvasScaler>();
		canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
		canvasScaler.referenceResolution = new Vector2(1920, 1080);

		// Grid Layout
		var gridLayout = canvasObject.GetComponent<GridLayoutGroup>();
		gridLayout.cellSize = new Vector2(300f, 40f);
		gridLayout.spacing = new Vector2(20f, 10f);
		gridLayout.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
		gridLayout.constraintCount = 2;








	}

	public enum BarType
	{
		None,
		Slider,
		Text
	}

	public GameObject GenerateBar(GameObject parent, string name, BarType type = BarType.Slider)
	{
		var obj = new GameObject($"{name}Bar");
		var rectTransform = obj.GetComponent<RectTransform>();
		rectTransform.anchorMin = Vector2.zero;
		rectTransform.anchorMax = Vector2.one;
		rectTransform.sizeDelta = Vector2.zero;

		var image = obj.AddComponent<RawImage>();
		image.color = Color.black;

		if (type == BarType.Slider)
		{
			var fillObj = Instantiate(objectToDuplicate);


		}
		else if (type == BarType.Text)
		{

		}

		return obj;
	}

}
