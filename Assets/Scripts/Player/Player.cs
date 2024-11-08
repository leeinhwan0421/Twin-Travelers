using System;
using System.Collections;
using System.Collections.Generic;
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
    public float jumpMultiplier = 1.0f;

    private float JumpVelocity
    {
        get
        {
            return this.jumpVelocity * jumpMultiplier;
        }
    }

    [Header("Effect")]
    [SerializeField] private GameObject deadEffect;

    [Header("Scripts")]
    [SerializeField] private PlayerBalloon playerBallon;

    public PlayerBalloon PlayerBalloon
    {
        get
        {
            return playerBallon;
        }
    }

    // Private Values
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rigid;
    private Collider2D coll;

    private Vector3 moveDirection;

    private float checkGroundDistance = 0.1f;

    private bool isGrounded;
    private GravityPortal.GravityPortalType gravityType;

    #region Unity LifeCycle
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

        ChangeGravity(GravityPortal.GravityPortalType.Down);
    }

    private void Update()
    {
        SetAnimatorParameters();
        Move();
        OnGrounded();
    }
    #endregion

    #region PlayerInput Event Handle
    /// <summary>
    /// Player Input Send Message
    /// </summary>
    /// <param name="value">1D Axis</param>
    private void OnMove(InputValue value)
    {
        float axis = value.Get<float>();

        moveDirection.x = axis;
    }

    /// <summary>
    /// Player Input Send Message
    /// </summary>
    private void OnJump()
    {
        if (!isGrounded) return;

        AudioManager.Instance.PlaySFX("Jump");

        rigid.velocity = transform.up.normalized * JumpVelocity;
    }

    /// <summary>
    /// Player Input Send Message
    /// </summary>
    private void OnPause()
    {
        if (GameManager.Instance.IsPause == true)
        {
            GameManager.Instance.Resume();
        }
        else
        {
            GameManager.Instance.Pause();
        }
    }

    /// <summary>
    /// Camera Zoom
    /// </summary>
    private void OnZoom()
    {
        CameraMovement cameraMovement = FindObjectOfType<CameraMovement>();

        cameraMovement.IsMaxCamera = !cameraMovement.IsMaxCamera;
    }

    #endregion

    /// <summary>
    /// 이동 및 여러가지 메서드 
    /// </summary>
    private void Move()
    {
        if (moveDirection == Vector3.zero)
            return;

        transform.Translate(moveDirection * Time.deltaTime * moveSpeed);
        spriteRenderer.flipX = moveDirection.x < 0;
    }

    /// <summary>
    /// 땅에 닿았는지를 업데이트하는 메서드
    /// </summary>
    private void OnGrounded()
    {
        Vector2[] list = new Vector2[0];

        if (gravityType == GravityPortal.GravityPortalType.Up)
        {
            list = new Vector2[]
            {
                new Vector2(coll.bounds.min.x, coll.bounds.max.y),
                new Vector2(transform.position.x, coll.bounds.max.y),
                new Vector2(coll.bounds.max.x, coll.bounds.max.y),
            };
        }
        else if (gravityType == GravityPortal.GravityPortalType.Down)
        {
            list = new Vector2[]
            {
                new Vector2(coll.bounds.min.x, coll.bounds.min.y),
                new Vector2(transform.position.x, coll.bounds.min.y),
                new Vector2(coll.bounds.max.x, coll.bounds.min.y),
            };
        }

        foreach (var v in list)
        {
            RaycastHit2D hit = Physics2D.Raycast(v, -transform.up, checkGroundDistance, LayerMask.GetMask("Ground"));

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

    /// <summary>
    /// 애니메이터 설정 메서드
    /// </summary>
    private void SetAnimatorParameters()
    {
        animator.SetBool("IsMove", moveDirection.x != 0);
        animator.SetBool("IsGrounded", isGrounded);
        animator.SetFloat("Velocity", rigid.velocity.y);
    }

    /// <summary>
    /// 사망 이펙트 초기화 및 생성 메서드
    /// </summary>
    public void InstantiateDeadEffect()
    {
        GameObject.Instantiate(deadEffect, transform.position, Quaternion.identity);
    }

    /// <summary>
    /// 중력 타입을 지정하는 메서드
    /// </summary>
    /// <param name="type">중력 타입 지정</param>
    public void ChangeGravity(GravityPortal.GravityPortalType type)
    {
        gravityType = type;

        rigid.velocity = new Vector2(Math.Clamp(rigid.velocity.x, -9.8f, 9.8f),
                                     Math.Clamp(rigid.velocity.y, -9.8f, 9.8f));

        switch (type)
        {
            case GravityPortal.GravityPortalType.Up:

                rigid.gravityScale = -Math.Abs(rigid.gravityScale);
                transform.rotation = Quaternion.Euler(180.0f, 0.0f, 0.0f);

                break;
            case GravityPortal.GravityPortalType.Down:

                rigid.gravityScale = Math.Abs(rigid.gravityScale);
                transform.rotation = Quaternion.identity;

                break;
            default:
#if SHOW_DEBUG_MESSAGES
                Debug.LogWarning($"call with GravityPortalType null reference");
#endif
                break;
        }
    }
}