using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformAttachment : InteractableCollision
{
    protected override void EnterEvent(Collision2D collision)
    {
        collision.transform.parent = transform;
    }

    protected override void ExitEvent(Collision2D collision)
    {
        collision.transform.parent = null;
    }
}
