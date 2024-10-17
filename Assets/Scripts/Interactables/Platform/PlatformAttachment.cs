using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformAttachment : InteractableCollision
{
    private HashSet<GameObject> colls = new HashSet<GameObject>();
    private Vector3 prev;

    private void Start()
    {
        prev = transform.position;
    }

    private void Update()
    {
        Vector3 cur = transform.position;
        
        foreach(GameObject item in colls)
        {
            item.transform.position += (cur - prev);
        } 

        prev = cur;
    }

    protected override void EnterEvent(Collision2D collision)
    {
        colls.Add(collision.gameObject);
    }

    protected override void ExitEvent(Collision2D collision)
    {
        colls.Remove(collision.gameObject);
    }
}
