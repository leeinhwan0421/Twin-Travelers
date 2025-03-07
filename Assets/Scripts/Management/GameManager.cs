using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using TwinTravelers.UI;
using TwinTravelers.Core.Network;

namespace TwinTravelers.Management
{
    /// <summary>
    /// 게임 전반적인 관리를 하는 클래스
    /// </summary>
    [RequireComponent(typeof(PhotonView))]
    public class GameManager : MonoBehaviourPunCallbacks
    {
        #region Singletion
        private static GameManager instance;
        public static GameManager Instance
        {
            get
            {
                if (instance == null)
                    return null;

                return instance;
            }
        }
        #endregion

        #region Managers
        /// <summary>
        /// 현재 씬의 UI 매니저
        /// </summary>
        [Header("Managers")]
        [Tooltip("현재 씬의 UI 매니저")]
        public UIManager uiManager;

        /// <summary>
        /// 현재 씬의 스폰 매니저
        /// </summary>
        [Tooltip("현재 씬의 스폰 매니저")]
        public SpawnManager spawnManager;

        /// <summary>
        /// 현재 씬의 스테이지 알림 패널
        /// </summary>
        [Tooltip("현재 씬의 스테이지 알림 패널")]
        public StageAllowPanel stageAllowPanel;
        #endregion

        #region Fields
        /// <summary>
        /// 현재 스테이지의 클리어 시간 제한
        /// </summary>
        [Header("stage settings")]
        [Tooltip("현재 스테이지의 클리어 시간 제한")]
        [SerializeField] 
        private float timeLimit;

        /// <summary>
        /// 현재 스테이지의 코인 획득 제한
        /// </summary>
        [Tooltip("현재 스테이지의 코인 획득 제한")]
        [SerializeField] 
        private int coinLimit;

        /// <summary>
        /// 플레이어 리스트를 반환하는 프로퍼티
        /// </summary>
        public List<GameObject> Player
        {
            get { return spawnManager.Players; }
        }

        /// <summary>
        /// 일시정지 상태인지 반환하는 프로퍼티
        /// </summary>
        private bool isPause;
        public bool IsPause => isPause;

        /// <summary>
        /// 현재 테마
        /// </summary>
        private int theme;
        public int Theme => theme;

        /// <summary>
        /// 현재 스테이지
        /// </summary>
        private int stage;
        public int Stage => stage;

        /// <summary>
        /// 플레이 시간
        /// </summary>
        private float playtime;

        /// <summary>
        /// 획득한 코인 수 
        /// </summary>
        private int earnedCoin;

        /// <summary>
        /// 게임이 로드된 플레이어의 수
        /// </summary>
        private int loadedPlayerCount;
        #endregion

        #region Unity Methods

        private void Awake()
        {
            instance = this;
        }

        private void Start()
        {
            switch (RoomManager.Instance.playmode)
            {
                case Playmode.Multi:
                    OnSceneLoaded();
                    break;

                case Playmode.Single:
                    stageAllowPanel.gameObject.SetActive(true);
                    break;
            }
        }

        private void Update()
        {
            UpdatePlaytime(Time.deltaTime);
        }

        #endregion

        #region Initialize
        /// <summary>
        /// 현재 스테이지 데이터를 모두 초기화합니다.
        /// </summary>
        public void InitializeStage()
        {
            string[] parts = SceneManager.GetActiveScene().name.Split(' ');

            theme = int.Parse(parts[1]) - 1;
            stage = int.Parse(parts[3]) - 1;

            spawnManager.ResetAll();

            playtime = 0.0f;
            earnedCoin = 0;
            isPause = false;
            coinLimit = spawnManager.coinCount;

            uiManager.SetEarnedCoinText(earnedCoin);
            uiManager.SetTimeText(playtime);
        }

        /// <summary>
        /// UI를 초기화합니다.
        /// </summary>
        private void InitailizeUI()
        {
            if (uiManager.VictoryPanel.gameObject.activeSelf)
            {
                uiManager.VictoryPanel.Disable();
            }

            if (uiManager.DefeatPanel.gameObject.activeSelf)
            {
                uiManager.DefeatPanel.Disable();
            }

            if (uiManager.PausePanel.gameObject.activeSelf)
            {
                uiManager.PausePanel.Disable();
            }

            uiManager.ResultBackgroundPanel.SetActive(false);
        }
        #endregion

        #region Methods
        /// <summary>
        /// 플레이 시간을 업데이트합니다.
        /// </summary>
        /// <param name="deltaTime">Time.deltaTime</param>
        private void UpdatePlaytime(float deltaTime)
        {
            if (uiManager.VictoryPanel.gameObject.activeSelf ||
                uiManager.DefeatPanel.gameObject.activeSelf)
                return;

            playtime += deltaTime;
            uiManager.SetTimeText(playtime);
        }

        /// <summary>
        /// 코인을 획득 할 때, 호출되는 메서드
        /// </summary>
        /// <param name="value">더할 코인의 갯수</param>
        public void EarnCoin(int value)
        {
            earnedCoin += value;
            uiManager.SetEarnedCoinText(earnedCoin);
        }

        /// <summary>
        /// 별 갯수를 반환합니다 (클리어 시)        
        /// /// </summary>
        /// <returns>획득한 별 갯수</returns>
        public int ReturnStarCount()
        {
            int count = 1; // 클리어 시, 기본 1 지급.

            if (timeLimit >= playtime) // 시간 제한보다 일찍 클리어
            {
                count++;
            }

            if (coinLimit <= earnedCoin) // 코인 제한보다 코인 많이 먹으면
            {
                count++;
            }

            return count;
        }
        #endregion

