using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToxicGas : InteractableTrigger
{
    protected override void EnterEvent(Collider2D collision)
    {
        AudioManager.Instance.PlaySFX("ToxicGas");
    }

    protected override void ExitEvent(Collider2D collision)
    {
        // Nothing..
    }
}
