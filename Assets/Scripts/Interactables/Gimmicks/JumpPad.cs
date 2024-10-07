using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPad : InteractableTrigger
{
    [Header("Preset")]
    [Range(1.0f, 5.0f)][SerializeField] private float jumpMultiplier = 1.0f;

    protected override void EnterEvent(Collider2D collision)
    {
        collision.GetComponent<Player>().jumpMultiplier = this.jumpMultiplier;
    }

    protected override void ExitEvent(Collider2D collision)
    {
        collision.GetComponent<Player>().jumpMultiplier = 1.0f;
    }
}
