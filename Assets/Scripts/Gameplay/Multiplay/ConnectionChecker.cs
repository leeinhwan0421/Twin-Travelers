using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;
using Photon.Realtime;
using System.Net.NetworkInformation;

public class ConnectionChecker : MonoBehaviourPunCallbacks
{
    [SerializeField] private GameObject offlinePanelLayer;
    [SerializeField] private Panel offlinePanel;

    private void Start()
    {
        if (!IsInternetAvailable())
        {
            ShowOfflineWarning();
            return;
        }

        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
#if SHOW_DEBUG_MESSAGES
        Debug.Log("Successfully connected to Photon Master Server");
#endif
        HideOfflineWarning();
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
#if SHOW_DEBUG_MESSAGES
        Debug.LogWarning($"Disconnected from Photon. Cause: {cause}");
#endif
        ShowOfflineWarning();
    }

    private bool IsInternetAvailable()
    {
        try
        {
            foreach (var networkInterface in NetworkInterface.GetAllNetworkInterfaces())
            {
                if (networkInterface.OperationalStatus == OperationalStatus.Up &&
                    (networkInterface.NetworkInterfaceType == NetworkInterfaceType.Wireless80211 ||
                     networkInterface.NetworkInterfaceType == NetworkInterfaceType.Ethernet))
                {
                    return true;
                }
            }
        }
        catch
        {
#if SHOW_DEBUG_MESSAGES
            Debug.LogWarning("Failed to check network interfaces.");
#endif
        }
        return false;
    }

    private void ShowOfflineWarning()
    {
        offlinePanelLayer.SetActive(true);
        offlinePanel.Enable();
    }

    private void HideOfflineWarning()
    {
        offlinePanelLayer.SetActive(false);
        offlinePanel.Disable();
    }
}
