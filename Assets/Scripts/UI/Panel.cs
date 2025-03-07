using UnityEngine;

namespace TwinTravelers.UI
{
    /// <summary>
    /// 패널
    /// </summary>
    public class Panel : MonoBehaviour
    {
        #region Fields
        /// <summary>
        /// 애니메이터
        /// </summary>
        private Animator animator;
        #endregion

        #region Unity Methods
        private void Awake()
        {
            if (!TryGetComponent<Animator>(out animator))
            {
#if UNITY_EDITOR
                Debug.Log($"{gameObject.name} object don't have animator.");
#endif
            }
        }
        #endregion

        #region
        /// <summary>
        /// 패널 활성화 시, 애니메이션 재생
        /// </summary>
        public void Enable()
        {
            SetEnable();
            animator.ResetTrigger("Disable");
            animator.SetTrigger("Enable");
        }

        /// <summary>
        /// 패널 비활성화 시, 애니메이션 재생
        /// </summary>
        public void Disable()
        {
            animator.ResetTrigger("Enable");
            animator.SetTrigger("Disable");
        }

        /// <summary>
        /// 활성화 설정
        /// </summary>
        private void SetEnable() => gameObject.SetActive(true);

        /// <summary>
        /// 비활성화 설정
        /// </summary>
        private void SetDisable() => gameObject.SetActive(false);
        #endregion
    }
}
