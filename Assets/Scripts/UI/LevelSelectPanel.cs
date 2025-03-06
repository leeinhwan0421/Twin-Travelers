using System;
using System.Collections.Generic;
using UnityEngine;
using TwinTravelers.Management;

namespace TwinTravelers.UI
{
    public sealed class LevelSelectPanel : Panel
    {
        [Header("Theme")]
        [SerializeField] private GameObject themeGroup;
        [SerializeField] private GameObject themePagenation;
        [Space(10.0f)]
        [SerializeField] private GameObject themePrefab;
        [SerializeField] private GameObject themePagenationPrefab;

        private List<GameObject> themeList = new List<GameObject>();
        private List<ThemePagenationObject> themePagenationList = new List<ThemePagenationObject>();

        private int currentTheme = 0;

        private void Start()
        {
            SetThemePanel();
        }

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

        public void PageArrow(int direction) // LEFT: -1, RIGHT: 1
        {
            themeList[currentTheme].SetActive(false);
            themePagenationList[currentTheme].ChangeOffSprite();

            currentTheme += direction;
            currentTheme = Math.Clamp(currentTheme, 0, themeList.Count - 1);

            themeList[currentTheme].SetActive(true);
            themePagenationList[currentTheme].ChangeOnSprite();
        }
    }
}
