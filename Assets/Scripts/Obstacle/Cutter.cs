using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cutter : InteractableTrigger
{
    protected override void EnterEvent(Collider2D collision)
    {
        collision.GetComponent<PlayerBalloon>().ExplodeBalloon();
    }

    protected override void ExitEvent(Collider2D collision)
    {
        // Nothing
    }
}
