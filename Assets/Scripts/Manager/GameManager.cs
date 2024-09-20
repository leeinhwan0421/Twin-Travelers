using System.Collections;
using System.Collections.Generic;
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

    private void Awake()
    {
        instance = this;
    }

    public void Start()
    {
        InitializeStage();
    }

    public void InitializeStage()
    {
        SpawnManager.Instance.SpawnPlayers();
    }
}
