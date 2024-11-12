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
        // GameManager���� index ���·� �����ϱ� ������
        // Theme 1 Stage 1 ���� Theme = 0, Stage = 0;
        // + 1 �ؼ� �����ɴϴ�.

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
