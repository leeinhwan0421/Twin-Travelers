using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TwinTravelers.UI
{
    /// <summary>
    /// 로딩 패널
    /// </summary>
    public class LoadingPanel : MonoBehaviour
    {
        #region Fields
        [Header("Element")]
        /// <summary>
        /// 로딩 진행도를 표시하는 프로그레스 바
        /// </summary>
        [Tooltip("로딩 진행도를 표시하는 프로그레스 바")]
        public Image progressBar;

        /// <summary>
        /// 로딩 진행도를 표시하는 텍스트
        /// </summary>
        [Tooltip("로딩 진행도를 표시하는 텍스트")]
        public TextMeshProUGUI loadingText;

        /// <summary>
        /// 로딩 바 하단 텍스트
        /// </summary>
        [Tooltip("로딩 바 하단 텍스트")]
        public TextMeshProUGUI TipText;

        /// <summary>
        /// 로딩 텍스트를 순환시킬 텍스트 리스트 (Loading, Loading. Loading.., ..)
        /// </summary>
        [Header("Loading Preset")]
        [Tooltip("로딩 텍스트를 순환시킬 텍스트 리스트 (Loading, Loading. Loading.., ..)")]
        [SerializeField]
        private List<string> cycleText;

        /// <summary>
        /// 순환 간격
        /// </summary>
        [Tooltip("순환 간격")]
        [SerializeField]
        private float cycleInterval = 0.5f;

        /// <summary>
        /// 순환 인덱스
        /// </summary>
        private int cycleIndex = 0;
        #endregion

        #region Unity Methods
        private void Start()
        {
            TipText.text = LoadTipsFromFile();

            StartCoroutine(CycleLoadingText());
        }
        #endregion

        #region Methods
        private IEnumerator CycleLoadingText()
        {
            while (true)
            {
                loadingText.text = cycleText[cycleIndex];
                cycleIndex = (cycleIndex + 1) % cycleText.Count;
                yield return new WaitForSecondsRealtime(cycleInterval);
            }
        }

        private string LoadTipsFromFile()
        {
            string tip = Encoding.UTF8.GetString(Resources.Load<TextAsset>("Tips").bytes);

            if (tip != null)
            {
                string[] tips = tip.Split("\n");

                return tips[Random.Range(0, tips.Length)];
            }
            else
            {
                return "No tips Available";
            }
        }
        #endregion
    }
}
