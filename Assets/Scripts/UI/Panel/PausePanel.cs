using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PausePanel : Panel
{
    public void Resume()
    {
        GameManager.Instance.Resume();
    }

    public void LoadTitleScene()
    {
        Time.timeScale = 1.0f;
        LoadSceneManager.LoadScene("TitleScene");
    }

    public void RestartGame()
    {
        Time.timeScale = 1.0f;
        GameManager.Instance.RestartStage();
    }
}
