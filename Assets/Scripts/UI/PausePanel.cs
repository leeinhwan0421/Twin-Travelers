using UnityEngine;
using TwinTravelers.Management;

namespace TwinTravelers.UI
{
    /// <summary>
    /// 일시정지 패널
    /// </summary>
    public class PausePanel : Panel
    {
        /// <summary>
        /// 되돌아가기
        /// </summary>
        public void Resume()
        {
            GameManager.Instance.Resume();
        }

        /// <summary>
        /// 타이틀 씬을 불러옵니다.
        /// </summary>
        public void LoadTitleScene()
        {
            Time.timeScale = 1.0f;
            LoadSceneManager.LoadScene("TitleScene");
        }

        /// <summary>
        /// 게임을 재시작합니다.
        /// </summary>
        public void RestartGame()
        {
            Time.timeScale = 1.0f;
            GameManager.Instance.RestartStage();
        }
    }
}
