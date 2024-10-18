using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : InteractableTrigger
{
    [Header("Preset")]
    [SerializeField] private Teleport target;

    [Header("Prefab")]
    [SerializeField] private GameObject teleportEffect;

    // private value..
    private HashSet<Collider2D> recents = new HashSet<Collider2D>();

    protected override void EnterEvent(Collider2D collision)
    {
        if (recents.Contains(collision))
        {
            return;
        }

        collision.transform.position = target.transform.position;

        target.recents.Add(collision);

        AudioManager.Instance.PlaySFX("Teleport");

        Instantiate(teleportEffect, transform.position, Quaternion.identity);
        Instantiate(teleportEffect, target.transform.position, Quaternion.identity);
    }

    protected override void ExitEvent(Collider2D collision)
    {
        recents.Remove(collision);
    }
}
