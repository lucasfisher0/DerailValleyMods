using DV.UI;
using DV.UIFramework;
using DVModApi;
using System;
using TMPro;
using UnityEngine;

namespace BetterModUI
{
    public class SettingsElement : MonoBehaviour
    {
        public TextMeshProUGUI settingsName;
        public TextMeshProUGUI valueText;
        public UIElementTooltipNonLocalizedText tooltipText;
        public ToggleDV checkBox;
        public ButtonDV button;
        public SliderDV slider;
        public Selector selector;

#if !Mini
        void Start ()
        {
            if(settingsName != null)
            {
                settingsName.font = BetterModUI.mainFont;
            }
            if(valueText != null)
            {
                valueText.font = BetterModUI.mainFont;
            }
        }
        internal void SetupCB(ModSettingsCheckBox ms)
        {
            settingsName.text = ms.Name;
            checkBox.SetIsOnWithoutNotify(ms.Value);
            tooltipText.text = ms.Tooltip;
            checkBox.onValueChanged.RemoveAllListeners();
            checkBox.onValueChanged.AddListener(delegate
            {
                ms.SetValue(checkBox.isOn);
                ms.DoAction?.Invoke();
            });
        }
        internal void SetupText(ModSettings ms)
        {
            settingsName.text = ms.Name;
            settingsName.textWrappingMode = TextWrappingModes.Normal;
        }
        internal void SetupBtn(ModSettings ms)
        {
            settingsName.text = ms.Name;
            tooltipText.text = ms.Tooltip;
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(delegate
            {
                ms.DoAction?.Invoke();
            });
        }
        internal void SetupSlider(ModSettingsSlider ms)
        {
            settingsName.text = ms.Name;
            slider.wholeNumbers = false;
            slider.SetValueWithoutNotify(ms.Value);
            tooltipText.text = ms.Tooltip;
            valueText.text = ms.GetValue().ToString($"F{ms.DecimalPoints}");
            slider.minValue = ms.MinValue;
            slider.maxValue = ms.MaxValue;
            slider.onValueChanged.RemoveAllListeners();
            slider.onValueChanged.AddListener(delegate
            {
                ms.SetValue((float)Math.Round(slider.value, ms.DecimalPoints));
                valueText.text = ms.GetValue().ToString($"F{ms.DecimalPoints}");
                ms.DoAction?.Invoke();
            });           
        }
        internal void SetupSliderInt(ModSettingsSliderInt ms)
        {
            settingsName.text = ms.Name;
            slider.wholeNumbers = true;
            slider.SetValueWithoutNotify(ms.Value);
            tooltipText.text = ms.Tooltip;
            if (ms.valuesArray != null)
                valueText.text = ms.valuesArray[ms.Value];
            else
                valueText.text = ms.GetValue().ToString();
            slider.maxValue = ms.MaxValue;
            slider.onValueChanged.RemoveAllListeners();
            slider.onValueChanged.AddListener(delegate
            {
                ms.SetValue((int)slider.value); 
                if (ms.valuesArray != null)
                    valueText.text = ms.valuesArray[ms.Value];
                else
                    valueText.text = ms.GetValue().ToString();
                ms.DoAction?.Invoke();
            });
        }

#endif

    }
}
