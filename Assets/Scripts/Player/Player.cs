using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    enum PlayerType
    {
        Online,
        Offline
    }

    [Header("Types")]
    [SerializeField] private PlayerType playerType; 

    [Header("Preset")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float jumpVelocity;

    // Private Values
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rigid;
    private Collider2D coll;

    private Vector3 moveDirection;

    private float checkGroundDistance = 0.1f;
    private bool isGrounded;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        rigid = GetComponent<Rigidbody2D>();
        coll = GetComponent<Collider2D>();

        if (playerType == PlayerType.Offline)
        {
            GetComponent<PlayerInput>().SwitchCurrentControlScheme(new[] { Keyboard.current });
        }
    }

    private void Update()
    {
        SetAnimatorParameters();
    }

    private void FixedUpdate()
    {
        Move();
        OnGrounded();
    }

    // 이동 및 여러가지 함수 
    private void Move()
    {
        if (moveDirection == Vector3.zero)
            return;

        transform.Translate(moveDirection * Time.fixedDeltaTime * moveSpeed);
        spriteRenderer.flipX = moveDirection.x < 0;
    }

    private void OnGrounded()
    {
        // 모서리에 걸쳐있을 때도 점프가 가능하도록 하기 위해서, bounds.min.x, bounds.max.x 위치에서도 동일하게 작업해줍니다.
        Vector2[] list = new Vector2[] {
            new Vector2(coll.bounds.min.x, coll.bounds.min.y),
            new Vector2(transform.position.x, coll.bounds.min.y),
            new Vector2(coll.bounds.max.x, coll.bounds.min.y),
        };

        foreach(Vector2 v in list)
        {
            RaycastHit2D hit = Physics2D.Raycast(v, Vector3.down, checkGroundDistance, LayerMask.GetMask("Ground"));

            if (hit.collider != null)
            {
                isGrounded = true;
                break;
            }
            else
            {
                isGrounded = false;
            }
        }
    }

    // 애니메이터 설정
    private void SetAnimatorParameters()
    {
        animator.SetBool("IsMove", moveDirection.x != 0);
        animator.SetBool("IsGrounded", isGrounded);
        animator.SetFloat("Velocity", rigid.velocity.y);
    }

    // 입력 관련 함수
    private void OnMove(InputValue value)
    {
        float axis = value.Get<float>();

        moveDirection.x = axis;
    }

    private void OnJump()
    {
        if (!isGrounded) return;

        rigid.velocity = Vector2.up * jumpVelocity;
    }
}