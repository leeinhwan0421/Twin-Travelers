using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnCollisionHit : InteractableTrigger
{
    protected override void EnterEvent(Collider2D collision)
    {
        Destroy(gameObject);
    }

    protected override void ExitEvent(Collider2D collision)
    {
        // Nothing
    }
}
