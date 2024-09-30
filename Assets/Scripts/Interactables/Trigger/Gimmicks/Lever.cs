using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lever : InteractableTrigger
{
    // private properties...
    private Animator animator;
    private bool isActivate = true;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    protected override void Event(Collider2D collision)
    {
        if (isActivate) 
            return;
        
        isActivate = false;
        animator.SetTrigger("Active");
    }

    public void ResetLever()
    {
        animator.Rebind();
        isActivate = true;
    }
}
