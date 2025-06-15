#if !Mini
using UnityModManagerNet;
using UnityEngine;
using System;
using TMPro;
using System.Linq;
using DV.UIFramework;
using System.Reflection;
using DVModApi;
using DV.UI;

namespace BetterModUI
{
    public static class BetterModUI
    {
        internal static TMP_FontAsset mainFont;
        internal static ModSettingsCheckBox ummSetSkin;
        static UnityModManager.ModEntry modEntry;
        static GameObject goMainBtnP, goMainUIP;
        static bool Load(UnityModManager.ModEntry m)
        {
            modEntry = m;
            DVModAPI.Setup(m, FunctionType.OnMenuLoad, DVMenuLoad);
            DVModAPI.Setup(m, FunctionType.OnGameLoad, DVGameLoad);
            DVModAPI.Setup(m, FunctionType.ModSettings, DModSettings);
            return true;
        }
        private static void DModSettings()
        {
            DVModSettings.AddText("Better Mod UI Settings");
            ummSetSkin = DVModSettings.AddCheckBox("UMMSetSkin", "Enable Skin for UMM Settings", $"Use DV Debug Skin for UMM settings popup{Environment.NewLine}If unchecked it will use Unity default skin.", true);
        }
        private static void DVMenuLoad(bool returnedToMenu)
        {
            CreateModMenuUI(false);
        }
        private static void DVGameLoad()
        {
            CreateModMenuUI(true);
        }
        public static void CreateModMenuUI(bool pauseMenu)
        {
            if (mainFont == null)
                mainFont = Resources.FindObjectsOfTypeAll<TMP_FontAsset>().Where(x => x.name.Contains("__MAIN__")).FirstOrDefault();
            //Load Prefabs from bundle (if not already loaded)
            if (goMainBtnP == null)
            {
                AssetBundle ab = DVModAssets.LoadAssetBundle(modEntry, "modui");
                goMainBtnP = ab.LoadAsset<GameObject>("ButtonSelectable Mods.prefab");
                goMainUIP = ab.LoadAsset<GameObject>("PaneRight Mods.prefab");
                ab.Unload(false); //Unload bundle
            }

            //Instantiate UI
            GameObject goMainBtn = GameObject.Instantiate(goMainBtnP);
            GameObject goMainUI = GameObject.Instantiate(goMainUIP);
            goMainBtn.name = goMainBtn.name.Replace("(Clone)", string.Empty);
            goMainUI.name = goMainUI.name.Replace("(Clone)", string.Empty);

            //Replace default font with main game font
            for (int i = 0; i < goMainBtn.GetComponentsInChildren<TextMeshProUGUI>().Length; i++)
            {
                TextMeshProUGUI g = goMainBtn.GetComponentsInChildren<TextMeshProUGUI>(true)[i];
                g.font = mainFont;
            }
            for (int i = 0; i < goMainUI.GetComponentsInChildren<TextMeshProUGUI>().Length; i++)
            {
                TextMeshProUGUI g2 = goMainUI.GetComponentsInChildren<TextMeshProUGUI>(true)[i];
                g2.font = mainFont;
            }

            //Put UI in Main Menu
            if (!pauseMenu)
            {
                UIMenuController umc = GameObject.Find("MenuOpeningScene/PivotRight/WINDOW MainMenu PaneRight/menus and controllers").GetComponent<UIMenuController>();
                goMainBtn.transform.SetParent(GameObject.Find("MenuOpeningScene/PivotLeft/WINDOW MainMenu PaneLeft/PaneLeft MainMenu/Buttons").transform, false);
                goMainBtn.transform.SetSiblingIndex(2);
                goMainUI.transform.SetParent(umc.transform, false);
                umc.controlledMenus.Add(goMainUI.GetComponent<UIMenu>());
                UIMenuRequester uimr = goMainBtn.AddComponent<UIMenuRequester>();
                Debug.LogWarning("...But it will after that");
                uimr.targetMenuController = umc;
                uimr.requestedMenuIndex = umc.controlledMenus.Count - 1;
                //Reinvoke Awake since it lacks it in OnEnable
                uimr.GetType().GetMethod("Awake", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance, null, new Type[] { }, null).Invoke(uimr, null);
                uimr.enabled = true;
                return;
            }

            //Put UI in Pause Menu (Same prefabs)
            UIMenuController umc2 = GameObject.Find("[MAIN]").transform.Find("[GameUI]/[NewCanvasController]/Auxiliary Canvas, EventSystem, Input Module/scripts - pause menu/PauseMenuRoot").GetComponent<UIMenuController>();
            goMainBtn.transform.SetParent(GameObject.Find("[MAIN]").transform.Find("[GameUI]/[NewCanvasController]/Auxiliary Canvas, EventSystem, Input Module/scripts - pause menu/PauseMenuRoot/Pivot/PauseMenuBasic/buttons").transform, false);
            goMainBtn.transform.SetSiblingIndex(2);
            goMainUI.transform.SetParent(GameObject.Find("[MAIN]").transform.Find("[GameUI]/[NewCanvasController]/Auxiliary Canvas, EventSystem, Input Module/scripts - pause menu/PauseMenuRoot/Pivot/submenus").transform, false);
            umc2.controlledMenus.Add(goMainUI.GetComponent<UIMenu>());
            goMainBtn.GetComponent<ButtonDVMarkable>().Clicked += (c) => umc2.SwitchMenu(umc2.controlledMenus.Count - 1);
            //Pause Menu Paths...
            //[MAIN]/[GameUI]/[NewCanvasController]/Auxiliary Canvas, EventSystem, Input Module/scripts - pause menu/PauseMenuRoot/Pivot/PauseMenuBasic/buttons
            //[MAIN]/[GameUI]/[NewCanvasController]/Auxiliary Canvas, EventSystem, Input Module/scripts - pause menu/PauseMenuRoot/Pivot/submenus
            //[MAIN]/[GameUI]/[NewCanvasController]/Auxiliary Canvas, EventSystem, Input Module/scripts - pause menu/PauseMenuRoot
        }
    }
}
#endif