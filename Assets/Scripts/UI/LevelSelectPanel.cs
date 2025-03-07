using System;
using System.Collections.Generic;
using UnityEngine;
using TwinTravelers.Management;

namespace TwinTravelers.UI
{
    /// <summary>
    /// 레벨 선택 패널
    /// </summary>
    public sealed class LevelSelectPanel : Panel
    {
        #region Fields
        /// <summary>
        /// Theme 그룹이 배치될 위치
        /// </summary>
        [Header("Theme")]
        [Tooltip("Theme 그룹이 배치될 위치")]
        [SerializeField] 
        private GameObject themeGroup;

        /// <summary>
        /// Theme Pagenation 프리펩이 배치될 위치
        /// </summary>
        [Tooltip("Theme Pagenation 프리펩이 배치될 위치")]
        [SerializeField]
        private GameObject themePagenation;

        /// <summary>
        /// Theme 프리팹
        /// </summary>
        [Space(10.0f)]
        [Tooltip("Theme 프리팹")]
        [SerializeField]
        private GameObject themePrefab;

        /// <summary>
        /// Theme Pagenation 프리팹
        /// </summary>
        [Tooltip("Theme Pagenation 프리팹")]
        [SerializeField]
        private GameObject themePagenationPrefab;

        /// <summary>
        /// Theme 리스트
        /// </summary>
        private List<GameObject> themeList = new List<GameObject>();

        /// <summary>
        /// Theme Pagenation 리스트
        /// </summary>
        private List<ThemePagenationObject> themePagenationList = new List<ThemePagenationObject>();

        /// <summary>
        /// 현재 Theme
        /// </summary>
        private int currentTheme = 0;
        #endregion

        #region Unity Methods
        private void Start()
        {
            SetThemePanel();
        }
        #endregion

        #region Methods
        /// <summary>
        /// Theme 패널 세팅
        /// </summary>
        private void SetThemePanel()
        {
            var themes = LevelManager.themes;

            for (int i = 0; i < themes.Count; i++)
            {
                // Theme 세팅
                GameObject theme = Instantiate(themePrefab, themeGroup.transform);
                ThemeObject themeObject = theme.GetComponent<ThemeObject>();

                themeObject.SetThemeName(themes[i].themeName);
                themeObject.SetStages(themes[i].stages);

                theme.SetActive(false);
                themeList.Add(theme);

                // Pagenation 세팅
                GameObject pagenation = Instantiate(themePagenationPrefab, themePagenation.transform);
                ThemePagenationObject pagenationComponent = pagenation.GetComponent<ThemePagenationObject>();

                pagenationComponent.ChangeOffSprite();
                themePagenationList.Add(pagenationComponent);
            }

            themeList[currentTheme].SetActive(true);
            themePagenationList[currentTheme].ChangeOnSprite();
        }

        /// <summary>
        /// 페이지 이동
        /// </summary>
        /// <param name="direction">방향</param>
        public void PageArrow(int direction) // LEFT: -1, RIGHT: 1
        {
            themeList[currentTheme].SetActive(false);
            themePagenationList[currentTheme].ChangeOffSprite();

            currentTheme += direction;
            currentTheme = Math.Clamp(currentTheme, 0, themeList.Count - 1);

            themeList[currentTheme].SetActive(true);
            themePagenationList[currentTheme].ChangeOnSprite();
        }
        #endregion
    }
}
