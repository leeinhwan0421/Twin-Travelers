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

    [Header("Spawn Points")]
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

    private void Awake()
    {
        instance = this;
    }

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
        foreach (var iter in players)
        {
            if (iter != null)
            {
                iter.GetComponent<Player>().InstantiateDeadEffect();
                Destroy(iter);
            }
        }

        players.Clear();
    }
}
