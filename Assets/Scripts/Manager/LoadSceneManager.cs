using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
#if SHOW_DEBUG_MESSAGES
        Debug.Log($"Loading the scene: {sceneName}");
#endif

        nextScene = sceneName;
        SceneManager.LoadScene("LoadScene");
    }

    private IEnumerator LoadScene()
    {
        yield return null;

        AsyncOperation op = SceneManager.LoadSceneAsync(nextScene);

        op.allowSceneActivation = false;
        float timer = 0.0f;

        while (!op.isDone)
        {
            yield return null;

            timer += Time.deltaTime;

            if (op.progress < 0.9f)
            {
                panel.progressBar.fillAmount = Mathf.Lerp(panel.progressBar.fillAmount, op.progress, timer);
                
                if (panel.progressBar.fillAmount >= op.progress)
                {
                    timer = 0.0f;
                }
            }
            else
            {
                panel.progressBar.fillAmount = Mathf.Lerp(panel.progressBar.fillAmount, 1f, timer);

                if (panel.progressBar.fillAmount == 1.0f)
                {
                    op.allowSceneActivation = true;
                    yield break;
                }
            }
        }

        yield break;
    }
}
