using System.Collections.Generic;
using UnityEngine;
using TMPro;
using TwinTravelers.Management;

namespace TwinTravelers.UI
{
    /// <summary>
    /// 승리 패널
    /// </summary>
    public sealed class VictoryPanel : Panel
    {
        #region Fields
        /// <summary>
        /// 별 리스트
        /// </summary>
        [Header("Preset")]
        [Tooltip("별 리스트")]
        [SerializeField] 
        private List<GameObject> stars;

        /// <summary>
        /// 획득한 코인 텍스트
        /// </summary>
        [Tooltip("별 리스트")]
        [SerializeField] 
        private TextMeshProUGUI earnedCoin;
        #endregion

        #region Methods
        /// <summary>
        /// 획득한 코인 텍스트를 설정합니다.
        /// </summary>
        /// <param name="earnedCoin">획득한 코인 수</param>
        public void SetEarnedCoinText(int earnedCoin)
        {
            this.earnedCoin.text = $"{earnedCoin.ToString()}";
        }

        /// <summary>
        /// 별의 표시 개수를 설정합니다.
        /// </summary>
        /// <param name="count">별 개수</param>
        public void SetDisplayStarCount(int count)
        {
            if (stars.Count < count)
            {
#if UNITY_EDITOR
                Debug.Log($"SetDisplayStarCount called with an invalid parameter: {count.ToString()}");
#endif
                return;
            }

            for (int i = 0; i < stars.Count; i++)
            {
                stars[i].SetActive(false);
            }

            for (int i = 0; i < count; i++)
            {
                stars[i].SetActive(true);
            }
        }

        /// <summary>
        /// 게임을 재시작합니다.
        /// </summary>
        public void RestartGame()
        {
            GameManager.Instance.RestartStage();
        }
        #endregion
    }
}
