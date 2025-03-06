using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Photon.Pun;
using TwinTravelers.Core.Gimmicks;
using TwinTravelers.Core.Network;
using TwinTravelers.Management;
using TwinTravelers.Core.Utility;

namespace TwinTravelers.Core.Actor
{
    /// <summary>
    /// 플레이어 객체의 네트워크 사용 여부
    /// </summary>
    enum PlayerType
    {
        Online,
        Offline
    }

    /// <summary>
    /// 플레이어를 전체적으로 관리하는 클래스
    /// </summary>
    public class Player : MonoBehaviourPun, IPunObservable
    {
        #region Preset
        /// <summary>
        /// 현재 플레이어의 네트워크 상태
        /// </summary>
        [Header("Types")]
        [Tooltip("현재 플레이어의 네트워크 상태")]
        [SerializeField] 
        private PlayerType playerType;

        /// <summary>
        /// 플레이어가 움직일 속도
        /// </summary>
        [Header("Preset")]
        [Tooltip("플레이어가 움직일 속도")]
        [SerializeField] 
        private float moveSpeed;

        /// <summary>
        /// 플레이어의 점프 속도
        /// </summary>
        [Tooltip("플레이어가 점프 속도")]
        [SerializeField] 
        private float jumpVelocity;

        /// <summary>
        /// 플레이어의 점프 속도의 곱연산 값
        /// </summary>
        [Tooltip("플레이어가 점프 속도의 곱연산 값")]
        public float jumpMultiplier = 1.0f;

        /// <summary>
        /// 실제 사용하게 될 플레이어의 점프 속도
        /// </summary>
        private float JumpVelocity => jumpVelocity * jumpMultiplier;


        /// <summary>
        /// 플레이어가 사망 시, 재생 될 이펙트
        /// </summary>
        [Header("Effect")]
        [Tooltip("플레이어가 사망 시, 재생 될 이펙트")]
        [SerializeField] 
        private GameObject deadEffect;

        /// <summary>
        /// 플레이어의 풍선 컴포넌트
        /// </summary>
        [Header("Components")]
        [Tooltip("플레이어의 풍선 컴포넌트")]
        [SerializeField] 
        private PlayerBalloon playerBallon;
        public PlayerBalloon PlayerBalloon => playerBallon;
        #endregion

        #region Field
        /// <summary>
        /// 플레이어 오브젝트의 애니메이터 컴포넌트
        /// </summary>
        private Animator animator;

        /// <summary>
        /// 플레이어 오브젝트의 스프라이트 렌더러 컴포넌트
        /// </summary>
        private SpriteRenderer spriteRenderer;

        /// <summary>
        /// 플레이어 오브젝트의 리지드바디 컴포넌트
        /// </summary>
        private Rigidbody2D rigid;

        /// <summary>
        /// 플레이어 오브젝트의 콜라이더2D 컴포넌트
        /// </summary>
        private Collider2D coll;

        /// <summary>
        /// 움직일 방향
        /// </summary>
        private Vector3 moveDirection;

        /// <summary>
        /// Raycast로 땅을 체크할 거리
        /// </summary>
        private float checkGroundDistance = 0.1f;

        /// <summary>
        /// 땅에 닿아있는지 여부
        /// </summary>
        private bool isGrounded;
        
        /// <summary>
        /// 현재 플레이어의 중력 타입
        /// </summary>
        private GravityPortal.GravityPortalType gravityType;
        #endregion

        #region Network Field
        /// <summary>
        /// 동기화를 위한 네트워크상 위치
        /// </summary>
        private Vector3 networkPosition;

        /// <summary>
        /// 동기화를 위한 네트워크상 회전 값
        /// </summary>
        private Vector3 networkEuler;

        /// <summary>
        /// 동기화를 위한 네트워크상 가속도
        /// </summary>
        private Vector2 networkVelocity;
        #endregion

        #region Unity LifeCycle
        private void Awake()
        {
            animator = GetComponent<Animator>();
            spriteRenderer = GetComponent<SpriteRenderer>();
            rigid = GetComponent<Rigidbody2D>();
            coll = GetComponent<Collider2D>();

            switch (playerType)
            {
                case PlayerType.Offline:
                    GetComponent<PlayerInput>().SwitchCurrentControlScheme(new[] { Keyboard.current });
                    break;
                case PlayerType.Online:
#if UNITY_EDITOR
                    if (!PhotonNetwork.IsConnected)
                    {
                        Debug.Log("Client is Offline, but state is Online.");
                    }
                    if (!photonView.IsMine)
                    {
                        GetComponent<PlayerInput>().enabled = false;
                    }
#endif
                    break;
            }

            ChangeGravity(GravityPortal.GravityPortalType.Down);
        }

