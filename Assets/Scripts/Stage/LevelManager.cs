using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    private static LevelManager instance;
    public static LevelManager Instance
    {
        get
        {
            if (instance == null)
                return null;

            return instance;
        }
    }

    [Header("Levels")]
    [SerializeField] private int themeCount = 6;
    [SerializeField] private int stageCount = 10;

    public List<Theme> themes { get; private set; }

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;

            DontDestroyOnLoad(gameObject);
        }
    }

    private void Start()
    {
        InitializeTheme();
        LoadProgress();
    }

    private void InitializeTheme()
    {
        themes = new List<Theme>();

        for (int i = 0; i < themeCount; i++)
        {
            Theme theme = new Theme();
            theme.themeName = $"Theme {i + 1}";
            theme.stages = new List<Stage>();

            for (int j = 0; j < stageCount; j++)
            {
                Stage stage = new Stage();
                stage.stageName = $"Stage {j + 1}";
                stage.isUnlocked = (i == 0 && j == 0);
                stage.starCount = 0;

                theme.stages.Add(stage);
            }

            themes.Add(theme);
        }
    }

    public void CompleteStage(int themeIndex, int stageIndex, int starCount)
    {
        if (themeIndex < themes.Count && stageIndex < themes[themeIndex].stages.Count)
        {
            if (themes[themeIndex].stages[stageIndex].starCount < starCount)
            {
                themes[themeIndex].stages[stageIndex].starCount = starCount;
            }

            if (stageIndex + 1 < themes[themeIndex].stages.Count)
            {
                themes[themeIndex].stages[stageIndex + 1].isUnlocked = true;
            }

            else if (themeIndex + 1 < themes.Count)
            {
                themes[themeIndex + 1].stages[0].isUnlocked = true;
            }

            SaveProgress();
        }
    }

    public bool ReturnStageUnlocked(int themeIndex, int stageIndex)
    {
        return themes[themeIndex].stages[stageIndex].isUnlocked;
    }

    public int ReturnStageStarCount(int themeIndex, int stageIndex)
    {
        return themes[themeIndex].stages[stageIndex].starCount;
    }


    #region SaveAndLoad
    public void SaveProgress()
    {
        for (int i = 0; i < themeCount; i++)
        {
            for (int j = 0; j < stageCount; j++)
            {
                PlayerPrefs.SetInt($"Theme{i + 1}_Stage{j + 1}_Stars", themes[i].stages[j].starCount);
                PlayerPrefs.SetInt($"Theme{i + 1}_Stage{j + 1}_Unlocked", themes[i].stages[j].isUnlocked ? 1 : 0);
            }
        }
    }

    public void LoadProgress()
    {
        for (int i = 0; i < themeCount; i++)
        {
            for (int j = 0; j < stageCount; j++)
            {
                themes[i].stages[j].starCount = PlayerPrefs.GetInt($"Theme{i + 1}_Stage{j + 1}_Stars", 0);
                themes[i].stages[j].isUnlocked = PlayerPrefs.GetInt($"Theme{i + 1}_Stage{j + 1}_Unlocked", 0) == 1 ? true : false;

                if (i == 0 && j == 0) // 첫번째 스테이지일 경우
                    themes[i].stages[j].isUnlocked = true;
            }
        }
    }
    #endregion
}
