using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadSceneButton : MonoBehaviour
{
    public void LoadScene(string sceneName)
    {
        LoadSceneManager.LoadScene(sceneName);
    }
}
