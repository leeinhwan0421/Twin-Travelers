using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

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
    #endregion

    #region Save & Load
    public static void SaveSettings()
    {
        var iniData = new Dictionary<string, string>
        {
            { "resolutionIndex", ResolutionIndex.ToString() },
            { "isFullscreen", IsFullScreen.ToString() },
        };

        INIParser.WriteINI(filePath, iniData);
    }

    public static void LoadSettings()
    {
        var iniData = INIParser.ReadINI(filePath);

        if (iniData.TryGetValue("resolutionIndex", out string resolutionIndexStr) && int.TryParse(resolutionIndexStr, out int resolutionIndex))
        {
            ResolutionIndex = resolutionIndex;
        }

        if (iniData.TryGetValue("isFullscreen", out string isFullscreenStr) && bool.TryParse(isFullscreenStr, out bool isFullscreen))
        {
            IsFullScreen = isFullscreen;
        }
    }
    #endregion
}
