using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEngine.SceneManagement;

using Photon.Pun;
using Photon.Realtime;

using ExitGames.Client.Photon;

using TwinTravelers.UI;
using TwinTravelers.Management;

namespace TwinTravelers.Core.Network
{
    public enum Playmode
    {
        None,
        Single,
        Multi
    };

    /// <summary>
    /// 멀티플레이어 방을 생성 및 관리하는 클래스
    /// </summary>
    public class RoomManager : MonoBehaviourPunCallbacks
    {
        #region Singletion
        private static RoomManager instance;
        public static RoomManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = FindObjectOfType<RoomManager>();

                    if (instance == null)
                    {
                        GameObject prefab = Resources.Load<GameObject>("RoomManager");

                        if (prefab != null)
                        {
                            GameObject obj = Instantiate(prefab);
                            instance = obj.GetComponent<RoomManager>();
                        }
                    }
                }
                return instance;
            }
        }
        #endregion

        #region Properties
        /// <summary>
        /// 플레이어가 방을 나갔을 때 표시할 UI 패널
        /// </summary>
        [Tooltip("플레이어가 방을 나갔을 때 표시할 UI 패널")]
        [SerializeField] 
        private Panel playerLeftPanel;

        /// <summary>
        /// 방 코드 길이
        /// </summary>
        public readonly int roomCodeLength = 6;

        /// <summary>
        /// 최대 플레이어 수
        /// </summary>
        public readonly int maxPlayerCount = 2;

        /// <summary>
        /// 현재 플레이 모드
        /// </summary>
        public Playmode playmode = Playmode.Single;

        /// <summary>
        /// 존재하는 방 목록
        /// </summary>
        private List<RoomInfo> currentRoomList = new List<RoomInfo>();

        #endregion

        #region LifeCycle
        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public override void OnEnable()
        {
            base.OnEnable();
            PhotonNetwork.NetworkingClient.EventReceived += OnEventReceived;
        }

        public override void OnDisable()
        {
            base.OnDisable();
            PhotonNetwork.NetworkingClient.EventReceived -= OnEventReceived;
        }

        private void Start()
        {
            PhotonNetwork.LogLevel = PunLogLevel.Full;
            PhotonNetwork.AutomaticallySyncScene = true;

            PhotonNetwork.SendRate = Application.targetFrameRate;
            PhotonNetwork.SerializationRate = 45;

            PhotonNetwork.JoinLobby();
        }
        #endregion

        #region Room Management
        /// <summary>
        /// 랜덤한 방 코드를 생성합니다.
        /// </summary>
        /// <returns>생성된 방 코드</returns>
        private string GenerateRoomCode()
        {
            string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            string newCode;
            int maxAttempts = 100;
            int attempts = 0;

            do
            {
                newCode = new string(Enumerable.Repeat(chars, roomCodeLength)
                                    .Select(s => s[UnityEngine.Random.Range(0, s.Length)])
                                    .ToArray());
                attempts++;
            } while (IsRoomCodeDuplicated(newCode) && attempts < maxAttempts);

            if (attempts >= maxAttempts)
            {
                Debug.LogError("Failed to generate a unique room code after multiple attempts.");
                return null;
            }

            return newCode;
        }

        /// <summary>
        /// 방 코드가 중복되었는지 확인합니다.
        /// </summary>
        /// <param name="roomCode">확인할 방 코드</param>
        /// <returns>중복 여부</returns>
        private bool IsRoomCodeDuplicated(string roomCode)
        {
            foreach (var room in currentRoomList)
            {
                if (room.Name == roomCode)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 방을 생성합니다.
        /// </summary>
        /// <returns>성공 여부</returns>
        public bool CreateRoom()
        {
            string roomCode = GenerateRoomCode();

            if (string.IsNullOrEmpty(roomCode))
            {
                Debug.LogError("Failed to create room. Could not generate a unique room code.");
                return false;
            }

            if (!PhotonNetwork.IsConnectedAndReady)
            {
                Debug.LogError("Failed to create room. Client is not connected to Master Server.");
                return false;
            }

            RoomOptions options = new RoomOptions
            {
                MaxPlayers = maxPlayerCount,
                EmptyRoomTtl = 0,
                PlayerTtl = 0
            };

            return PhotonNetwork.CreateRoom(roomCode, options, TypedLobby.Default);
        }

        /// <summary>
        /// 방에 입장을 시도합니다.
        /// </summary>
        /// <param name="roomCode">방 코드</param>
        public void JoinRoom(string roomCode)
        {
            bool success = PhotonNetwork.JoinRoom(roomCode);

            if (!success)
            {
                JoinPanel panel = FindObjectOfType<JoinPanel>();
                if (panel != null)
                {
                    panel.WriteErrorText("네트워크 에러, 잠시 뒤 다시 시도해주세요.");
                }
            }
        }

        /// <summary>
        /// 방을 나갑니다.
        /// </summary>
        public void LeaveRoom()
        {
            if (PhotonNetwork.InRoom)
            {
                PhotonNetwork.LeaveRoom();
            }

            playmode = Playmode.Single;
        }
        #endregion

        #region Callbacks

        /// <summary>
        /// 방이 생성되었을 때 호출되는 콜백
        /// </summary>
        public override void OnCreatedRoom()
        {
            HostPanel panel = FindObjectOfType<HostPanel>();

            if (panel != null)
            {
                panel.SetText($"게임 참가 번호는 {PhotonNetwork.CurrentRoom.Name} 입니다.");
            }

            playmode = Playmode.Multi;
        }

        /// <summary>
        /// 방 생성에 실패했을 때 호출되는 콜백
        /// </summary>
        /// <param name="returnCode"></param>
        /// <param name="message"></param>
        public override void OnCreateRoomFailed(short returnCode, string message)
        {
            HostPanel panel = FindObjectOfType<HostPanel>();

            if (panel != null)
            {
                panel.SetText("오류가 발생하였습니다.\r\n잠시 뒤 다시 시도해주세요.");
            }

            playmode = Playmode.Single;
        }

        /// <summary>
        /// 방 목록이 업데이트 되었을 때 호출되는 콜백
        /// </summary>
        /// <param name="roomList"></param>
        public override void OnRoomListUpdate(List<RoomInfo> roomList)
        {
            currentRoomList = roomList;
        }

        /// <summary>
        /// 방에 입장했을 때 호출되는 콜백
        /// </summary>
        public override void OnJoinedRoom()
        {
#if UNITY_EDITOR
            Debug.Log($"Joined Room: {PhotonNetwork.CurrentRoom.Name}");
#endif
            if (PhotonNetwork.CurrentRoom.PlayerCount > maxPlayerCount)
            {
                LeaveRoom();
                return;
            }

            JoinPanel panel = FindObjectOfType<JoinPanel>();
            if (panel != null)
            {
                panel.CloseJoinPanel();
            }

            playmode = Playmode.Multi;
        }

        /// <summary>
        /// 방에 입장을 실패했을 때 호출되는 콜백
        /// </summary>
        /// <param name="returnCode"></param>
        /// <param name="message"></param>
        public override void OnJoinRoomFailed(short returnCode, string message)
        {
            Debug.LogError($"Failed Join Room, message: {message}");

            JoinPanel panel = FindObjectOfType<JoinPanel>();
            if (panel != null)
            {
                panel.WriteErrorText("방이 존재하지 않습니다.");
            }

            playmode = Playmode.Single;
        }

        /// <summary>
        /// 플레이어가 방에 들어왔을 때 콜백
        /// </summary>
        /// <param name="newPlayer"></param>
        public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
        {
            Debug.Log($"Player {newPlayer.NickName} has joined the room.");

            HostPanel panel = FindObjectOfType<HostPanel>();

            if (panel != null && panel.isActiveAndEnabled)
            {
                panel.Connected();
            }

            PhotonNetwork.CurrentRoom.IsOpen = false;
            PhotonNetwork.CurrentRoom.IsVisible = false;
        }

        /// <summary>
        /// 플레이어가 방을 나갔을 때 콜백
        /// </summary>
        /// <param name="otherPlayer"></param>
        public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
        {
#if UNITY_EDITOR
            Debug.Log("A player has left the room. Returning to title.");
#endif
            LeaveRoom();
            playerLeftPanel.Enable();
            LoadSceneManager.LoadScene("TitleScene");
        }

        /// <summary>
        /// 다음 씬을 로드하는 이벤트
        /// </summary>
        /// <param name="photonEvent"></param>
        private void OnEventReceived(EventData photonEvent)
        {
            if (photonEvent.Code == 1)
            {
                string sceneName = (string)photonEvent.CustomData;

                LoadSceneManager.nextScene = sceneName;
                SceneManager.LoadScene("LoadScene");
            }
        }
        #endregion

        /// <summary>
        /// 다음 씬을 로드합니다. (모든 플레이어에게 이벤트를 전송)
        /// </summary>
        /// <param name="nextScene"></param>
        public void LoadNextScene(string nextScene)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                PhotonNetwork.RaiseEvent(1, nextScene, new RaiseEventOptions { Receivers = ReceiverGroup.All }, SendOptions.SendReliable);
            }
        }
    }
}
