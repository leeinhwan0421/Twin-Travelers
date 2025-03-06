using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TwinTravelers.Core.Network;
using TwinTravelers.UI;

namespace TwinTravelers.Management
{
    public class LoadSceneManager : MonoBehaviour
    {
        public static string nextScene;

        [SerializeField] private LoadingPanel panel;

        private void Start()
        {
            StartCoroutine(LoadScene());
        }

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

        public static void LoadSceneLocal(string sceneName)
        {
            nextScene = sceneName;
            SceneManager.LoadScene("LoadScene");
        }

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
