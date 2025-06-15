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

public class CreateGameObject : MonoBehaviour
{
	void Start()
	{
		// Create a new GameObject with the name "MyCube" and a Cube primitive.
		GameObject cube = new GameObject("MyCube", typeof(MeshFilter), typeof(MeshRenderer));

		// Alternatively, create a simple GameObject with just a Transform.
		GameObject simpleObject = new GameObject("SimpleObject");

		// Add a Rigidbody component.
		// Rigidbody rb = simpleObject.AddComponent<Rigidbody>();

		// Add a MeshFilter and MeshRenderer to create a cube.
		MeshFilter meshFilter = cube.GetComponent<MeshFilter>();
		MeshRenderer meshRenderer = cube.GetComponent<MeshRenderer>();

		// Create a basic cube mesh.
		meshFilter.mesh = new Mesh();
		meshFilter.mesh.vertices = new Vector3[] {
			new Vector3(-0.5f, -0.5f, -0.5f),
			new Vector3(0.5f, -0.5f, -0.5f),
			new Vector3(0.5f, 0.5f, -0.5f),
			new Vector3(-0.5f, 0.5f, -0.5f),
			new Vector3(-0.5f, 0.5f, 0.5f),
			new Vector3(0.5f, 0.5f, 0.5f),
			new Vector3(0.5f, -0.5f, 0.5f),
			new Vector3(-0.5f, -0.5f, 0.5f)
		};
		meshFilter.mesh.triangles = new int[] {
			0, 2, 1, 0, 3, 2,
			4, 6, 5, 4, 7, 6,
			1, 2, 5, 1, 5, 6,
			0, 7, 4, 0, 4, 3,
			3, 4, 5, 3, 5, 2,
			1, 6, 7, 1, 7, 0
		};
		meshFilter.mesh.RecalculateNormals();

		// Create a basic material.
		Material material = new Material(Shader.Find("Standard"));
		material.color = Color.red;
		meshRenderer.material = material;
	}
}

public class AddChild : MonoBehaviour
{
	public GameObject parentObject;
	public GameObject prefabToInstantiate;

	void Start()
	{
		// Create a new GameObject
		GameObject newChild = Instantiate(prefabToInstantiate);

		// Set the parent of the new GameObject
		newChild.transform.SetParent(parentObject.transform);

		// Optional: Reset the local position and rotation to avoid unexpected behavior
		newChild.transform.localPosition = Vector3.zero;
		newChild.transform.localRotation = Quaternion.identity;
	}
}
