using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Photon.Pun;
using TwinTravelers.Core.Gimmicks;
using TwinTravelers.Core.Actor;
using TwinTravelers.Core.Network;

namespace TwinTravelers.Management
{
    /// <summary>
    /// 플레이어와 아이템을 스폰하고 관리하는 클래스
    /// 너무 더러운 코드
    /// </summary>
    public class SpawnManager : MonoBehaviourPunCallbacks
    {
        #region Fields
        // ======================================================== //

        /// <summary>
        /// 로컬 플레이어 1P 프리펩 경로
        /// </summary>
        private readonly string pathOflocalPlayer1P = "Player/Local/Player_Local_1P";

        /// <summary>
        /// 로컬 플레이어 2P 프리펩 경로
        /// </summary>
        private readonly string pathOflocalPlayer2P = "Player/Local/Player_Local_2P";

        /// <summary>
        /// 네트워크 플레이어 1P 프리펩 경로
        /// </summary>
        private readonly string pathOfNetworkPlayer1P = "Player/Network/Player_Network_1P";

        /// <summary>
        /// 네트워크 플레이어 2P 프리펩 경로
        /// </summary>
        private readonly string pathOfNetworkPlayer2P = "Player/Network/Player_Network_2P";

        /// <summary>
        /// 생성된 플레이어 리스트
        /// </summary>
        private List<GameObject> players = new List<GameObject>();
        public List<GameObject> Players
        {
            get
            {
                return players.Where(player => player != null).ToList();
            }
        }

        /// <summary>
        /// 1P가 생성될 위치
        /// </summary>
        [Header("Player Spawn Points")]
        [Tooltip("1P가 생성될 위치")]
        [SerializeField]
        private Transform spawnPoint1P;

        /// <summary>
        /// 2P가 생성될 위치
        /// </summary>
        [Tooltip("2P가 생성될 위치")]
        [SerializeField]
        private Transform spawnPoint2P;

        // ======================================================== //

        /// <summary>
        /// 생성될 코인의 부모 트랜스폼
        /// </summary>
        [Header("Coins")]
        [Tooltip("생성될 코인의 부모 트랜스폼")]
        [SerializeField] 
        private Transform coinsSpawnPointsParent;

        /// <summary>
        /// 코인 프리펩 경로
        /// </summary>
        private string coin = "Gimmicks/Coin";

        /// <summary>
        /// 생성된 코인 리스트
        /// </summary>
        private List<GameObject> coins = new List<GameObject>();

        /// <summary>
        /// 생성된 코인의 개수
        /// </summary>
        public int coinCount
        {
            get
            {
                return coins.Count;
            }
        }

        // ======================================================== //

        /// <summary>
        /// 생성될 박스의 부모 트랜스폼
        /// </summary>
        [Header("Boxs")]
        [Tooltip("생성될 박스의 부모 트랜스폼")]
        [SerializeField] 
        private Transform boxSpawnPointsParent;

        /// <summary>
        /// 박스 프리펩 경로   
        /// </summary>
        private string box = "Gimmicks/Moveable/Box";

        /// <summary>
        /// 생성된 박스 리스트
        /// </summary>
        private List<GameObject> boxs = new List<GameObject>();

        // ======================================================== //

        /// <summary>
        /// 생성될 배럴의 부모 트랜스폼
        /// </summary>
        [Header("Barrels")]
        [Tooltip("생성될 배럴의 부모 트랜스폼")]
        [SerializeField] 
        private Transform barrelSpawnPointsParent;

        /// <summary>
        /// 배럴 프리펩 경로
        /// </summary>
        private string barrel = "Gimmicks/Moveable/Barrel";

        /// <summary>
        /// 생성된 배럴 리스트
        /// </summary>
        private List<GameObject> barrels = new List<GameObject>();

        // ======================================================== //

        /// <summary>
        /// 생성될 달걀의 부모 트랜스폼
        /// </summary>
        [Header("Eggs")]
        [Tooltip("생성될 달걀의 부모 트랜스폼")]
        [SerializeField] 
        private Transform eggSpawnPointsParent;

        /// <summary>
        /// 달걀 프리펩 경로
        /// </summary>
        private string egg = "Gimmicks/Moveable/Egg";

        /// <summary>
        /// 생성된 달걀 리스트
        /// </summary>
        private List<GameObject> eggs = new List<GameObject>();

