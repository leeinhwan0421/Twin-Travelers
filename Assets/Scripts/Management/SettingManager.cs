using System.IO;
using System.Collections.Generic;
using UnityEngine;
using TwinTravelers.Core.Utility;

namespace TwinTravelers.Management
{
    public static class SettingManager
    {
        // C:\Users\사용자이름\AppData\LocalLow\회사이름 
        private static readonly string filePath = Path.Combine(Application.persistentDataPath, "GameSetting.ini");

        static SettingManager()
        {
            LoadSettings();
        }

        #region Properties
        /*
            설정 데이터를 추가하게 될 경우

            Getter, Setter 함수를 추가합니다.
            Save, Load 함수에도 구문을 추가해주세요.
         */
        public static int ResolutionIndex { get; private set; } = 0;
        public static bool IsFullScreen { get; private set; } = true;
        public static int BGMVolume { get; private set; } = 100;
        public static int SFXVolume { get; private set; } = 100;
        #endregion

        #region Getter & Setter
        public static void SetResolutionIndex(int index)
        {
            ResolutionIndex = index;

            SaveSettings();
        }

        public static void SetIsFullScreen(bool isTrue)
        {
            IsFullScreen = isTrue;

            SaveSettings();
        }

        public static void SetBGMVolume(int volume)
        {
            BGMVolume = volume;

            SaveSettings();
        }

        public static void SetSFXVolume(int volume)
        {
            SFXVolume = volume;

            SaveSettings();
        }
        #endregion

        #region Save & Load
        public static void SaveSettings()
        {
            var iniData = new Dictionary<string, string>
        {
            { "resolutionIndex", ResolutionIndex.ToString() },
            { "isFullscreen", IsFullScreen.ToString() },
            { "BGMVolume", BGMVolume.ToString() },
            { "SFXVolume", SFXVolume.ToString() },
        };

            INIParser.WriteINI(filePath, iniData);
        }

        public static void LoadSettings()
        {
            var iniData = INIParser.ReadINI(filePath);

            if (iniData.TryGetValue("resolutionIndex", out var resolutionIndexStr) && int.TryParse(resolutionIndexStr, out var resolutionIndex))
            {
                ResolutionIndex = resolutionIndex;
            }

            if (iniData.TryGetValue("isFullscreen", out var isFullscreenStr) && bool.TryParse(isFullscreenStr, out var isFullscreen))
            {
                IsFullScreen = isFullscreen;
            }

            if (iniData.TryGetValue("BGMVolume", out var bgmVolumeStr) && int.TryParse(bgmVolumeStr, out var bgmVolume))
            {
                BGMVolume = bgmVolume;
            }

            if (iniData.TryGetValue("SFXVolume", out var sfxVolumeStr) && int.TryParse(sfxVolumeStr, out var sfxVolume))
            {
                SFXVolume = sfxVolume;
            }
        }
        #endregion
    }
}
