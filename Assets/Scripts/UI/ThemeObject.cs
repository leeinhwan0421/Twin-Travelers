using System.Collections.Generic;
using TMPro;
using UnityEngine;
using TwinTravelers.Level;

namespace TwinTravelers.UI
{
    /// <summary>
    /// 테마 오브젝트
    /// </summary>
    public class ThemeObject : MonoBehaviour
    {
        #region Fields
        /// <summary>
        /// 테마 이름
        /// </summary>
        [Header("Theme")]
        [Tooltip("테마 이름")]
        [SerializeField]
        private TextMeshProUGUI themename;

        /// <summary>
        /// 스테이지 그룹
        /// </summary>
        [Tooltip("스테이지 그룹")]
        [Header("Stage")]
        [SerializeField] 
        private GameObject stageGroup;

        /// <summary>
        /// 스테이지 프리팹
        /// </summary>
        [Tooltip("스테이지 프리팹")]
        [SerializeField] 
        private GameObject stagePrefab;
        #endregion

        #region Methods
        /// <summary>
        /// 테마 이름을 설정합니다.
        /// </summary>
        /// <param name="name">테마 이름</param>
        public void SetThemeName(string name)
        {
            themename.text = name;
        }

        /// <summary>
        /// 스테이지를 설정합니다.
        /// </summary>
        /// <param name="stages">스테이지 리스트</param>
        public void SetStages(List<Stage> stages)
        {
            for (int i = 0; i < stages.Count; i++)
            {
                GameObject stage = Instantiate(stagePrefab, stageGroup.transform);
                StageObject stageObject = stage.GetComponent<StageObject>();

                stageObject.SetStageObject(stages[i], $"{themename.text} {stages[i].stageName}");
            }
        }
        #endregion
    }
}
