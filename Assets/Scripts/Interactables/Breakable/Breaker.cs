using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Breaker : InteractableTrigger
{
    protected override void EnterEvent(Collider2D collision)
    {
        collision.GetComponent<Breakable>().Break();
    }

    protected override void ExitEvent(Collider2D collision)
    {

    }
}
