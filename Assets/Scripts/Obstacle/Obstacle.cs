using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : InteractableTrigger
{
    protected override void EnterEvent(Collider2D collision)
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.DefeatStage();
        }
    }

    protected override void ExitEvent(Collider2D collision)
    {
        // Nothing!
    }
}
