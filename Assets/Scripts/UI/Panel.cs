using UnityEngine;

namespace TwinTravelers.UI
{
    public class Panel : MonoBehaviour
    {
        private Animator animator;

        private void Awake()
        {
            if (!TryGetComponent<Animator>(out animator))
            {
#if UNITY_EDITOR
                Debug.Log($"{gameObject.name} object don't have animator.");
#endif
            }
        }

        public void Enable()
        {
            SetEnable();
            animator.ResetTrigger("Disable");
            animator.SetTrigger("Enable");
        }

        public void Disable()
        {
            animator.ResetTrigger("Enable");
            animator.SetTrigger("Disable");
        }

        private void SetEnable() => gameObject.SetActive(true);

        private void SetDisable() => gameObject.SetActive(false);
    }
}
