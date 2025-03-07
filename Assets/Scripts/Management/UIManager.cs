using TMPro;
using TwinTravelers.UI;
using UnityEngine;

namespace TwinTravelers.Management
{
    public class UIManager : MonoBehaviour
    {
        #region Fields
        /// <summary>
        /// 타이머 텍스트
        /// </summary>
        [Header("Timer")]
        [Tooltip("타이머 텍스트")]
        [SerializeField] 
        private TextMeshProUGUI timeText;

        /// <summary>
        /// 획득한 코인 텍스트
        /// </summary>
        [Header("Coins")]
        [Tooltip("획득한 코인 텍스트")]
        [SerializeField]
        private TextMeshProUGUI earnedCoinText;

        /// <summary>
        /// 결과 패널의 배경 패널
        /// </summary>
        [Header("UI Element")]
        [Tooltip("결과 패널의 배경 패널")]
        [SerializeField]
        private GameObject resultBackgroundPanel;

        /// <summary>
        /// 결과 패널의 배경 패널
        /// </summary>
        public GameObject ResultBackgroundPanel => resultBackgroundPanel;

        /// <summary>
        /// 패배 패널
        /// </summary>
        [Tooltip("패배 패널")]
        [SerializeField]
        private DefeatPanel defeatPanel;

        /// <summary>
        /// 패배 패널
        /// </summary>
        public DefeatPanel DefeatPanel => defeatPanel;

        /// <summary>
        /// 승리 패널
        /// </summary>
        [Tooltip("승리 패널")]
        [SerializeField]
        private VictoryPanel victoryPanel;

        /// <summary>
        /// 승리 패널
        /// </summary>
        public VictoryPanel VictoryPanel => victoryPanel;

        /// <summary>
        /// 일시정지 패널
        /// </summary>
        [Tooltip("일시정지 패널")]
        [SerializeField]
        private PausePanel pausePanel;

        /// <summary>
        /// 일시정지 패널
        /// </summary>
        public PausePanel PausePanel => pausePanel;
        #endregion

        /// <summary>
        /// 타이머 텍스트를 설정합니다.
        /// </summary>
        /// <param name="playTime">플레이 시간</param>
        public void SetTimeText(float playTime)
        {
            int min = (int)(playTime / 60.0f);
            int sec = (int)(playTime % 60.0f);

            timeText.text = $"{min.ToString("00")} : {sec.ToString("00")}";
        }

        /// <summary>
        /// 획득한 코인 텍스트를 설정합니다.
        /// </summary>
        /// <param name="earnedCoinCount">획득한 코인 수량</param>
        public void SetEarnedCoinText(int earnedCoinCount)
        {
            earnedCoinText.text = $"{earnedCoinCount.ToString()}";
        }
    }
}
