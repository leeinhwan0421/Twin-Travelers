using UnityEngine;

using Photon.Pun;
using System.Collections;

public class ConnectedAndReadyButton : MonoBehaviour
{
    private SoundButton button;

    private void OnEnable()
    {
        button = GetComponent<SoundButton>();
        StartCoroutine(Coroutine());
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    private IEnumerator Coroutine()
    {
        while (true)
        {
            button.interactable = PhotonNetwork.IsConnectedAndReady;

            yield return new WaitForSecondsRealtime(0.25f);
        }
    }
}
