using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;
using UnityEngine.SceneManagement;

public class RoomManager : MonoBehaviourPunCallbacks
{
    public enum Playmode
    {
        None,
        Single,
        Multi
    };

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

    [SerializeField] private Panel playerLeftPanel;

    public readonly int roomCodeLength = 6;
    public readonly int maxPlayerCount = 2;

    public Playmode playmode = Playmode.Single;

    private List<RoomInfo> currentRoomList = new List<RoomInfo>();


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
    private string GenerateRoomCode()
    {
        string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        string newCode;
        int maxAttempts = 100;
        int attempts = 0;

        do {
            newCode = new string(Enumerable.Repeat(chars, roomCodeLength)
                                .Select(s => s[Random.Range(0, s.Length)])
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

    public string CreateRoom()
    {
        string roomCode = GenerateRoomCode();

        if (string.IsNullOrEmpty(roomCode))
        {
            Debug.LogError("Failed to create room. Could not generate a unique room code.");
            return null;
        }

        RoomOptions options = new RoomOptions
        {
            MaxPlayers = maxPlayerCount,
            EmptyRoomTtl = 0,
            PlayerTtl = 0
        };

        PhotonNetwork.CreateRoom(roomCode, options, TypedLobby.Default);
        playmode = Playmode.Multi;

        return roomCode;
    }

    public void JoinRoom(string roomCode)
    {
        bool success = PhotonNetwork.JoinRoom(roomCode);

        if (!success)
        {
            JoinPanel panel = FindObjectOfType<JoinPanel>();
            if (panel != null)
            {
                panel.WriteErrorText("��Ʈ��ũ ����, ��� �� �ٽ� �õ����ּ���.");
            }
        }
    }

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
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        currentRoomList = roomList;
    }

    public override void OnJoinedRoom()
    {
#if SHOW_DEBUG_MESSAGES
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

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.LogError($"Failed Join Room, message: {message}");

        JoinPanel panel = FindObjectOfType<JoinPanel>();
        if (panel != null)
        {
            panel.WriteErrorText("���� �������� �ʽ��ϴ�.");
        }

        playmode = Playmode.Single;
    }

    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        Debug.Log($"Player {newPlayer.NickName} has joined the room.");

        HostPanel panel = FindObjectOfType<HostPanel>();

        if (panel != null && panel.isActiveAndEnabled)
        {
            panel.Connected();
        }
    }

    public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
    {
#if SHOW_DEBUG_MESSAGES
        Debug.Log("A player has left the room. Returning to title.");
#endif
        LeaveRoom();
        playerLeftPanel.Enable();
        LoadSceneManager.LoadScene("TitleScene");
    }

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

    public void LoadNextScene(string nextScene)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.RaiseEvent(1, nextScene, new RaiseEventOptions { Receivers = ReceiverGroup.All }, SendOptions.SendReliable);
        }
    }

}
