using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Realtime;
using Photon.Pun;
using System.Linq;

public class RoomManager : MonoBehaviourPunCallbacks
{
    #region properties
    // 싱글턴 구현
    private static RoomManager instance;
    public static RoomManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<RoomManager>(); // 이래도 없다?

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

    public readonly int roomCodeLength = 6;
    public readonly int maxPlayerCount = 2;
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

    private void Start()
    {
        PhotonNetwork.LogLevel = PunLogLevel.Full;
        PhotonNetwork.AutomaticallySyncScene = false;
    }
    #endregion

    #region Method
    private string GenerateRoomCode()
    {
        string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

        return new string(Enumerable.Repeat(chars, roomCodeLength).Select(s => s[Random.Range(0, s.Length)]).ToArray());
    }

    public void CreateRoom()
    {
        string roomCode = GenerateRoomCode();
        RoomOptions options = new RoomOptions
        {
            MaxPlayers = maxPlayerCount
        };
        
        PhotonNetwork.CreateRoom(roomCode, options, null);
    }

    public void JoinRoom(string roomCode)
    {
        if (roomCode.Length < roomCodeLength)
        {
            return;
        }

        PhotonNetwork.JoinRoom(roomCode);
    }
    #endregion

    #region Override Method
    public override void OnJoinedRoom()
    {
#if SHOW_DEBUG_MESSAGES
        Debug.Log($"Joined Room: {PhotonNetwork.CurrentRoom.Name}");
#endif

        if (PhotonNetwork.CurrentRoom.PlayerCount > maxPlayerCount)
        {
            PhotonNetwork.LeaveRoom();
        }
    }

    public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
    {
#if SHOW_DEBUG_MESSAGES
        Debug.Log($"A player has left the room. Return to title");
#endif

        PhotonNetwork.LeaveRoom();
        LoadSceneManager.LoadScene("TitleScene"); 
    }

    public void LoadNextScene(string nextScene)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            photonView.RPC("LoadScene", RpcTarget.All, nextScene);
        }
    }

    [PunRPC]
    private void LoadScene(string sceneName)
    {
        LoadSceneManager.LoadScene(sceneName);
    }

    #endregion
}
