using UnityEngine;
using TwinTravelers.Core.Utility;
using TwinTravelers.Management;

namespace TwinTravelers.Core.Gimmicks
{
    /// <summary>
    /// 두명 이상이 충돌 중일때, 사라지는 플랫폼
    /// </summary>
    public class TrapPlatform : InteractableCollision
    {
        #region Fields
        /// <summary>
        /// 부셔지는 이펙트
        /// </summary>
        [Header("Presets")]
        [Tooltip("부셔지는 이펙트")]
        [SerializeField] 
        private GameObject effect;

        /// <summary>
        /// 현재 플랫폼과 접촉중인 플레이어 수
        /// </summary>
        private int count = 0;
        #endregion

        #region Methods
        /// <summary>
        /// 플랫폼을 초기화합니다.
        /// </summary>
        public void ResetTrapPlatform()
        {
            count = 0;
            gameObject.SetActive(true);
        }

        protected override void EnterEvent(Collision2D collision)
        {
            count++;

            if (count >= 2)
            {
                gameObject.SetActive(false);
                AudioManager.Instance.PlaySFX("BreakWood");
                Instantiate(effect, transform.position, Quaternion.identity);
            }
        }

        protected override void ExitEvent(Collision2D collision)
        {
            count--;
        }
        #endregion
    }
}