        private void Update()
        {
            switch (playerType)
            {
                case PlayerType.Offline:
                    SetAnimatorParameters();
                    Move();
                    OnGrounded();
                    break;
                case PlayerType.Online:
                    if (photonView.IsMine)
                    {
                        SetAnimatorParameters();
                        Move();
                        OnGrounded();
                    }
                    else
                    {
                        SmoothNetworkMovement();
                    }
                    break;
            }
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
        /// Player Input Send Message
        /// </summary>
        private void OnZoom()
        {
            CameraMovement cameraMovement = FindObjectOfType<CameraMovement>();

            cameraMovement.IsMaxCamera = !cameraMovement.IsMaxCamera;
        }

        #endregion

        #region PUN Network
        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (playerType == PlayerType.Offline)
            {
                return;
            }

            if (stream.IsWriting)
            {
                stream.SendNext(transform.position);
                stream.SendNext(transform.eulerAngles);
                stream.SendNext(GetComponent<Rigidbody2D>().velocity);

                stream.SendNext(animator.GetBool("IsMove"));
                stream.SendNext(animator.GetBool("IsGrounded"));

                stream.SendNext(gravityType);

                stream.SendNext(spriteRenderer.flipX);
            }
            else if (stream.IsReading)
            {
                networkPosition = (Vector3)stream.ReceiveNext();
                networkEuler = (Vector3)stream.ReceiveNext();
                networkVelocity = (Vector2)stream.ReceiveNext();

                bool isMove = (bool)stream.ReceiveNext();
                bool isGrounded = (bool)stream.ReceiveNext();

                GravityPortal.GravityPortalType gravityType = (GravityPortal.GravityPortalType)stream.ReceiveNext();

                spriteRenderer.flipX = (bool)stream.ReceiveNext();

                animator.SetBool("IsMove", isMove);
                animator.SetBool("IsGrounded", isGrounded);
                animator.SetFloat("Velocity", networkVelocity.y);

                if (this.gravityType != gravityType)
                {
                    ChangeGravity(gravityType);
                }
            }
        }

        private void SmoothNetworkMovement()
        {
            transform.eulerAngles = networkEuler;
            rigid.velocity = networkVelocity;

            Vector3 dir = (networkPosition - transform.position).normalized;
            float distance = Vector3.Distance(transform.position, networkPosition);
            float movePerFrame = moveSpeed * Time.deltaTime;

            if (distance < movePerFrame)
            {
                transform.position = networkPosition;
            }
            else
            {
                transform.position += dir * Mathf.Min(distance, movePerFrame);
            }
        }

        [PunRPC]
        private void SyncPosition(Vector3 targetPosition)
        {
            transform.position = targetPosition;
            networkPosition = targetPosition;
        }
        #endregion

        #region Methods
        /// <summary>
        /// 이동 처리 메서드
        /// </summary>
        private void Move()
        {
            if (moveDirection == Vector3.zero)
                return;

            transform.Translate(moveDirection * Time.deltaTime * moveSpeed);
            spriteRenderer.flipX = moveDirection.x < 0;
        }

        /// <summary>
        /// 땅에 닿아있는지를 확인하는 메서드
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
        /// 순간이동 로직
        /// <summary>
        public void MoveToPosition(Vector3 targetPosition)
        {
            switch (RoomManager.Instance.playmode)
            {
                case RoomManager.Playmode.Multi:
                    if (photonView.IsMine)
                    {
                        transform.position = targetPosition;

                        photonView.RPC("SyncPosition", RpcTarget.Others, targetPosition);
                    }
                    break;

                case RoomManager.Playmode.Single:
                    transform.position = targetPosition;
                    break;
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
        /// <param name="type">전환할 중력 타입</param>
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
#if UNITY_EDITOR
                    Debug.LogWarning($"call with GravityPortalType null reference");
#endif
                    break;
            }
        }
        #endregion
    }
}