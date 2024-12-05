using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;

using Photon.Pun;

[RequireComponent(typeof(PhotonView))]
public class GameManager : MonoBehaviourPunCallbacks
{
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

    [Header("Managers")]
    public UIManager uiManager;
    public SpawnManager spawnManager;
    public StageAllowPanel stageAllowPanel;

    [Header("stage settings")]
    [SerializeField] private float timeLimit;
    [SerializeField] private int coinLimit;

    // public properties
    public List<GameObject> Player
    {
        get { return spawnManager.Players; }
    }

    // Play Status
    private bool isPause;
    public bool IsPause { get { return isPause; } }

    // private data...
    private int theme;
    public int Theme { get { return theme; } }

    private int stage;
    public int Stage { get { return stage; } }

    private float playtime;
    private int earnedCoin;

    // Player Infos
    private int loadedPlayerCount;

    #region LifeCycle

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        switch (RoomManager.Instance.playmode)
        {
            case RoomManager.Playmode.Multi:
                OnSceneLoaded();
                break;

            case RoomManager.Playmode.Single:
                stageAllowPanel.gameObject.SetActive(true);
                break;
        }
    }

    private void Update()
    {
        UpdatePlaytime(Time.deltaTime);
    }

    #endregion

    #region PUN Callbacks

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

    #region playtime
    private void UpdatePlaytime(float deltaTime)
    {
        if (uiManager.VictoryPanel.gameObject.activeSelf || 
            uiManager.DefeatPanel.gameObject.activeSelf) 
            return;
            
        playtime += deltaTime;
        uiManager.SetTimeText(playtime);
    }
    #endregion

    #region Earned Coin
    public void EarnCoin(int value)
    {
        earnedCoin += value;
        uiManager.SetEarnedCoinText(earnedCoin);
    }

    #endregion

    #region Initialize
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
    #endregion

    #region Defeat
    public void DefeatStage()
    {
        switch (RoomManager.Instance.playmode)
        {
            case RoomManager.Playmode.Multi:
                photonView.RPC("RPC_DefeatStage", RpcTarget.AllBuffered);
                break;
            case RoomManager.Playmode.Single:
                RPC_DefeatStage();
                break;
        }
    }

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
    public void VictoryStage()
    {
        switch (RoomManager.Instance.playmode)
        {
            case RoomManager.Playmode.Multi:
                photonView.RPC("RPC_VictoryStage", RpcTarget.AllBuffered);
                break;
            case RoomManager.Playmode.Single:
                RPC_VictoryStage();
                break;
        }
    }

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
    public void RestartStage()
    {
        switch (RoomManager.Instance.playmode)
        {
            case RoomManager.Playmode.Multi:
                photonView.RPC("RPC_RestartStage", RpcTarget.AllBuffered);
                break;
            case RoomManager.Playmode.Single:
                RPC_RestartStage();
                break;
        }
    }

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

    #region Other
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

    public int ReturnStarCount()
    {
        int count = 1; // Ŭ���� ��, �⺻ 1 ����.

        if (timeLimit >= playtime) // �ð� ���Ѻ��� ���� Ŭ����
        {
            count++;
        }

        if (coinLimit <= earnedCoin) // ���� ���Ѻ��� ���� ���� ������
        {
            count++;
        }

        return count;
    }
    #endregion

    #region Pause, Resume
    public void Pause()
    {
        isPause = true;

        switch (RoomManager.Instance.playmode)
        {
            case RoomManager.Playmode.Multi:
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

    public void Resume()
    {
        isPause = false;

        switch (RoomManager.Instance.playmode)
        {
            case RoomManager.Playmode.Multi:
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
}
