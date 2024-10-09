using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformAttachment : InteractableCollision
{
    private HashSet<Collision2D> colls = new HashSet<Collision2D>();
    private Vector3 prev;

    private void Start()
    {
        prev = transform.position;
    }

    private void Update()
    {
        Vector3 cur = transform.position;
        
        foreach(Collision2D coll in colls)
        {
            coll.transform.position += (cur - prev);
        } 

        prev = cur;
    }

    protected override void EnterEvent(Collision2D collision)
    {
        colls.Add(collision);
    }

    protected override void ExitEvent(Collision2D collision)
    {
        colls.Remove(collision);
    }
}
