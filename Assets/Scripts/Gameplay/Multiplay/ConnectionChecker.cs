using System.Collections;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System.Net.NetworkInformation;

public class ConnectionChecker : MonoBehaviourPunCallbacks
{
    [SerializeField] private GameObject offlinePanelLayer;
    [SerializeField] private OfflinePanel offlinePanel;

    private Coroutine connectionCheckCoroutine;

    private bool wasOffline = false;

    private void Start()
    {
        if (!IsInternetAvailable())
        {
            ShowOfflineWarning();
            wasOffline = true;
        }
        else
        {
            PhotonNetwork.ConnectUsingSettings();
            wasOffline = false;
        }

        connectionCheckCoroutine = StartCoroutine(PeriodicConnectionCheck());
    }

    public override void OnConnectedToMaster()
    {
#if UNITY_EDITOR
        Debug.Log("Successfully connected to Photon Master Server");
#endif
        HideOfflineWarning();

        PhotonNetwork.JoinLobby();
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
#if UNITY_EDITOR
        Debug.LogWarning($"Disconnected from Photon. Cause: {cause}");
#endif
        RoomManager.Instance.playmode = RoomManager.Playmode.Single;
        ShowOfflineWarning();
    }

    private bool IsInternetAvailable()
    {
        return Application.internetReachability != NetworkReachability.NotReachable;
    }

    private void ShowOfflineWarning()
    {
        if (offlinePanel == null || offlinePanelLayer == null)
        {
            return;
        }

        offlinePanelLayer.SetActive(true);
        offlinePanel.Enable();
    }

    private void HideOfflineWarning()
    {
        if (offlinePanel == null && offlinePanelLayer == null)
        {
            return;
        }

        if (offlinePanel.gameObject.activeSelf == false && offlinePanelLayer.activeSelf == false)
        {
            return;
        }

        offlinePanelLayer.SetActive(false);
        offlinePanel.Disable();
    }

    private IEnumerator PeriodicConnectionCheck()
    {
        while (true)
        {
            bool isCurrentlyOffline = !IsInternetAvailable();

            if (isCurrentlyOffline && !wasOffline)
            {
#if UNITY_EDITOR
                Debug.Log("Internet disconnected. Showing offline warning...");
#endif
                ShowOfflineWarning();
                wasOffline = true;

                RoomManager.Instance.LeaveRoom();
                PhotonNetwork.Disconnect();
            }
            else if (!isCurrentlyOffline && wasOffline)
            {
#if UNITY_EDITOR
                Debug.Log("Internet reconnected. Hiding offline warning and reconnecting...");
#endif
                HideOfflineWarning();
                wasOffline = false;

                PhotonNetwork.ConnectUsingSettings();
            }

            yield return new WaitForSeconds(3);
        }
    }

    private void OnDestroy()
    {
        if (connectionCheckCoroutine != null)
        {
            StopCoroutine(connectionCheckCoroutine);
        }
    }
}
