using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : InteractableTrigger
{
    protected override void Event(Collider2D collision)
    {
        GameManager.Instance.DefeatStage();
    }
}
