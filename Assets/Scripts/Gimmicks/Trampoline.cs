using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trampoline : InteractableCollision
{
    [Header("Presets")]
    [SerializeField] private Vector2 velocity;

    private Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    protected override void EnterEvent(Collision2D collision)
    {
        Rigidbody2D rigid2D = collision.gameObject.GetComponent<Rigidbody2D>();
        rigid2D.velocity = velocity;

        anim.SetTrigger("Jump");
        AudioManager.Instance.PlaySFX("Trampoline");
    }

    protected override void ExitEvent(Collision2D collision)
    {
        anim.ResetTrigger("Jump");
    }
}