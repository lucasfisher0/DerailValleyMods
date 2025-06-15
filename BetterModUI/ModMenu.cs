using DV.UI;
using DV.UIFramework;
using System;
using System.IO;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityModManagerNet;
using DVModApi;

namespace BetterModUI
{
    public class ModMenu : MonoBehaviour
    {
        public TextMeshProUGUI titleText, versionText;
               
        [Header("ModList stuff")]
        public GameObject modListContent, modBtnPrefab;
        public ButtonDV modSettingsUMM, modSettingsAPI, openWebsite;
        public ToggleDV modEnabledToggle;
        public TextMeshProUGUI modDetailsText;

        [Header("Bottom tabs")]
        public ButtonDVMarkable modsTab, logsTab;
        public GameObject modList, logDisp;

        [Header("UMM Log Text")]
        public TextMeshProUGUI logText;
        public ButtonDV openFullLog;
        public GameObject UMMSettingsMask;

        [Header("DVModSettings stuff for DVModAPI")]
        public GameObject modCheckBoxP, modSelectorP, modSliderP, modTextP,modBtnP;
        public GameObject modSettingsPanel, modDetailsPanel, modSettingsContent;
#if !Mini
        private ModElement selectedMod;
        void Awake()
        {
            versionText.font = BetterModUI.mainFont;
            versionText.text = $"UMM {UnityModManager.GetVersion()}";
            modsTab.Clicked += ModsTab_Clicked;
            logsTab.Clicked += LogsTab_Clicked;
            openFullLog.Clicked += (clickable) => UnityModManager.OpenUnityFileLog();
            Setup();
        }
        void Setup()
        {
            ToggleButtons(null);
            modDetailsText.text = "";
            UMMSettingsMask.SetActive(false);
            modSettingsPanel.SetActive(false);
        }
        public void SetDetailsText(UnityModManager.ModEntry m)
        {
            modDetailsText.text = $"<size=25>{selectedMod.modStatusText}</size>{Environment.NewLine}<alpha=#50>Id:</color> {m.Info.Id}{Environment.NewLine}<alpha=#50>Name:</color> {m.Info.DisplayName}{Environment.NewLine}<alpha=#50>Version:</color> {m.Info.Version}  {(m.NewestVersion != null ? "=> (<color=green>" + m.NewestVersion.ToString() + "</color> available)" : string.Empty)} {Environment.NewLine}<alpha=#50>Author:</color> {m.Info.Author}{Environment.NewLine}<alpha=#50>Required mods:</color> {(m.Requirements.Count > 0  ? string.Join(",", m.Info.Requirements) : "-")}";
        }
        void ToggleButtons(UnityModManager.ModEntry m, bool canDisable = true)
        {
            modSettingsUMM.gameObject.SetActive(false);
            modSettingsAPI.gameObject.SetActive(false);
            openWebsite.gameObject.SetActive(false);
            modEnabledToggle.gameObject.SetActive(false);
            modSettingsUMM.transform.parent.GetComponent<TextMeshProUGUI>().text = "<alpha=#50>Select mod from list to see details...</color>";
            if (m != null)
            {
                modSettingsUMM.transform.parent.GetComponent<TextMeshProUGUI>().text = string.Empty;
                modEnabledToggle.gameObject.SetActive(true);
                modEnabledToggle.onValueChanged.RemoveAllListeners();
                modEnabledToggle.isOn = m.Enabled;
                modEnabledToggle.onValueChanged.AddListener((en) => selectedMod.ToggleMod(en));
                modEnabledToggle.interactable = canDisable;
                if (m.Active && m.OnGUI != null)
                {
                    modSettingsUMM.gameObject.SetActive(true);
                    modSettingsUMM.onClick.RemoveAllListeners();
                    modSettingsUMM.onClick.AddListener(() => selectedMod.ToggleSettings(true));
                }
                if (!string.IsNullOrEmpty(m.Info.HomePage))
                {
                    openWebsite.gameObject.SetActive(true);
                    openWebsite.onClick.RemoveAllListeners();
                    openWebsite.onClick.AddListener(() => Application.OpenURL(m.Info.HomePage));
                }
                DVModEntry me = DVModAPI.DVModEntries.Where(x => x.modEntry == m).FirstOrDefault();
                if (me != null)
                {
                    if (me.hasModSettings)
                    {
                        modSettingsAPI.gameObject.SetActive(true);
                        modSettingsAPI.onClick.RemoveAllListeners();
                        modSettingsAPI.onClick.AddListener(() => ToggleModAPISettings(me));
                    }
                }
            }
        }
        bool setNeedsSave = false;
        DVModEntry saveSetEntry;
        void ToggleModAPISettings(DVModEntry me)
        {
            SwitchDetailsSettings(true);
            setNeedsSave = true;
            saveSetEntry = me;
            RemoveChildren(modSettingsContent.transform);
            for (int i = 0; i < me.modSettings.Count; i++)
            {
                switch (me.modSettings[i].Type)
                {
                    case SettingsType.CheckBox:
                        GameObject modCheckbox = Instantiate(modCheckBoxP);
                        modCheckbox.transform.SetParent(modSettingsContent.transform, false);
                        modCheckbox.GetComponent<SettingsElement>().SetupCB((ModSettingsCheckBox)me.modSettings[i]);
                        break;
                    case SettingsType.Button:
                        GameObject modBtn = Instantiate(modBtnP);
                        modBtn.transform.SetParent(modSettingsContent.transform, false);
                        modBtn.GetComponent<SettingsElement>().SetupBtn(me.modSettings[i]);
                        break;
                    case SettingsType.Slider:
                        GameObject modSlider = Instantiate(modSliderP);
                        modSlider.transform.SetParent(modSettingsContent.transform, false);
                        modSlider.GetComponent<SettingsElement>().SetupSlider((ModSettingsSlider)me.modSettings[i]);
                        break;
                    case SettingsType.SliderInt:
                        GameObject modSliderInt = Instantiate(modSliderP);
                        modSliderInt.transform.SetParent(modSettingsContent.transform, false);
                        modSliderInt.GetComponent<SettingsElement>().SetupSliderInt((ModSettingsSliderInt)me.modSettings[i]);
                        break;
                    case SettingsType.Text:
                        GameObject modText = Instantiate(modTextP);
                        modText.transform.SetParent(modSettingsContent.transform, false);
                        modText.GetComponent<SettingsElement>().SetupText(me.modSettings[i]);
                        break;
                    case SettingsType.Selector:
                        //TODO: Make selector working correctly
                        break;
                }
            }

        }
        void OnEnable()
        {
            modsTab.Click();
        }
        private void LogsTab_Clicked(IClickable clickable)
        {
            openFullLog.gameObject.SetActive(true);
            modList.SetActive(false);
            logDisp.SetActive(true);
            modsTab.ToggleMarked(false);
            logsTab.ToggleMarked(true);
            try
            {
                if(File.Exists(UnityModManager.Logger.filepath))
                    logText.text = File.ReadAllText(UnityModManager.Logger.filepath);
                else
                    logText.text = $"[Log is empty]";
            }
            catch (Exception e)
            {
                logText.text = $"[Failed to open UMM log file] {e.Message}";
            }
        }

