using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    private static SpawnManager instance;
    public static SpawnManager Instance
    {
        get
        {
            if (instance == null)
                return null;

            return instance;
        }
    }

    [Header("Player Spawn Points")]
    [SerializeField] private Transform spawnPoint1P; // 1P SpawnPoint
    [SerializeField] private Transform spawnPoint2P; // 2P SpawnPoint

    [Header("Prefabs")]
    [SerializeField] private GameObject player1P;    // 1P Prefab
    [SerializeField] private GameObject player2P;    // 2P Prefab

    // Instance
    private List<GameObject> players = new List<GameObject>();
    public List<GameObject> Players
    {
        get
        {
            return players;
        }
    }

    [Header("Coin Spawn Points")]
    [SerializeField] private List<GameObject> coinsSpawnPoint;

    [Header("Prefabs")]
    [SerializeField] private GameObject coin;

    private List<GameObject> coins = new List<GameObject>();
    public List<GameObject> Coins
    {
        get
        {
            return coins;
        }
    }

    private void Awake()
    {
        instance = this;
    }

    #region Player
    public void SpawnPlayers()
    {
        if (players.Count > 0)
        {
#if SHOW_DEBUG_MESSAGES
            Debug.Log("called spawn timing is invalid");
#endif
            RemovePlayers();
        }

        players.Add(Instantiate(player1P, spawnPoint1P.position, Quaternion.identity));
        players.Add(Instantiate(player2P, spawnPoint2P.position, Quaternion.identity));
    }

    public void RemovePlayers()
    {
        for (int i = 0; i < players.Count; i++)
        {
            if (players[i] != null)
            {
                Destroy(players[i]);
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
                Destroy(players[i]);
            }
        }

        players.Clear();
    }
    #endregion

    #region Coin
    public void SpawnCoins()
    {
        if (coins.Count > 0)
        {
            RemoveCoins();
        }

        for (int i = 0; i < coinsSpawnPoint.Count; i++)
        {
            Coins.Add(Instantiate(coin, coinsSpawnPoint[i].transform.position, Quaternion.identity));
        }
    }

    public void RemoveCoins()
    {
        for (int i = 0; i < coins.Count; i++)
        {
            if (coins[i] != null)
            {
                Destroy(coins[i]);
            }
        }

        coins.Clear();
    }
    #endregion
}
