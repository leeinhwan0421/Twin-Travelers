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
    private float cooldown = 0.1f;

    protected override void Event(Collider2D collision)
    {
        if (recents.Contains(collision))
        {
            return;
        }

        collision.transform.position = target.transform.position;

        target.recents.Add(collision);
        target.StartCoroutine(target.RemoveFromRecentCollider(collision, cooldown));

        Instantiate(teleportEffect, transform.position, Quaternion.identity);
        Instantiate(teleportEffect, target.transform.position, Quaternion.identity);
    }

    private IEnumerator RemoveFromRecentCollider(Collider2D coll, float delay)
    {
        yield return new WaitForSeconds(delay);
        recents.Remove(coll);
    }
}
