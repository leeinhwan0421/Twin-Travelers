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

    [Header("stage settings")]
    [SerializeField] private float timeLimit;
    [SerializeField] private int coinLimit;

    // private data...
    private int theme;
    private int stage;

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
        if (UIManager.Instance.VictoryPanel.gameObject.activeSelf || 
            UIManager.Instance.DefeatPanel.gameObject.activeSelf) 
            return;
            
        playtime += deltaTime;
        UIManager.Instance.SetTimeText(playtime);
    }
    #endregion

    #region Earned Coin
    public void EarnCoin(int value)
    {
        earnedCoin += value;
        UIManager.Instance.SetEarnedCoinText(earnedCoin);
    }

    #endregion

    #region Initialize, Defeat, Victory, Restart
    private void InitializeStage()
    {
        string[] parts = SceneManager.GetActiveScene().name.Split(' ');

        theme = int.Parse(parts[1]) - 1;
        stage = int.Parse(parts[3]) - 1;

        SpawnManager.Instance.ResetAll();

        playtime = 0.0f;
        earnedCoin = 0;

        UIManager.Instance.SetEarnedCoinText(earnedCoin);
        UIManager.Instance.SetTimeText(playtime);
    }

    public void DefeatStage()
    {
        SpawnManager.Instance.RemovePlayersWithDeadEffect();

        UIManager.Instance.DefeatPanel.Enable();
        UIManager.Instance.DefeatPanel.SetEarnedCoinText(earnedCoin);
    }

    public void VictoryStage()
    {
        SpawnManager.Instance.RemovePlayers();

        int starCount = ReturnStarCount();

        UIManager.Instance.VictoryPanel.Enable();
        UIManager.Instance.VictoryPanel.SetEarnedCoinText(earnedCoin);
        UIManager.Instance.VictoryPanel.SetDisplayStarCount(starCount);

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
        if (UIManager.Instance.VictoryPanel.gameObject.activeSelf)
        {
            UIManager.Instance.VictoryPanel.Disable();
        }

        if (UIManager.Instance.DefeatPanel.gameObject.activeSelf)
        {
            UIManager.Instance.DefeatPanel.Disable();
        }

        SpawnManager.Instance.ResetAll();

        playtime = 0.0f;
        earnedCoin = 0;

        UIManager.Instance.SetEarnedCoinText(earnedCoin);
        UIManager.Instance.SetTimeText(playtime);
    }
    #endregion
}
