using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalloonTrigger : InteractableTrigger
{
    protected override void EnterEvent(Collider2D collision)
    {
        Player player = collision.GetComponent<Player>();

        if (player == null || player.PlayerBalloon == null)
        {
            return;
        }

        player.PlayerBalloon.gameObject.SetActive(true);
    }

    protected override void ExitEvent(Collider2D collision)
    {
        // Nothing
    }
}
