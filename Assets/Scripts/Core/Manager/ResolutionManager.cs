using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ResolutionManager : MonoBehaviour
{
    #region Properties
    private static ResolutionManager instance;
    public static ResolutionManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<ResolutionManager>(); // 이래도 없다?

                if (instance == null)
                {
                    GameObject prefab = Resources.Load<GameObject>("ResolutionManager");

                    if (prefab != null)
                    {
                        GameObject obj = Instantiate(prefab);
                        instance = obj.GetComponent<ResolutionManager>();
                    }
                }
            }
            return instance;
        }
    }

    public List<Resolution> resolutions{ get; private set; } = new List<Resolution>();
    #endregion

    #region LifeCycle
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

            Initialize();
        }
        else
        {
            Destroy(gameObject);
        }
    }
    #endregion

    #region Initialize
    private void Initialize()
    {
        // Lists
        InitializeResolutionList();

        // Resolution
        if (SettingManager.ResolutionIndex >= 0 && SettingManager.ResolutionIndex < resolutions.Count)
        {
            OnResolutionChange(resolutions[SettingManager.ResolutionIndex], SettingManager.ResolutionIndex);
        }
        else
        {
            OnResolutionChange(resolutions[0], 0);
        }

        // FullScreen
        ResolutionManager.OnFullScreenChange(SettingManager.IsFullScreen);
    }

    private void InitializeResolutionList()
    {
        resolutions = Screen.resolutions
            .Where(res => Mathf.Approximately((float)res.width / res.height, 16f / 9f))
            .Reverse()
            .ToList();
    }

    private void InitializeScreenResolution()
    {
        if (SettingManager.ResolutionIndex >= 0 && SettingManager.ResolutionIndex < resolutions.Count)
        {
            OnResolutionChange(resolutions[SettingManager.ResolutionIndex], SettingManager.ResolutionIndex);
        }
        else
        {
            OnResolutionChange(resolutions[0], 0);
        }
    }
    #endregion

    #region static method
    public static void OnResolutionChange(Resolution selectedResolution, int resolutionIndex)
    {
        Screen.SetResolution(selectedResolution.width, selectedResolution.height, Screen.fullScreenMode, selectedResolution.refreshRateRatio);
        SettingManager.SetResolutionIndex(resolutionIndex);

#if SHOW_DEBUG_MESSAGES
        Debug.Log($"Resolution set to: {selectedResolution.width} x {selectedResolution.height} @ {selectedResolution.refreshRateRatio.numerator / selectedResolution.refreshRateRatio.denominator}Hz");
#endif
    }

    public static void OnFullScreenChange(bool isOn)
    {
        Screen.fullScreenMode = isOn ? FullScreenMode.FullScreenWindow : FullScreenMode.Windowed;
        SettingManager.SetIsFullScreen(isOn);

#if SHOW_DEBUG_MESSAGES
        Debug.Log($"Screen set to {(isOn ? "FullScreen" : "Windowed")}");
#endif
    }
    #endregion
}
