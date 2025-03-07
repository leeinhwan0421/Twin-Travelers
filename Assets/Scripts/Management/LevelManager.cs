using System.Collections.Generic;
using UnityEngine;
using TwinTravelers.Level;

namespace TwinTravelers.Management
{
    /// <summary>
    /// 레벨 관리 클래스
    /// </summary>
    public static class LevelManager
    {
        /// <summary>
        /// 테마 개수
        /// </summary>
        public const int themeCount = 5;

        /// <summary>
        /// 스테이지 개수
        /// </summary>
        public const int stageCount = 10;

        /// <summary>
        /// 테마 리스트
        /// </summary>
        public static List<Theme> themes { get; private set; }

        /// <summary>
        /// static 생성자
        /// </summary>
        static LevelManager()
        {
            InitializeTheme();
            LoadProgress();
        }

        #region Methods
        /// <summary>
        /// 테마 초기화 (사실상 스테이지도 초기화)
        /// </summary>
        private static void InitializeTheme()
        {
            themes = new List<Theme>();
            themes.Capacity = themeCount;

            for (int i = 0; i < themeCount; i++)
            {
                Theme theme = new Theme();
                theme.themeName = $"Theme {i + 1}";
                theme.stages = new List<Stage>();
                theme.stages.Capacity = stageCount;

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

        /// <summary>
        /// 스테이지 완료 시, 스테이지 정보 업데이트
        /// </summary>
        /// <param name="themeIndex">클리어한 테마 인덱스</param>
        /// <param name="stageIndex">클리어한 스테이지 인덱스</param>
        /// <param name="starCount">획득한 별 개수</param>
        public static void CompleteStage(int themeIndex, int stageIndex, int starCount)
        {
            if (themeIndex < themes.Count && stageIndex < themes[themeIndex].stages.Count)
            {
                themes[themeIndex].stages[stageIndex].isUnlocked = true;

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

        /// <summary>
        /// 스테이지 잠금 해제 여부 반환
        /// </summary>
        /// <param name="themeIndex">조회하려는 테마 인덱스</param>
        /// <param name="stageIndex">조회하려는 스테이지 인덱스</param>
        /// <returns>잠금 여부</returns>
        public static bool ReturnStageUnlocked(int themeIndex, int stageIndex)
        {
            return themes[themeIndex].stages[stageIndex].isUnlocked;
        }

        /// <summary>
        /// 스테이지 별 개수 반환
        /// </summary>
        /// <param name="themeIndex">조회하려는 테마 인덱스</param>
        /// <param name="stageIndex">조회하려는 스테이지 인덱스</param>
        /// <returns>획득한 별 갯수</returns>
        public static int ReturnStageStarCount(int themeIndex, int stageIndex)
        {
            return themes[themeIndex].stages[stageIndex].starCount;
        }

        /// <summary>
        /// 모든 스테이지 정보 저장
        /// </summary>
        public static void SaveProgress()
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

        /// <summary>
        /// 모든 스테이지 정보 불러오기
        /// </summary>
        public static void LoadProgress()
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

        /// <summary>
        /// 모든 스테이지 정보 초기화
        /// </summary>
        public static void ResetProgress()
        {
            PlayerPrefs.DeleteAll();
            PlayerPrefs.Save();
        }

        /// <summary>
        /// 모든 스테이지 잠금 해제
        /// </summary>
        public static void UnlockAllStages()
        {
            foreach (var theme in themes)
            {
                foreach (var stage in theme.stages)
                {
                    stage.isUnlocked = true;
                    stage.starCount = 3;
                }
            }

            SaveProgress();
        }
        #endregion
    }
}
