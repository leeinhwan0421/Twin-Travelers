using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class LevelSelectPanel : Panel
{
    [Header("Theme")]
    [SerializeField] private GameObject themeGroup;
    [SerializeField] private GameObject themePrefab;

    private List<GameObject> themeList = new List<GameObject>();
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
            GameObject theme = Instantiate(themePrefab, themeGroup.transform);
            ThemeObject themeObject = theme.GetComponent<ThemeObject>();

            themeObject.SetThemeName(themes[i].themeName);
            themeObject.SetStages(themes[i].stages);

            theme.SetActive(false);
            themeList.Add(theme);
        }


        themeList[currentTheme].SetActive(true);
    }

    public void PageArrow(int direction) // LEFT: -1, RIGHT: 1
    {
        themeList[currentTheme].SetActive(false);

        currentTheme += direction;
        currentTheme = Math.Clamp(currentTheme, 0, themeList.Count - 1);

        themeList[currentTheme].SetActive(true);
    }
}
