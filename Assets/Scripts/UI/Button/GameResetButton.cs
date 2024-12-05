using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameResetButton : MonoBehaviour
{
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
