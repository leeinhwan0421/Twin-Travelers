using UnityEngine;
using TwinTravelers.Core.Utility;
using TwinTravelers.Management;

namespace TwinTravelers.Core.Gimmicks
{
    /// <summary>
    /// 레버를 당겼을 때 문을 열고 닫는 클래스
    /// </summary>
    public class Lever_Door : InteractableTrigger
    {
        /// <summary>
        /// 애니메이터 컴포넌트
        /// </summary>
        private Animator animator;

        /// <summary>
        /// 레버가 활성화되었는지 확인하는 변수
        /// </summary>
        private bool isActivate = false;

        private void Awake()
        {
            animator = GetComponent<Animator>();
        }

        protected override void EnterEvent(Collider2D collision)
        {
            if (isActivate)
                return;

            isActivate = true;

            AudioManager.Instance.PlaySFX("Lever");
            animator.SetTrigger("Active");
        }

        protected override void ExitEvent(Collider2D collision)
        {
            // Nothing
        }

        /// <summary>
        /// 문을 초기 상태로 되돌립니다.
        /// </summary>
        public void ResetLeverDoor()
        {
            animator.Rebind();
            isActivate = false;
        }
    }
}
