using UnityEngine;

namespace TwinTravelers.Core.Utility
{
    public class Effect : MonoBehaviour
    {
        private Animator animator;

        private void Awake()
        {
            animator = GetComponent<Animator>();
        }

        private void Start()
        {
            float time = animator.GetCurrentAnimatorStateInfo(0).length;
            Destroy(gameObject, time);
        }
    }
}
