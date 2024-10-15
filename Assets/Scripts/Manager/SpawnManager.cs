using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [Header("Player Spawn Points")]
    [SerializeField] private Transform spawnPoint1P; // 1P SpawnPoint
    [SerializeField] private Transform spawnPoint2P; // 2P SpawnPoint

    [Header("Player Prefabs")]
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

    // ======================================================== //

    [Header("Coins")]
    [SerializeField] private Transform coinsSpawnPointsParent;
    [SerializeField] private GameObject coin;

    private List<GameObject> coins = new List<GameObject>();

    public int coinCount
    {
        get
        {
            return coins.Count;
        }
    } 

    // ======================================================== //

    [Header("Boxs")]
    [SerializeField] private Transform boxSpawnPointsParent;
    [SerializeField] private GameObject box;

    private List<GameObject> boxs = new List<GameObject>();

    // ======================================================== //

    [Header("Barrels")]
    [SerializeField] private Transform barrelSpawnPointsParent;
    [SerializeField] private GameObject barrel;

    private List<GameObject> barrels = new List<GameObject>();

    // ======================================================== //

    [Header("Gimmicks")]
    [SerializeField] private List<Lever> levers;

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

        for (int i = 0; i < coinsSpawnPointsParent.childCount; i++)
        {
            Transform spawnPoint = coinsSpawnPointsParent.GetChild(i);
            coins.Add(Instantiate(coin, spawnPoint.position, Quaternion.identity));
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

    #region Box
    public void SpawnBoxs()
    {
        if (boxs.Count > 0)
        {
            RemoveBoxs();
        }

        for (int i = 0; i < boxSpawnPointsParent.childCount; i++)
        {
            Transform spawnPoint = boxSpawnPointsParent.GetChild(i);
            boxs.Add(Instantiate(box, spawnPoint.position, Quaternion.identity));
        }
    }

    public void RemoveBoxs()
    {
        for (int i = 0; i < boxs.Count; i++)
        {
            if (boxs[i] != null)
            {
                Destroy(boxs[i]);
            }
        }

        boxs.Clear();
    }
    #endregion

    #region Barrel
    public void SpawnBarrels()
    {
        if (barrels.Count > 0)
        {
            RemoveBarrels();
        }

        for (int i = 0; i < barrelSpawnPointsParent.childCount; i++)
        {
            Transform spawnPoint = barrelSpawnPointsParent.GetChild(i);
            barrels.Add(Instantiate(barrel, spawnPoint.position, Quaternion.identity));
        }
    }

    public void RemoveBarrels()
    {
        for (int i = 0; i < barrels.Count; i++)
        {
            if (barrels[i] != null)
            {
                Destroy(barrels[i]);
            }
        }

        barrels.Clear();
    }
    #endregion

    #region Gimmick
    public void ResetLevers()
    {
        if (levers.Count <= 0)
        {
            return;
        }

        for (int i = 0; i < levers.Count; i++)
        {
            if (levers[i] != null)
            {
                levers[i].GetComponent<Lever>().ResetLever();
            }
        }
    }
    #endregion

    public void ResetAll()
    {
        RemovePlayers();
        SpawnPlayers();

        RemoveCoins();
        SpawnCoins();

        RemoveBoxs();
        SpawnBoxs();

        RemoveBarrels();
        SpawnBarrels();

        ResetLevers();
    }
}
