using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadSceneButton : MonoBehaviour
{
    public void LoadScene(string sceneName)
    {
        LoadSceneManager.LoadScene(sceneName);
    }

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

    public void ExitGame()
    {
        Application.Quit();
    }
}
