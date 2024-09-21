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

    private float playtime;

    private void Awake()
    {
        instance = this;
        playtime = 0.0f;
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
        SpawnManager.Instance.SpawnPlayers();
    }

    private void UpdatePlaytime(float deltaTime)
    {
        playtime += deltaTime;

        UIManager.Instance.SetTimer(playtime);
    }

    public void DefeatStage()
    {
        UIManager.Instance.DefeatPanel.Enable();
    }

    public void VictoryStage()
    {
        UIManager.Instance.VictoryPanel.Enable();
    }

    public void RestartStage()
    {
        UIManager.Instance.VictoryPanel.Disable();
        UIManager.Instance.DefeatPanel.Disable();

        SpawnManager.Instance.RemovePlayers();
        SpawnManager.Instance.SpawnPlayers();

        playtime = 0.0f;
    }
}
