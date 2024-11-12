using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using System.Security.Cryptography;

public class ResolutionSettings : MonoBehaviour
{
    [Header("Preset")]
    [SerializeField] private TMP_Dropdown resolutionDropdown;
    [SerializeField] private Toggle fullscreenToggle;

    private void Start()
    {
        InitializeResolutionDropdown();
        InitializeFullscreenToggle();
    }

    private void InitializeResolutionDropdown()
    {
        resolutionDropdown.options.Clear();

        List<Resolution> resolutions = ResolutionManager.Instance.resolutions;

        for (int i = 0; i < resolutions.Count; i++)
        {
            Resolution res = resolutions[i];

            float refreshRate = (float)res.refreshRateRatio.numerator / res.refreshRateRatio.denominator;
            string option = $"{res.width} x {res.height} {refreshRate:F1}Hz ";
            resolutionDropdown.options.Add(new TMP_Dropdown.OptionData(option));
        }

        resolutionDropdown.onValueChanged.RemoveAllListeners();
        resolutionDropdown.onValueChanged.AddListener(delegate { ResolutionManager.OnResolutionChange(resolutions[resolutionDropdown.value], resolutionDropdown.value); });

        if (SettingManager.ResolutionIndex >= 0 && SettingManager.ResolutionIndex < resolutions.Count)
        {
            ResolutionManager.OnResolutionChange(resolutions[SettingManager.ResolutionIndex], SettingManager.ResolutionIndex);
            resolutionDropdown.value = SettingManager.ResolutionIndex;
        }
        else
        {
            ResolutionManager.OnResolutionChange(resolutions[0], 0);
            resolutionDropdown.value = 0;
        }

        resolutionDropdown.RefreshShownValue();
    }

    private void InitializeFullscreenToggle()
    {
        fullscreenToggle.onValueChanged.RemoveAllListeners();
        fullscreenToggle.isOn = SettingManager.IsFullScreen;
        fullscreenToggle.onValueChanged.AddListener(delegate { ResolutionManager.OnFullScreenChange(fullscreenToggle.isOn); });

        ResolutionManager.OnFullScreenChange(SettingManager.IsFullScreen);
    }
}