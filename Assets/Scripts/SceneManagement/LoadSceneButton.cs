using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadSceneButton : MonoBehaviour
{
    public void LoadScene(int sceneNum)
    {
#if SHOW_DEBUG_MESSAGES
        Debug.Log($"Loaded Scene... Scene Number: {sceneNum}");
#endif
        LoadSceneManager.LoadScene(sceneNum);
    }
}
