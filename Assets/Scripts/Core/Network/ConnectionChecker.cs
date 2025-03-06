using System.Collections;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TwinTravelers.UI;

namespace TwinTravelers.Core.Network
{
    /// <summary>
    /// 현재 기기의 연결 상태를 확인하고, 상태에 대한 처리를 정의한 클래스
    /// </summary>
    public class ConnectionChecker : MonoBehaviourPunCallbacks
    {
        #region Field
        /// <summary>
        /// 오프라인일 때 표시되는 패널 레이어
        /// </summary>
        [Tooltip("오프라인일 때 표시되는 패널 레이어")]
        [SerializeField] 
        private GameObject offlinePanelLayer;

        /// <summary>
        /// 오프라인일 때 표시되는 패널 컴포넌트
        /// </summary>
        [Tooltip("오프라인일 때 표시되는 패널 컴포넌트")]
        [SerializeField] 
        private OfflinePanel offlinePanel;

        /// <summary>
        /// 주기적으로 네트워크 상태를 확인하는 코루틴
        /// </summary>
        private Coroutine connectionCheckCoroutine;

        /// <summary>
        /// 이전 네트워크 상태 (true: 오프라인, false: 온라인)
        /// </summary>
        private bool wasOffline = false;
        #endregion

        #region Unity Methods
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

        private void OnDestroy()
        {
            if (connectionCheckCoroutine != null)
            {
                StopCoroutine(connectionCheckCoroutine);
            }
        }
        #endregion

        #region Photon Callbacks
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
            RoomManager.Instance.playmode = Playmode.Single;
            ShowOfflineWarning();
        }
        #endregion

        #region Methods
        /// <summary>
        /// 현재 인터넷 연결이 가능한지 확인
        /// </summary>
        /// <returns>인터넷이 연결되어 있으면 true, 아니면 false</returns>
        private bool IsInternetAvailable()
        {
            return Application.internetReachability != NetworkReachability.NotReachable;
        }

        /// <summary>
        /// 오프라인 상태일 때 UI를 표시하는 함수
        /// </summary>
        private void ShowOfflineWarning()
        {
            if (offlinePanel == null || offlinePanelLayer == null)
            {
                return;
            }

            offlinePanelLayer.SetActive(true);
            offlinePanel.Enable();
        }

        /// <summary>
        /// 온라인 상태로 복귀했을 때 UI를 숨기는 함수
        /// </summary>
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

        /// <summary>
        /// 3초마다 네트워크 상태를 확인하는 코루틴
        /// </summary>
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
        #endregion
    }
}
