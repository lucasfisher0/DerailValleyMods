using DV.UI;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityModManagerNet;

namespace BetterModUI
{
    public class ModElement : MonoBehaviour
    {
        public ModMenu menu;
        public TextMeshProUGUI modText;
        public Image modStatus;
        public ButtonDVMarkable modButton;
        public UIElementTooltipNonLocalizedText modTooltip;
        public string modStatusText = "Status: Unknown!";
        private bool openSettings = false;
#if !Mini
        UnityModManager.ModEntry mod;
        public void Setup(ModMenu mm, UnityModManager.ModEntry m)
        {
            menu = mm;
            mod = m;
            modButton.Clicked += (c) => menu.ShowModInfo(this, m);
            modTooltip.text = $"<color=white>Click to show more about</color> {m.Info.DisplayName} <color=white>by</color> <color=yellow>{m.Info.Author}</color>";
            modText.font = BetterModUI.mainFont;
            if (m.NewestVersion != null)
            {
                modText.text = $"<color=#F29839>{m.Info.DisplayName}</color>{Environment.NewLine}<size=18>by <color=yellow>{m.Info.Author}</color> (<color=#00ffffff>{m.Info.Version}</color>) -> (<color=green>{m.NewestVersion} update available</color>)</size>";
            }
            else
            {
                modText.text = $"<color=#F29839>{m.Info.DisplayName}</color>{Environment.NewLine}<size=18>by <color=yellow>{m.Info.Author}</color> (<color=#00ffffff>{m.Info.Version}</color>)</size>";
            }
            ModStatus();
        }

        public void MarkBtn(bool en)
        {
            modButton.ToggleMarked(en);
        }
        void ModStatus()
        {
            //Recreated from UMM source
            if (mod.ErrorOnLoading)
            {
                modStatusText = $"Status: <color=red>Error!!!</color>";
                modStatus.color = Color.red;
            }
            else
            {
                if (mod.Active)
                {
                    if (mod.Enabled)
                    {
                        modStatusText = $"Status: <color=green>OK!</color>";
                        modStatus.color = Color.green;
                    }
                    else
                    {
                        modStatusText = $"Status: <color=yellow>Needs restart!</color>";
                        modStatus.color = Color.yellow;
                    }
                }
                else
                {
                    if (mod.Enabled)
                    {
                        modStatusText = $"Status: <color=yellow>Needs restart!</color>";
                        modStatus.color = Color.yellow;

                    }
                    else
                    {
                        modStatusText = $"Status: <color=#808080ff>Inactive!</color>";
                        modStatus.color = new Color32(100, 100, 100, 255);
                    }
                }
            }
        }
        public void ToggleMod(bool action)
        {
            mod.Enabled = action;
            if (mod.Toggleable)
                mod.Active = action;
            else if (action && !mod.Loaded)
                mod.Active = action;
            ModStatus();
            menu.SetDetailsText(mod);
        }
        
        void OnGUI()
        {
            if (mod == null) return;
            //Recreated part from UMM source (to decouple settings from UMM menu)
            if (mod.Active && mod.OnGUI != null)
            {
                if (!openSettings) return;
                if (BetterModUI.ummSetSkin.GetValue())
                {
                    GUI.skin = DVGUI.skin;
                    UnityModManager.UI.bold = GUIStyle.none;
                }
                else
                    GUI.skin = null;
                GUI.Window(5492, new Rect((Screen.width / 2) - (Screen.width / 2) / 2, (Screen.height / 2) - (Screen.height / 2) / 2, Screen.width / 2, Screen.height / 2), new GUI.WindowFunction(GuiSettingsWindow), $"{mod.Info.DisplayName} - Settings");
            }

        }
        Vector2 scrollPosition = Vector2.zero;
        private void GuiSettingsWindow(int windowId)
        {
            scrollPosition = GUILayout.BeginScrollView(scrollPosition, GUILayout.MinWidth((Screen.width / 2) - 10));
            GUILayout.BeginVertical("box");
            try
            {
                mod.OnGUI(mod);
            }

            catch (Exception e)
            {
                mod.Logger.LogException("OnGUI", e);
                ToggleSettings(false);
                GUIUtility.ExitGUI();
            }
            GUILayout.EndVertical();
            GUILayout.EndScrollView();
            if (GUILayout.Button("Close Settings Window"))
            {
                ToggleSettings(false);
                if (mod.Active && mod.OnHideGUI != null)
                {
                    try
                    {
                        mod.OnHideGUI(mod);
                    }
                    catch (ExitGUIException)
                    {
                    }
                    catch (Exception ex)
                    {
                        mod.Logger.LogException("OnHideGUI", ex);
                    }
                }
                UnityModManager.SaveSettingsAndParams();
            }
            GUI.skin = null;
        }
        internal void ToggleSettings(bool op)
        {
           // if (GetComponentInParent<ModMenu>().settingOpened && op) return;
            openSettings = op;
          //  GetComponentInParent<ModMenu>().settingOpened = op;
            if (op)
            {
                menu.UMMSettingsMask.SetActive(true);
                if (mod.Active && mod.OnShowGUI != null && mod.OnGUI != null)
                {
                    try
                    {
                        mod.OnShowGUI(mod);
                    }
                    catch (ExitGUIException)
                    {
                    }
                    catch (Exception ex)
                    {
                        mod.Logger.LogException("OnShowGUI", ex);
                    }
                }
            }
            else
            {
                menu.UMMSettingsMask.SetActive(false);
            }
        }       
#endif
    }
}
