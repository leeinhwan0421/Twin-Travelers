using UnityEngine;
using TMPro;
using TwinTravelers.Management;

namespace TwinTravelers.UI
{
    /// <summary>
    /// 패배 패널
    /// </summary>
    public class DefeatPanel : Panel
    {
        /// <summary>
        /// 획득한 코인을 표시하는 텍스트
        /// </summary>
        [Header("Preset")]
        [Tooltip("획득한 코인을 표시하는 텍스트")]
        [SerializeField] 
        private TextMeshProUGUI earnedCoin;

        /// <summary>
        /// 획득한 코인을 표시합니다.
        /// </summary>
        /// <param name="earnedCoin">획득한 코인 개수</param>
        public void SetEarnedCoinText(int earnedCoin)
        {
            this.earnedCoin.text = $"{earnedCoin.ToString()}";
        }

        /// <summary>
        /// 게임을 재시작합니다.
        /// </summary>
        public void RestartGame()
        {
            GameManager.Instance.RestartStage();
        }
    }
}
