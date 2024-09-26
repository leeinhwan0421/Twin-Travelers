using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class URLButton : MonoBehaviour
{
    [Header("preset")]
    [SerializeField] private string link;

    public void OnClick()
    {
#if SHOW_DEBUG_MESSAGES
        Debug.Log($"OpenURL : {link}");
#endif
        Application.OpenURL(link);
    }
}
