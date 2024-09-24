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
    }

    private void UpdatePlaytime(float deltaTime)
    {
        if (UIManager.Instance.VictoryPanel.gameObject.activeSelf || 
            UIManager.Instance.DefeatPanel.gameObject.activeSelf) 
            return;
            
        playtime += deltaTime;
        UIManager.Instance.SetTimer(playtime);
    }

    public void DefeatStage()
    {
        SpawnManager.Instance.RemovePlayersWithDeadEffect();
        UIManager.Instance.DefeatPanel.Enable();
    }

    public void VictoryStage()
    {
        UIManager.Instance.VictoryPanel.Enable();

        SpawnManager.Instance.RemovePlayers();
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

        playtime = 0.0f;
    }
}
