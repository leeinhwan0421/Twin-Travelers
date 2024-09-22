using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSelectPanel : MonoBehaviour
{
    [Header("Theme")]
    [SerializeField] private GameObject themeGroup;
    [SerializeField] private GameObject themePrefab;

    private List<GameObject> themeList = new List<GameObject>();
    private int currentTheme = 0;

    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        SetThemePanel();
    }

    public void Enable()
    {
        SetEnable();
        animator.SetTrigger("Enable");
    }

    public void Disable()
    {
        animator.SetTrigger("Disable");
    }

    private void SetEnable() => gameObject.SetActive(true);

    private void SetDisable() => gameObject.SetActive(false);

    private void SetThemePanel()
    {
        var themes = LevelManager.Instance.themes;

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
