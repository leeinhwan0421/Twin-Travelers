using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : InteractableTrigger
{
    [Header("Preset")]
    [SerializeField] private GameObject earnEffect;

    private void InstantiateEarnEffect()
    {
        Instantiate(earnEffect, transform.position, Quaternion.identity);
    }

    protected override void Event()
    {
        GameManager.Instance.EarnCoin(1);
        InstantiateEarnEffect();
        Destroy(gameObject);
    }
}
