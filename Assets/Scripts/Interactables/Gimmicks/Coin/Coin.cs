using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : InteractableTrigger
{
    [Header("Preset")]
    [SerializeField] private GameObject earnEffect;

    private bool isActivate = true;

    private void InstantiateEarnEffect()
    {
        Instantiate(earnEffect, transform.position, Quaternion.identity);
    }

    protected override void EnterEvent(Collider2D collision)
    {
        if (!isActivate)
            return;

        isActivate = false;

        GameManager.Instance.EarnCoin(1);
        AudioManager.Instance.PlaySFX("earnedCoin");
        InstantiateEarnEffect();

        Destroy(gameObject);
    }

    protected override void ExitEvent(Collider2D collision)
    {
        // Nothing
    }
}
