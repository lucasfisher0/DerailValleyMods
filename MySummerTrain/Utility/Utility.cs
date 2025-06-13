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


namespace MySummerTrain.Utility;

public class PlayerUtility
{
    public static CustomFirstPersonController? GetFirstPersonController()
    {
        if (!(PlayerManager.PlayerTransform != null))
        {
            return null;
        }
        return PlayerManager.PlayerTransform.GetComponent<CustomFirstPersonController>();
    }

    public static bool IsInFirstPerson() => PlayerCameraSwitcher.IsInFirstPerson;
}

/********************************************************************


AStartGameData startGameData = SingletonBehaviour<SaveGameManager>.Instance.FindStartGameData();
		if (startGameData == null)
		{
			Debug.LogError("Got null SaveGameData, starting new career as fallback");
			this.Info("loading/start_game_data_missing");
			yield return null;
			startGameData = AStartGameData.FallbackNewCareer();
		}
		SaveGameData saveGame = startGameData.GetSaveGameData();





*/
