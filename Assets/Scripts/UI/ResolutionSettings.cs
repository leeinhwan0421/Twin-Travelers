using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using TwinTravelers.Management;

namespace TwinTravelers.UI
{
    /// <summary>
    /// 해상도 설정
    /// </summary>
    public class ResolutionSettings : MonoBehaviour
    {
        #region Fields
        /// <summary>
        /// 해상도 설정 드롭다운
        /// </summary>
        [Header("Preset")]
        [Tooltip("해상도 설정 드롭다운")]
        [SerializeField] 
        private TMP_Dropdown resolutionDropdown;

        /// <summary>
        /// 전체화면 설정 토글
        /// </summary>
        [Tooltip("전체화면 설정 토글")]
        [SerializeField]
        private Toggle fullscreenToggle;
        #endregion

        #region Unity Methods
        private void Start()
        {
            InitializeResolutionDropdown();
            InitializeFullscreenToggle();
        }
        #endregion

        #region Methods
        /// <summary>
        /// 해상도 설정 드롭다운을 초기화합니다.
        /// </summary>
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

        /// <summary>
        /// 전체화면 설정을 초기화합니다.
        /// </summary>
        private void InitializeFullscreenToggle()
        {
            fullscreenToggle.onValueChanged.RemoveAllListeners();
            fullscreenToggle.isOn = SettingManager.IsFullScreen;
            fullscreenToggle.onValueChanged.AddListener(delegate { ResolutionManager.OnFullScreenChange(fullscreenToggle.isOn); });

            ResolutionManager.OnFullScreenChange(SettingManager.IsFullScreen);
        }
        #endregion
    }
}