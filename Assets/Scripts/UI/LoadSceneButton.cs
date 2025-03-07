using UnityEngine;
using TwinTravelers.Management;

namespace TwinTravelers.UI
{
    /// <summary>
    /// 씬을 로드하는 버튼
    /// </summary>
    public class LoadSceneButton : MonoBehaviour
    {
        /// <summary>
        /// 씬을 로드합니다.
        /// </summary>
        /// <param name="sceneName">이동할 씬 이름</param>
        public void LoadScene(string sceneName)
        {
            LoadSceneManager.LoadScene(sceneName);
        }

        /// <summary>
        /// 다음 스테이지 씬을 불러옵니다.
        /// GameManager에서 index 형태로 저장하기 때문에
        /// Theme 1 Stage 1 에서 Theme = 0, Stage = 0;
        /// + 1 해서 가져옵니다.
        /// </summary>
        public void LoadNextStageScene()
        {
            // GameManager에서 index 형태로 저장하기 때문에
            // Theme 1 Stage 1 에서 Theme = 0, Stage = 0;
            // + 1 해서 가져옵니다.

            int theme = GameManager.Instance.Theme + 1;
            int stage = GameManager.Instance.Stage + 1;

            if (stage + 1 <= LevelManager.stageCount)
            {
                LoadSceneManager.LoadScene($"Theme {theme} Stage {stage + 1}");
            }
            else
            {
                LoadSceneManager.LoadScene("TitleScene");
            }
        }

        /// <summary>
        /// 게임을 종료합니다.
        /// </summary>
        public void ExitGame()
        {
            Application.Quit();
        }
    }
}
