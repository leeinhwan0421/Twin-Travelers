using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;
using UnityEngine;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using System;

public class GameManager : MonoBehaviour
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

    [Header("stage settings")]
    [SerializeField] private float timeLimit;
    [SerializeField] private int coinLimit;

    // public properties
    public List<GameObject> Player
    {
        get { return spawnManager.Players; }
    }

    // private data...
    private int theme;
    public int Theme { get { return theme; } }

    private int stage;
    public int Stage { get { return stage; } }

    private float playtime;
    private int earnedCoin;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        InitializeStage();
    }

    private void Update()
    {
        UpdatePlaytime(Time.deltaTime);
    }

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

    #region Initialize, Defeat, Victory, Restart
    private void InitializeStage()
    {
        string[] parts = SceneManager.GetActiveScene().name.Split(' ');

        theme = int.Parse(parts[1]) - 1;
        stage = int.Parse(parts[3]) - 1;

        spawnManager.ResetAll();

        playtime = 0.0f;
        earnedCoin = 0;

        uiManager.SetEarnedCoinText(earnedCoin);
        uiManager.SetTimeText(playtime);
    }

    public void DefeatStage()
    {
        spawnManager.RemovePlayersWithDeadEffect();

        uiManager.DefeatPanel.Enable();
        uiManager.DefeatPanel.SetEarnedCoinText(earnedCoin);
    }

    public void VictoryStage()
    {
        spawnManager.RemovePlayers();

        int starCount = ReturnStarCount();

        uiManager.VictoryPanel.Enable();
        uiManager.VictoryPanel.SetEarnedCoinText(earnedCoin);
        uiManager.VictoryPanel.SetDisplayStarCount(starCount);

        LevelManager.CompleteStage(theme, stage, starCount);
    }

    private int ReturnStarCount()
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

    public void RestartStage()
    {
        if (uiManager.VictoryPanel.gameObject.activeSelf)
        {
            uiManager.VictoryPanel.Disable();
        }

        if (uiManager.DefeatPanel.gameObject.activeSelf)
        {
            uiManager.DefeatPanel.Disable();   
        }

        spawnManager.ResetAll();

        playtime = 0.0f;
        earnedCoin = 0;

        uiManager.SetEarnedCoinText(earnedCoin);
        uiManager.SetTimeText(playtime);
    }
    #endregion
}
