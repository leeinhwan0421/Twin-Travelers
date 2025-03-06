using UnityEngine;
using TwinTravelers.Core.Utility;
using TwinTravelers.Management;

namespace TwinTravelers.Core.Gimmicks
{
    public class Lever_Door : InteractableTrigger
    {
        // private properties...
        private Animator animator;
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

        public void ResetLeverDoor()
        {
            animator.Rebind();
            isActivate = false;
        }
    }
}
