using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

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

    // private data..
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

    private void InitializeStage()
    {
        playtime = 0.0f;
        SpawnManager.Instance.SpawnPlayers();
        SpawnManager.Instance.SpawnCoins();
    }

    private void UpdatePlaytime(float deltaTime)
    {
        if (UIManager.Instance.VictoryPanel.gameObject.activeSelf || 
            UIManager.Instance.DefeatPanel.gameObject.activeSelf) 
            return;
            
        playtime += deltaTime;
        UIManager.Instance.SetTimer(playtime);
    }

    #region Earned Coin
    public void EarnCoin(int value)
    {
        earnedCoin += value;
    }
    
    #endregion

    #region Defeat, Victory, Restart
    public void DefeatStage()
    {
        SpawnManager.Instance.RemovePlayersWithDeadEffect();

        UIManager.Instance.DefeatPanel.Enable();
        UIManager.Instance.DefeatPanel.SetEarnedCoinText(earnedCoin);
    }

    public void VictoryStage()
    {
        SpawnManager.Instance.RemovePlayers();

        UIManager.Instance.VictoryPanel.Enable();
        UIManager.Instance.VictoryPanel.SetEarnedCoinText(earnedCoin);
        UIManager.Instance.VictoryPanel.SetDisplayStarCount(ReturnStarCount());
    }

    private int ReturnStarCount()
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

        SpawnManager.Instance.RemovePlayers();
        SpawnManager.Instance.SpawnPlayers();

        SpawnManager.Instance.RemoveCoins();
        SpawnManager.Instance.SpawnCoins();

        playtime = 0.0f;
        earnedCoin = 0;
    }
    #endregion
}
