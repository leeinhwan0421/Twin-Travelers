using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ResolutionSettings : MonoBehaviour
{
    [Header("preset")]
    [SerializeField] private TMP_Dropdown resolutionDropdown;
    [SerializeField] private Toggle fullscreenToggle;

    private void Start()
    {
        InitializeResolutionDropdown();
        InitializeFullscreenToggle();
    }

    #region Resolution
    private void InitializeResolutionDropdown()
    {
        resolutionDropdown.options.Clear();

        Resolution[] resolutions = Screen.resolutions;
        Resolution currentResolution = Screen.currentResolution;

        for (int i = resolutions.Length - 1; i >= 0; i--)
        {
            Resolution res = resolutions[i];

            float refreshRate = (float)res.refreshRateRatio.numerator / res.refreshRateRatio.denominator;
            string option = $"{res.width} x {res.height} {refreshRate:F1}Hz ";

            resolutionDropdown.options.Add(new TMP_Dropdown.OptionData(option));
        }

        resolutionDropdown.onValueChanged.RemoveAllListeners();
        resolutionDropdown.onValueChanged.AddListener(delegate { OnResolutionChange(resolutions[resolutionDropdown.value], resolutionDropdown.value); });

        if (SettingManager.ResolutionIndex >= 0 && SettingManager.ResolutionIndex < resolutions.Length)
        {
            OnResolutionChange(resolutions[SettingManager.ResolutionIndex], SettingManager.ResolutionIndex);
            resolutionDropdown.value = SettingManager.ResolutionIndex;
        }
        else
        {
            OnResolutionChange(resolutions[0], 0);
            resolutionDropdown.value = 0;
        }

        resolutionDropdown.RefreshShownValue();
    }

    public void OnResolutionChange(Resolution selectedResolution, int resolutionIndex)
    {
        Screen.SetResolution(selectedResolution.width, selectedResolution.height, Screen.fullScreenMode, selectedResolution.refreshRateRatio);
        SettingManager.SetResolutionIndex(resolutionIndex);
    }
    #endregion

    #region FullScreen
    private void InitializeFullscreenToggle()
    {
        fullscreenToggle.onValueChanged.RemoveAllListeners();
        fullscreenToggle.isOn = SettingManager.IsFullScreen;
        fullscreenToggle.onValueChanged.AddListener(delegate { OnFullScreenChange(fullscreenToggle.isOn); });

        OnFullScreenChange(SettingManager.IsFullScreen);
    }

    private void OnFullScreenChange(bool isOn)
    {
        Screen.fullScreenMode = isOn ? FullScreenMode.FullScreenWindow : FullScreenMode.Windowed;
        SettingManager.SetIsFullScreen(isOn);
    }

    #endregion
}