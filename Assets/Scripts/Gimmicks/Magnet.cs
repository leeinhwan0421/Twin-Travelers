using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magnet : InteractableTrigger
{
    [Header("Preset")]
    [Range(0.01f, 10.0f)] [SerializeField] private float pullSpeed;

    private HashSet<GameObject> colls = new HashSet<GameObject>();

    private void Update()
    {
        foreach (GameObject coll in colls)
        {
            Vector3 dir = transform.position - coll.transform.position;

            if (coll.TryGetComponent<Rigidbody2D>(out Rigidbody2D rigid))
            {
                rigid.velocity = Vector2.zero;
            }

            coll.transform.position += pullSpeed * Time.unscaledDeltaTime * dir.normalized; 
        }
    }

    protected override void EnterEvent(Collider2D collision)
    {
        colls.Add(collision.gameObject);
    }

    protected override void ExitEvent(Collider2D collision)
    {
        colls.Remove(collision.gameObject);
    }
}
