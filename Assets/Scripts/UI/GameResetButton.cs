using TwinTravelers.Management;
using UnityEngine;

namespace TwinTravelers.UI
{
    /// <summary>
    /// 게임 초기화 버튼
    /// </summary>
    public class GameResetButton : MonoBehaviour
    {
        /// <summary>
        /// 게임 진행 상태를 초기화합니다.
        /// </summary>
        public void ResetGameProgress()
        {
#if UNITY_EDITOR
            Debug.Log("Game progress reset triggered. Returning to Title Scene...");
#endif

            LevelManager.ResetProgress();
            LevelManager.LoadProgress();

            LoadSceneManager.LoadScene("TitleScene");
        }
    }
}
