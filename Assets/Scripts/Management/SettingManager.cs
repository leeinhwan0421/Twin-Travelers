using System.IO;
using System.Collections.Generic;
using UnityEngine;
using TwinTravelers.Core.Utility;

namespace TwinTravelers.Management
{
    /// <summary>
    /// 게임 설정을 관리하는 클래스
    /// </summary>
    public static class SettingManager
    {
        /// <summary>
        /// C:\Users\사용자이름\AppData\LocalLow\회사이름 
        /// </summary>
        private static readonly string filePath = Path.Combine(Application.persistentDataPath, "GameSetting.ini");

        /// <summary>
        /// static 생성자
        /// </summary>
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

        /// <summary>
        /// 해상도 인덱스
        /// </summary>
        public static int ResolutionIndex { get; private set; } = 0;

        /// <summary>
        /// 전체 화면 여부
        /// </summary>
        public static bool IsFullScreen { get; private set; } = true;

        /// <summary>
        /// 배경음 볼륨
        /// </summary>
        public static int BGMVolume { get; private set; } = 100;

        /// <summary>
        /// 효과음 볼륨
        /// </summary>
        public static int SFXVolume { get; private set; } = 100;
        #endregion

        #region Getter & Setter
        /// <summary>
        /// 해상도 인덱스 설정
        /// </summary>
        /// <param name="index">해상도 인덱스</param>
        public static void SetResolutionIndex(int index)
        {
            ResolutionIndex = index;

            SaveSettings();
        }

        /// <summary>
        /// 전체 화면 여부 설정
        /// </summary>
        /// <param name="isTrue">전체화면 여부</param>
        public static void SetIsFullScreen(bool isTrue)
        {
            IsFullScreen = isTrue;

            SaveSettings();
        }

        /// <summary>
        /// 배경음 볼륨 설정
        /// </summary>
        /// <param name="volume">설정할 볼륨</param>
        public static void SetBGMVolume(int volume)
        {
            BGMVolume = volume;

            SaveSettings();
        }

        /// <summary>
        /// 효과음 볼륨 설정
        /// </summary>
        /// <param name="volume">설정할 볼륨</param>
        public static void SetSFXVolume(int volume)
        {
            SFXVolume = volume;

            SaveSettings();
        }
        #endregion

        #region Methods
        /// <summary>
        /// 모든 설정 저장
        /// </summary>
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

        /// <summary>
        /// 모든 설정 불러오기
        /// </summary>
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
