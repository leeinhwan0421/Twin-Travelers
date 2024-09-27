using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : InteractableTrigger
{
    protected override void Event()
    {
        GameManager.Instance.DefeatStage();
    }
}
