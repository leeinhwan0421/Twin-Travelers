using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Breakable : MonoBehaviour
{
    [Header("Preset")]
    [SerializeField] private GameObject effect;

    private Animator animator;

    private Collider2D coll;
    private Rigidbody2D rigid;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        coll = GetComponent<Collider2D>();
        rigid = GetComponent<Rigidbody2D>();
    }

    public void Break()
    {
        if (effect != null)
        {
            Instantiate(effect, transform.position, Quaternion.identity);
        }

        coll.enabled = false;
        Destroy(rigid);

        animator.SetTrigger("Break");
    }

    private void Remove()
    {
        GameObject.Destroy(gameObject);
    }
}
