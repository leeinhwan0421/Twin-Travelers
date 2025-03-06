using UnityEngine;
using TMPro;
using TwinTravelers.Management;

namespace TwinTravelers.UI
{
    public class DefeatPanel : Panel
    {
        [Header("Preset")]
        [SerializeField] private TextMeshProUGUI earnedCoin;

        public void SetEarnedCoinText(int earnedCoin)
        {
            this.earnedCoin.text = $"{earnedCoin.ToString()}";
        }

        public void RestartGame()
        {
            GameManager.Instance.RestartStage();
        }
    }
}