        #region Defeat
        /// <summary>
        /// 스테이지 패배 시, 호출되는 메서드
        /// </summary>
        public void DefeatStage()
        {
            switch (RoomManager.Instance.playmode)
            {
                case Playmode.Multi:
                    photonView.RPC("RPC_DefeatStage", RpcTarget.AllBuffered);
                    break;
                case Playmode.Single:
                    RPC_DefeatStage();
                    break;
            }
        }

        /// <summary>
        /// 스테이지 패배 시, 모든 클라이언트에게 호출되는 RPC 이벤트
        /// </summary>
        [PunRPC]
        private void RPC_DefeatStage()
        {
            spawnManager.RemovePlayersWithDeadEffect();

            AudioManager.Instance.PlaySFX("Defeat");

            uiManager.DefeatPanel.Enable();
            uiManager.DefeatPanel.SetEarnedCoinText(earnedCoin);

            uiManager.ResultBackgroundPanel.SetActive(true);
        }
        #endregion

        #region Victory

        /// <summary>
        /// 스테이지 클리어 시, 호출되는 메서드
        /// </summary>
        public void VictoryStage()
        {
            switch (RoomManager.Instance.playmode)
            {
                case Playmode.Multi:
                    photonView.RPC("RPC_VictoryStage", RpcTarget.AllBuffered);
                    break;
                case Playmode.Single:
                    RPC_VictoryStage();
                    break;
            }
        }

        /// <summary>
        /// 스테이지 클리어 시, 모든 클라이언트에게 호출되는 RPC 이벤트 
        /// </summary>
        [PunRPC]
        private void RPC_VictoryStage()
        {
            spawnManager.RemovePlayers();

            int starCount = ReturnStarCount();

            AudioManager.Instance.PlaySFX("Victory");

            uiManager.VictoryPanel.Enable();
            uiManager.VictoryPanel.SetEarnedCoinText(earnedCoin);
            uiManager.VictoryPanel.SetDisplayStarCount(starCount);

            uiManager.ResultBackgroundPanel.SetActive(true);

            LevelManager.CompleteStage(theme, stage, starCount);
        }
        #endregion

        #region Restart
        /// <summary>
        /// 스테이지 재시작 시, 호출되는 메서드
        /// </summary>
        public void RestartStage()
        {
            switch (RoomManager.Instance.playmode)
            {
                case Playmode.Multi:
                    photonView.RPC("RPC_RestartStage", RpcTarget.AllBuffered);
                    break;
                case Playmode.Single:
                    RPC_RestartStage();
                    break;
            }
        }

        /// <summary>
        /// 스테이지 재시작 시, 모든 클라이언트에게 호출되는 RPC 이벤트
        /// </summary>
        [PunRPC]
        private void RPC_RestartStage()
        {
            InitailizeUI();

            spawnManager.ResetAll();

            playtime = 0.0f;
            earnedCoin = 0;
            isPause = false;

            uiManager.SetEarnedCoinText(earnedCoin);
            uiManager.SetTimeText(playtime);
        }
        #endregion

        #region Pause, Resume
        /// <summary>
        /// 일시정지
        /// </summary>
        public void Pause()
        {
            isPause = true;

            switch (RoomManager.Instance.playmode)
            {
                case Playmode.Multi:
                    // Nothing
                    break;
                default:
                    Time.timeScale = 0.0f;
                    break;
            }

            AudioManager.Instance.PlaySFX("Pause");

            uiManager.PausePanel.Enable();
            uiManager.ResultBackgroundPanel.SetActive(true);
        }

        /// <summary>
        /// 재개
        /// </summary>
        public void Resume()
        {
            isPause = false;

            switch (RoomManager.Instance.playmode)
            {
                case Playmode.Multi:
                    // Nothing
                    break;
                default:
                    Time.timeScale = 1.0f;
                    break;
            }

            AudioManager.Instance.PlaySFX("Resume");

            uiManager.PausePanel.Disable();
            uiManager.ResultBackgroundPanel.SetActive(false);
        }
        #endregion

        #region PUN Callbacks

        /// <summary>
        /// 플레이어가 로드 될 시, 호출되는 RPC 이벤트
        /// </summary>
        [PunRPC]
        private void RPC_PlayerLoaded()
        {
            loadedPlayerCount++;

            Debug.Log($"Player Loaded: {loadedPlayerCount}/{RoomManager.Instance.maxPlayerCount}");

            if (loadedPlayerCount >= RoomManager.Instance.maxPlayerCount)
            {
                photonView.RPC("RPC_AllPlayersLoaded", RpcTarget.AllBuffered);
            }
        }

        /// <summary>
        /// 모든 플레이어가 로드되었을 때, 호출되는 RPC 이벤트
        /// </summary>
        [PunRPC]
        private void RPC_AllPlayersLoaded()
        {
            stageAllowPanel.gameObject.SetActive(true);

            Debug.Log("All players loaded. Stage allow panel activated.");

            foreach (var player in PhotonNetwork.PlayerList)
            {
                ExitGames.Client.Photon.Hashtable props = new ExitGames.Client.Photon.Hashtable
            {
                { "SceneLoaded", null }
            };
                player.SetCustomProperties(props);
            }
        }

        /// <summary>
        /// 플레이어가 씬을 로드했을 때, RPC_PlayerLoaded RPC 이벤트를 호출합니다.
        /// </summary>
        public void OnSceneLoaded()
        {
            ExitGames.Client.Photon.Hashtable props = new ExitGames.Client.Photon.Hashtable
        {
            { "SceneLoaded", true }
        };

            PhotonNetwork.LocalPlayer.SetCustomProperties(props);

            photonView.RPC("RPC_PlayerLoaded", RpcTarget.AllBuffered);

            Debug.Log("Scene loaded and notified to every client.");
        }

        #endregion
    }
}
