using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TwinTravelers.Core.Network;
using TwinTravelers.UI;

namespace TwinTravelers.Management
{
    /// <summary>
    /// 씬을 로드를 관리하는 클래스 (비효율적인 코드)
    /// </summary>
    public class LoadSceneManager : MonoBehaviour
    {
        /// <summary>
        /// 다음 씬 이름
        /// </summary>
        public static string nextScene;

        /// <summary>
        /// 로딩 패널
        /// </summary>
        [SerializeField] 
        [Tooltip("로딩 패널")]
        private LoadingPanel panel;

        private void Start()
        {
            StartCoroutine(LoadScene());
        }

        /// <summary>
        /// 씬을 로드합니다.
        /// </summary>
        /// <param name="sceneName">이동할 씬 이름</param>
        public static void LoadScene(string sceneName)
        {
#if UNITY_EDITOR
            Debug.Log($"Loading the scene: {sceneName}");
#endif

            if (RoomManager.Instance.playmode == Playmode.Multi)
            {
                RoomManager.Instance.LoadNextScene(sceneName);
            }
            else
            {
                LoadSceneLocal(sceneName);
            }
        }

        /// <summary>
        /// 씬을 로드합니다.
        /// </summary>
        /// <param name="sceneName">이동할 씬 이름</param>
        public static void LoadSceneLocal(string sceneName)
        {
            nextScene = sceneName;
            SceneManager.LoadScene("LoadScene");
        }

        /// <summary>
        /// 씬을 로드하는 코루틴 (Panel의 ProgressBar 및 기타 시각 효과 반영)
        /// </summary>
        /// <returns>IEnumerator</returns>
        private IEnumerator LoadScene()
        {
            yield return null;

            AsyncOperation op = SceneManager.LoadSceneAsync(nextScene);

            op.allowSceneActivation = false;
            float timer = 0.0f;

            AudioManager.Instance.StopBGM();

            while (!op.isDone)
            {
                yield return null;

                timer += Time.deltaTime;

                if (op.progress < 0.9f)
                {
                    panel.progressBar.fillAmount = Mathf.Max(panel.progressBar.fillAmount, Mathf.Lerp(panel.progressBar.fillAmount, op.progress, timer));

                    if (panel.progressBar.fillAmount >= op.progress)
                    {
                        timer = 0.0f;
                    }
                }
                else
                {
                    panel.progressBar.fillAmount = Mathf.Max(panel.progressBar.fillAmount, Mathf.Lerp(panel.progressBar.fillAmount, 1f, timer));

                    if (panel.progressBar.fillAmount == 1.0f)
                    {
                        AudioManager.Instance.ChangeWithPlay(nextScene);
                        op.allowSceneActivation = true;

                        yield break;
                    }
                }
            }

            AudioManager.Instance.ChangeWithPlay(nextScene);

            yield break;
        }
    }
}