        // ======================================================== //

        /// <summary>
        /// 레버 도어 리스트
        /// </summary>
        [Header("Gimmicks")]
        [Tooltip("레버 도어 리스트")]
        [SerializeField] 
        private List<Lever_Door> lever_doors;

        /// <summary>
        /// 버튼 도어 리스트
        /// </summary>
        [Tooltip("버튼 도어 리스트")]
        [SerializeField] 
        private List<Button_Door> button_doors;

        /// <summary>
        /// 투 버튼 도어 리스트
        /// </summary>
        [Tooltip("투 버튼 도어 리스트")]
        [SerializeField]
        private List<Two_Button_Door> two_button_doors;

        /// <summary>
        /// 키 도어 리스트
        /// </summary>
        [Tooltip("키 도어 리스트")]
        [SerializeField]
        private List<Key_Door> key_doors;

        /// <summary>
        /// 함정 플랫폼 리스트
        /// </summary>
        [Tooltip("함정 플랫폼 리스트")]
        [SerializeField]
        private List<TrapPlatform> trap_platforms;

        /// <summary>
        /// 보스 프리펩
        /// </summary>
        [Header("Boss")]
        [Tooltip("보스 프리펩")]
        [SerializeField] 
        private GameObject bossPrefab;

        /// <summary>
        /// 보스 생성 위치
        /// </summary>
        [Tooltip("보스 생성 위치")]
        [SerializeField]
        private Transform bossPosition;

        /// <summary>
        /// 보스 인스턴스
        /// </summary>
        private GameObject bossInstance;
        #endregion

        #region Player
        /// <summary>
        /// 플레이어 스폰 메서드        
        /// </summary>
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

        /// <summary>
        /// 플레이어 제거 메서드
        /// </summary>
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

        /// <summary>
        /// 플레이어 제거 메서드 + 죽음 이펙트 생성
        /// </summary>
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

        /// <summary>
        /// 플레이어 속성 업데이트 메서드
        /// </summary>
        /// <param name="player">플레이어 오브젝트</param>
        /// <param name="playerIndex">플레이어 인덱스</param>
        private void UpdatePlayerProperties(GameObject player, int playerIndex)
        {
            ExitGames.Client.Photon.Hashtable playerProperties = PhotonNetwork.CurrentRoom.CustomProperties;

            playerProperties[$"Player{playerIndex}"] = player.GetComponent<PhotonView>().ViewID;
            PhotonNetwork.CurrentRoom.SetCustomProperties(playerProperties);
        }
        #endregion

        #region Photon Callbacks
        /// <summary>
        /// 방 속성 업데이트 콜백
        /// </summary>
        /// <param name="propertiesThatChanged">Hashtable</param>
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

        /// <summary>
        /// 아이템 스폰 메서드
        /// </summary>
        /// <param name="parent">스폰할 오브젝트의 부모</param>
        /// <param name="pathOfPrefab">프리펩 경로(Resources 이용)</param>
        /// <param name="list">참조할 리스트</param>
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

        /// <summary>
        /// 아이템 제거 메서드
        /// </summary>
        /// <param name="list">제거할 아이템 리스트</param>
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

        /// <summary>
        /// 모든 기믹 오브젝트 리셋
        /// </summary>
        public void ResetGimmicks()
        {
            ResetList(lever_doors, door => door.ResetLeverDoor());
            ResetList(button_doors, door => door.ResetButtonDoor());
            ResetList(two_button_doors, door => door.ResetTwoButtonDoor());
            ResetList(key_doors, door => door.ResetKeyDoor());
            ResetList(trap_platforms, platform => platform.ResetTrapPlatform());
        }

        /// <summary>
        /// 리스트 리셋
        /// </summary>
        /// <typeparam name="T">타입</typeparam>
        /// <param name="list">타입 리스트</param>
        /// <param name="resetAction">리셋 시 호출할 이벤트</param>
        private void ResetList<T>(List<T> list, System.Action<T> resetAction)
        {
            foreach (var item in list)
            {
                resetAction?.Invoke(item);
            }
        }

        #endregion

        #region Boss
        /// <summary>
        /// 보스 리셋
        /// </summary>
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

        /// <summary>
        /// 모든 오브젝트 리셋
        /// </summary>
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
