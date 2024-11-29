using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviourPunCallbacks
{
    [Header("Player Spawn Points")]
    [SerializeField] private Transform spawnPoint1P;                // 1P SpawnPoint
    [SerializeField] private Transform spawnPoint2P;                // 2P SpawnPoint

    [Header("Local Player Prefabs")]
    [SerializeField] private string pathOflocalPlayer1P = "Player/Local/Player_Local_1P";               // Resources path
    [SerializeField] private string pathOflocalPlayer2P = "Player/Local/Player_Local_2P";               // Resources path

    [Header("Network Player Prefabs")]
    [SerializeField] private string pathOfNetworkPlayer1P = "Player/Network/Player_Network_1P";         // Resources path
    [SerializeField] private string pathOfNetworkPlayer2P = "Player/Network/Player_Network_2P";         // Resources path

    // Instance
    private List<GameObject> players = new List<GameObject>();
    public List<GameObject> Players
    {
        get
        {
            return players;
        }
    }

    // ======================================================== //

    [Header("Coins")]
    [SerializeField] private Transform coinsSpawnPointsParent;
    [SerializeField] private GameObject coin;

    private List<GameObject> coins = new List<GameObject>();

    public int coinCount
    {
        get
        {
            return coins.Count;
        }
    } 

    // ======================================================== //

    [Header("Boxs")]
    [SerializeField] private Transform boxSpawnPointsParent;
    [SerializeField] private GameObject box;

    private List<GameObject> boxs = new List<GameObject>();

    // ======================================================== //

    [Header("Barrels")]
    [SerializeField] private Transform barrelSpawnPointsParent;
    [SerializeField] private GameObject barrel;

    private List<GameObject> barrels = new List<GameObject>();

    // ======================================================== //

    [Header("Eggs")]
    [SerializeField] private Transform eggSpawnPointsParent;
    [SerializeField] private GameObject egg;

    private List<GameObject> eggs = new List<GameObject>();

    // ======================================================== //

    [Header("Gimmicks")]
    [SerializeField] private List<Lever_Door> lever_doors;
    [SerializeField] private List<Button_Door> button_doors;
    [SerializeField] private List<Two_Button_Door> two_button_doors;
    [SerializeField] private List<Key_Door> key_doors;
    [SerializeField] private List<TrapPlatform> trap_platforms;

    [Header("Boss")]
    [SerializeField] private GameObject bossPrefab;
    [SerializeField] private Transform bossPosition;
    private GameObject bossInstance;

    #region Player
    public void SpawnPlayers()
    {
        if (players.Count > 0)
        {
#if SHOW_DEBUG_MESSAGES
            Debug.Log("called spawn timing is invalid");
#endif
            RemovePlayers();
        }

        switch (RoomManager.Instance.playmode)
        {
            case RoomManager.Playmode.None:
#if SHOW_DEBUG_MESSAGES
                Debug.Log("RoomManager.playmode is none, how to come gamescene?");
#endif
                LoadSceneManager.LoadScene("TitleScene");
                break;

            case RoomManager.Playmode.Single:
                GameObject playerLocal1P = Resources.Load(pathOflocalPlayer1P) as GameObject;
                GameObject playerLocal2P = Resources.Load(pathOflocalPlayer2P) as GameObject;

                players.Add(Instantiate(playerLocal1P, spawnPoint1P.position, Quaternion.identity));
                players.Add(Instantiate(playerLocal2P, spawnPoint2P.position, Quaternion.identity));
                break;

            case RoomManager.Playmode.Multi:
                if (PhotonNetwork.IsMasterClient)
                {
                    GameObject playerNetwork1P = PhotonNetwork.Instantiate(pathOfNetworkPlayer1P, spawnPoint1P.position, Quaternion.identity);
                    players.Add(playerNetwork1P);

                    UpdatePlayerProperties(playerNetwork1P, 1);
                }
                else
                {
                    GameObject playerNetwork2P = PhotonNetwork.Instantiate(pathOfNetworkPlayer2P, spawnPoint2P.position, Quaternion.identity);
                    players.Add(playerNetwork2P);

                    UpdatePlayerProperties(playerNetwork2P, 2);
                }
                break;
        }

    }

    public void RemovePlayers()
    {
        for (int i = 0; i < players.Count; i++)
        {
            if (players[i] != null)
            {
                var photonView = players[i].GetComponent<PhotonView>();

                if (photonView != null)
                {
                    switch (RoomManager.Instance.playmode)
                    {
                        case RoomManager.Playmode.Multi:
                            if (photonView.IsMine)
                            {
                                PhotonNetwork.Destroy(players[i]);
                            }
                            else if (PhotonNetwork.IsMasterClient)
                            {
                                photonView.TransferOwnership(PhotonNetwork.LocalPlayer);
                                PhotonNetwork.Destroy(players[i]);
                            }
                            break;

                        case RoomManager.Playmode.Single:
                            Destroy(players[i]);
                            break;
                    }
                }
                else
                {
                    Destroy(players[i]);
                }
            }
        }

        players.Clear();
    }

    public void RemovePlayersWithDeadEffect()
    {
        for (int i = 0; i < players.Count; i++)
        {
            if (players[i] != null)
            {
                players[i].GetComponent<Player>().InstantiateDeadEffect();

                switch (RoomManager.Instance.playmode)
                {
                    case RoomManager.Playmode.Multi:
                        PhotonNetwork.Destroy(players[i]);
                        break;

                    case RoomManager.Playmode.Single:
                        Destroy(players[i]);
                        break;
                }
            }
        }

        players.Clear();
    }

    private void UpdatePlayerProperties(GameObject player, int playerIndex)
    {
        ExitGames.Client.Photon.Hashtable playerProperties = PhotonNetwork.CurrentRoom.CustomProperties;

        playerProperties[$"Player{playerIndex}"] = player.GetComponent<PhotonView>().ViewID;
        PhotonNetwork.CurrentRoom.SetCustomProperties(playerProperties);
    }
    #endregion

    #region Photon Callbacks
    public override void OnRoomPropertiesUpdate(ExitGames.Client.Photon.Hashtable propertiesThatChanged)
    {
        foreach (var key in propertiesThatChanged.Keys)
        {
            if (key.ToString().StartsWith("Player"))
            {
                int viewID = (int)propertiesThatChanged[key];
                GameObject player = PhotonView.Find(viewID)?.gameObject;

                if (player != null && !players.Contains(player))
                {
                    players.Add(player);
                }
            }
        }
    }
    #endregion

    #region Coin
    public void SpawnCoins()
    {
        if (coins.Count > 0)
        {
            RemoveCoins();
        }

        for (int i = 0; i < coinsSpawnPointsParent.childCount; i++)
        {
            Transform spawnPoint = coinsSpawnPointsParent.GetChild(i);
            coins.Add(Instantiate(coin, spawnPoint.position, Quaternion.identity));
        }
    }

    public void RemoveCoins()
    {
        for (int i = 0; i < coins.Count; i++)
        {
            if (coins[i] != null)
            {
                Destroy(coins[i]);
            }
        }

        coins.Clear();
    }
    #endregion

    #region Box
    public void SpawnBoxs()
    {
        if (boxs.Count > 0)
        {
            RemoveBoxs();
        }

        for (int i = 0; i < boxSpawnPointsParent.childCount; i++)
        {
            Transform spawnPoint = boxSpawnPointsParent.GetChild(i);
            boxs.Add(Instantiate(box, spawnPoint.position, Quaternion.identity));
        }
    }

    public void RemoveBoxs()
    {
        for (int i = 0; i < boxs.Count; i++)
        {
            if (boxs[i] != null)
            {
                Destroy(boxs[i]);
            }
        }

        boxs.Clear();
    }
    #endregion

    #region Barrel
    public void SpawnBarrels()
    {
        if (barrels.Count > 0)
        {
            RemoveBarrels();
        }

        for (int i = 0; i < barrelSpawnPointsParent.childCount; i++)
        {
            Transform spawnPoint = barrelSpawnPointsParent.GetChild(i);
            barrels.Add(Instantiate(barrel, spawnPoint.position, Quaternion.identity));
        }
    }

    public void RemoveBarrels()
    {
        for (int i = 0; i < barrels.Count; i++)
        {
            if (barrels[i] != null)
            {
                Destroy(barrels[i]);
            }
        }

        barrels.Clear();
    }
    #endregion

    #region Egg
    public void SpawnEggs()
    {
        if (eggs.Count > 0)
        {
            RemoveEggs();
        }

        for (int i = 0; i < eggSpawnPointsParent.childCount; i++)
        {
            Transform spawnPoint = eggSpawnPointsParent.GetChild(i);
            eggs.Add(Instantiate(egg, spawnPoint.position, Quaternion.identity));
        }
    }

    public void RemoveEggs()
    {
        for (int i = 0; i < eggs.Count; i++)
        {
            if (eggs[i] != null)
            {
                Destroy(eggs[i]);
            }
        }

        eggs.Clear();
    }
    #endregion

    #region Gimmick
    public void ResetLeverDoors()
    {
        if (lever_doors.Count <= 0)
        {
            return;
        }

        for (int i = 0; i < lever_doors.Count; i++)
        {
            if (lever_doors[i] != null)
            {
                lever_doors[i].ResetLeverDoor();
            }
        }
    }

    public void ResetButtonDoors()
    {
        if (button_doors.Count <= 0)
        {
            return;
        }

        for (int i = 0; i < button_doors.Count; i++)
        {
            if (button_doors[i] != null)
            {
                button_doors[i].ResetButtonDoor();
            }
        }
    }

    public void ResetTwoButtonDoors()
    {
        if (two_button_doors.Count <= 0)
        {
            return;
        }

        for (int i = 0; i < two_button_doors.Count; i++)
        {
            if (two_button_doors[i] != null)
            {
                two_button_doors[i].ResetTwoButtonDoor();
            }
        }
    }

    public void ResetKeyDoors()
    {
        if (key_doors.Count <= 0)
        {
            return;
        }

        for (int i = 0; i < key_doors.Count; i++)
        {
            if (key_doors[i] != null)
            {
                key_doors[i].ResetKeyDoor();
            }
        }
    }

    public void ResetTrapPlatforms()
    {
        if (trap_platforms.Count <= 0)
        {
            return;
        }

        for (int i = 0; i < trap_platforms.Count; i++)
        {
            if (trap_platforms[i] != null)
            {
                trap_platforms[i].ResetTrapPlatform();
            }
        }
    }
    #endregion

    #region Boss
    private void ResetBoss()
    {
        if (bossPrefab == null || bossPosition == null)
        {
            return;
        }

        if (bossInstance != null)
        {
            Destroy(bossInstance);
        }

        bossInstance = Instantiate(bossPrefab, bossPosition.position, Quaternion.identity);
    }
    #endregion

    public void ResetAll()
    {
        RemovePlayers();
        SpawnPlayers();

        RemoveCoins();
        SpawnCoins();

        RemoveBoxs();
        SpawnBoxs();

        RemoveBarrels();
        SpawnBarrels();

        RemoveEggs();
        SpawnEggs();

        ResetLeverDoors();
        ResetButtonDoors();
        ResetTwoButtonDoors();
        ResetKeyDoors();

        ResetTrapPlatforms();

        ResetBoss();
    }
}
