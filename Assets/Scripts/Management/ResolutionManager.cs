using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace TwinTravelers.Management
{
    /// <summary>
    /// 화면 해상도를 관리하는 클래스
    /// </summary>
    public class ResolutionManager : MonoBehaviour
    {
        #region Field
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

        /// <summary>
        /// 해상도 리스트
        /// </summary>
        public List<Resolution> resolutions { get; private set; } = new List<Resolution>();
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

        #region Methods
        /// <summary>
        /// 초기화 메서드
        /// </summary>
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

        /// <summary>
        /// 해상도 리스트 초기화
        /// </summary>
        private void InitializeResolutionList()
        {
            resolutions = Screen.resolutions
                .Where(res => Mathf.Approximately((float)res.width / res.height, 16f / 9f))
                .Reverse()
                .ToList();
        }

        /// <summary>
        /// 화면 해상도 초기화
        /// </summary>
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

        /// <summary>
        /// 해상도 변경 시 호출되는 메서드
        /// </summary>
        /// <param name="selectedResolution">선택된 해상도</param>
        /// <param name="resolutionIndex">해상도 인덱스</param>
        public static void OnResolutionChange(Resolution selectedResolution, int resolutionIndex)
        {
            Screen.SetResolution(selectedResolution.width, selectedResolution.height, Screen.fullScreenMode, selectedResolution.refreshRateRatio);
            SettingManager.SetResolutionIndex(resolutionIndex);

#if UNITY_EDITOR
            Debug.Log($"Resolution set to: {selectedResolution.width} x {selectedResolution.height} @ {selectedResolution.refreshRateRatio.numerator / selectedResolution.refreshRateRatio.denominator}Hz");
#endif
        }

        /// <summary>
        /// 전체 화면 모드 변경 시 호출되는 메서드
        /// </summary>
        /// <param name="isFullScrren">전체화면 활성화 여부</param>
        public static void OnFullScreenChange(bool isFullScrren)
        {
            Screen.fullScreenMode = isFullScrren ? FullScreenMode.FullScreenWindow : FullScreenMode.Windowed;
            SettingManager.SetIsFullScreen(isFullScrren);

#if UNITY_EDITOR
            Debug.Log($"Screen set to {(isFullScrren ? "FullScreen" : "Windowed")}");
#endif
        }
        #endregion
    }
}
