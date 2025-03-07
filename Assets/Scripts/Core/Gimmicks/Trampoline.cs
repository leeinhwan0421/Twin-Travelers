using UnityEngine;
using TwinTravelers.Core.Utility;
using TwinTravelers.Management;

namespace TwinTravelers.Core.Gimmicks
{
    /// <summary>
    /// 트리거에 충돌 시, 플레이어를 튕기는 클래스
    /// </summary>
    public class Trampoline : InteractableCollision
    {
        #region Fields
        /// <summary>
        /// 튕길 속도
        /// </summary>
        [Header("Presets")]
        [Tooltip("튕길 속도")]
        [SerializeField] 
        private Vector2 velocity;

        /// <summary>
        /// 애니메이터 컴포넌트
        /// </summary>
        private Animator anim;
        #endregion

        #region Unity methods
        private void Awake()
        {
            anim = GetComponent<Animator>();
        }
        #endregion

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
}