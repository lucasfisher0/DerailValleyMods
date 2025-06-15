using System;
using System.Collections.Generic;
using System.Reflection;
using HarmonyLib;
using UnityEngine;
using UnityModManagerNet;
using DV.Utils;

namespace MySummerTrain.Survival;

[ExecuteAfter(typeof(CustomFirstPersonController))]
public class DrunkCamera : MonoBehaviour
{
    public Transform head;
    private CustomFirstPersonController fpc;

    private void Awake()
    {

    }

    private void Start()
    {

    }

    private void Update()
    {

    }

    private void OnDestroy()
    {

    }
    

}