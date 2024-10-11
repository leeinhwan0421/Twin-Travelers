using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collider2DGizmos : MonoBehaviour
{
    private Collider2D coll;

    private void OnDrawGizmos()
    {
        if (coll == null)
        {
            coll = GetComponent<Collider2D>();
        }

        Vector2 min = coll.bounds.min;
        Vector2 max = coll.bounds.max;

        Gizmos.color = Color.magenta;

        // rectangle
        Vector2 p1 = new Vector2(min.x, max.y);
        Vector2 p2 = new Vector2(max.x, max.y);
        Vector2 p3 = new Vector2(max.x, min.y);
        Vector2 p4 = new Vector2(min.x, min.y);

        Gizmos.DrawLine(p1, p2);
        Gizmos.DrawLine(p2, p3);
        Gizmos.DrawLine(p3, p4);
        Gizmos.DrawLine(p4, p1);
    }
}
