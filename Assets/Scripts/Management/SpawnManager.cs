using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Photon.Pun;
using TwinTravelers.Core.Gimmicks;
using TwinTravelers.Core.Actor;
using TwinTravelers.Core.Network;

namespace TwinTravelers.Management
{
    public class SpawnManager : MonoBehaviourPunCallbacks
    {
        // ======================================================== //

        private string pathOflocalPlayer1P = "Player/Local/Player_Local_1P";               // Resources path
        private string pathOflocalPlayer2P = "Player/Local/Player_Local_2P";               // Resources path

        private string pathOfNetworkPlayer1P = "Player/Network/Player_Network_1P";         // Resources path
        private string pathOfNetworkPlayer2P = "Player/Network/Player_Network_2P";         // Resources path

        // Instance
        private List<GameObject> players = new List<GameObject>();
        public List<GameObject> Players
        {
            get
            {
                return players.Where(player => player != null).ToList();
            }
        }

        [Header("Player Spawn Points")]
        [SerializeField] private Transform spawnPoint1P;                // 1P SpawnPoint
        [SerializeField] private Transform spawnPoint2P;                // 2P SpawnPoint

        // ======================================================== //

        [Header("Coins")]
        [SerializeField] private Transform coinsSpawnPointsParent;

        private string coin = "Gimmicks/Coin";
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

        private string box = "Gimmicks/Moveable/Box";
        private List<GameObject> boxs = new List<GameObject>();

        // ======================================================== //

        [Header("Barrels")]
        [SerializeField] private Transform barrelSpawnPointsParent;

        private string barrel = "Gimmicks/Moveable/Barrel";
        private List<GameObject> barrels = new List<GameObject>();

        // ======================================================== //

        [Header("Eggs")]
        [SerializeField] private Transform eggSpawnPointsParent;

        private string egg = "Gimmicks/Moveable/Egg";
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
#if UNITY_EDITOR
                Debug.Log("called spawn timing is invalid");
#endif
                RemovePlayers();
            }

            switch (RoomManager.Instance.playmode)
            {
                case Playmode.None:
#if UNITY_EDITOR
                    Debug.Log("RoomManager.playmode is none, how to come gamescene?");
#endif
                    LoadSceneManager.LoadScene("TitleScene");
                    break;

                case Playmode.Single:
                    GameObject playerLocal1P = Resources.Load(pathOflocalPlayer1P) as GameObject;
                    GameObject playerLocal2P = Resources.Load(pathOflocalPlayer2P) as GameObject;

                    players.Capacity = 2;

                    players.Add(Instantiate(playerLocal1P, spawnPoint1P.position, Quaternion.identity));
                    players.Add(Instantiate(playerLocal2P, spawnPoint2P.position, Quaternion.identity));
                    break;

                case Playmode.Multi:
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
                            case Playmode.Multi:
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

                            case Playmode.Single:
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
                        case Playmode.Multi:
                            PhotonNetwork.Destroy(players[i]);
                            break;

                        case Playmode.Single:
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

        #region Spawn and Remove Items

        private void SpawnItems(Transform parent, string pathOfPrefab, List<GameObject> list)
        {
            RemoveItems(list);

            list.Capacity = parent.childCount;

            for (int i = 0; i < parent.childCount; i++)
            {
                var spawnPoint = parent.GetChild(i);

                switch (RoomManager.Instance.playmode)
                {
                    case Playmode.Multi:
                        if (PhotonNetwork.IsMasterClient)
                        {
                            GameObject objNetwork = PhotonNetwork.InstantiateRoomObject(pathOfPrefab, spawnPoint.position, Quaternion.identity);

                            if (objNetwork.TryGetComponent<PhotonView>(out PhotonView photonView))
                            {
                                photonView.TransferOwnership(PhotonNetwork.MasterClient);
                            }

                            list.Add(objNetwork);
                        }

                        break;
                    case Playmode.Single:

                        GameObject objLocal = Resources.Load(pathOfPrefab) as GameObject;

                        if (objLocal == null)
                        {
                            Debug.LogError("SpawnItems, but Can't read Item Path");
                            return;
                        }

                        var itemLocal = Instantiate(objLocal, spawnPoint.position, Quaternion.identity);

                        list.Add(itemLocal);

                        break;
                }
            }
        }

        private void RemoveItems(List<GameObject> list)
        {
            foreach (var item in list)
            {
                if (item != null)
                {
                    switch (RoomManager.Instance.playmode)
                    {
                        case Playmode.Multi:
                            if (PhotonNetwork.IsMasterClient)
                            {
                                PhotonNetwork.Destroy(item);
                            }
                            break;
                        case Playmode.Single:
                            Destroy(item);
                            break;
                    }
                }
            }

            list.Clear();
        }

        public void SpawnCoins() => SpawnItems(coinsSpawnPointsParent, coin, coins);
        public void RemoveCoins() => RemoveItems(coins);

        public void SpawnBoxs() => SpawnItems(boxSpawnPointsParent, box, boxs);
        public void RemoveBoxs() => RemoveItems(boxs);

        public void SpawnBarrels() => SpawnItems(barrelSpawnPointsParent, barrel, barrels);
        public void RemoveBarrels() => RemoveItems(barrels);

        public void SpawnEggs() => SpawnItems(eggSpawnPointsParent, egg, eggs);
        public void RemoveEggs() => RemoveItems(eggs);

        #endregion

        #region Gimmick Resets

        public void ResetGimmicks()
        {
            ResetList(lever_doors, door => door.ResetLeverDoor());
            ResetList(button_doors, door => door.ResetButtonDoor());
            ResetList(two_button_doors, door => door.ResetTwoButtonDoor());
            ResetList(key_doors, door => door.ResetKeyDoor());
            ResetList(trap_platforms, platform => platform.ResetTrapPlatform());
        }

        private void ResetList<T>(List<T> list, System.Action<T> resetAction)
        {
            foreach (var item in list)
            {
                resetAction?.Invoke(item);
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

            SpawnCoins();
            SpawnBoxs();
            SpawnBarrels();
            SpawnEggs();

            ResetGimmicks();
            ResetBoss();
        }
    }
}