        private void ModsTab_Clicked(IClickable clickable)
        {
            openFullLog.gameObject.SetActive(false);
            modList.SetActive(true);
            logDisp.SetActive(false);
            modsTab.ToggleMarked(true);
            logsTab.ToggleMarked(false);
            PopulateList();
        }

        void PopulateList()
        {
            RemoveChildren(modListContent.transform);
            Setup();
            for (int i = 0; i < UnityModManager.modEntries.Count; i++)
            {
                GameObject mod = Instantiate(modBtnPrefab);
                mod.transform.SetParent(modListContent.transform, false);
                mod.GetComponent<ModElement>().Setup(this, UnityModManager.modEntries[i]);
            }
        }
        void RemoveChildren(Transform parent)
        {
            if (parent.childCount > 0)
            {
                for (int i = 0; i < parent.childCount; i++)
                    Destroy(parent.GetChild(i).gameObject);
            }
        }

        public void ShowModInfo(ModElement me, UnityModManager.ModEntry m)
        {
            if (selectedMod != null) 
                selectedMod.MarkBtn(false);
            me.MarkBtn(true);
            selectedMod = me;
            SwitchDetailsSettings(false);
            if (setNeedsSave)
            {
                DVModSettings.SaveSettings(saveSetEntry);
                setNeedsSave = false;
            }
            SetDetailsText(m);
            if (m.Info.Id == "BetterModUI" || m.Info.Id == "DVModAPI") //Prevent disabling self
                ToggleButtons(m, false);
            else
                ToggleButtons(m);
        }
        void OnDisable()
        {
            if (setNeedsSave)
            {
                DVModSettings.SaveSettings(saveSetEntry);
                setNeedsSave = false;
            }
        }
        public void SwitchDetailsSettings(bool settings)
        {
            if (settings)
            {
                modSettingsPanel.SetActive(true);
                modDetailsPanel.SetActive(false);
                return;
            }
            modSettingsPanel.SetActive(false);
            modDetailsPanel.SetActive(true);
        }
#endif
    }
}
