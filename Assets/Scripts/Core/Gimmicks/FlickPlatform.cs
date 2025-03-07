using System.Collections;
using UnityEngine;

namespace TwinTravelers.Core.Gimmicks
{
    /// <summary>
    /// 깜빡이는 플랫폼 클래스
    /// </summary>
    public class FlashPlatform : MonoBehaviour
    {
        #region Fields
        /// <summary>
        /// 활성화 시간
        /// </summary>
        [Header("Presets")]
        [Tooltip("활성화 시간")]
        [Range(1.0f, 10.0f), SerializeField]
        private float activeTime;

        /// <summary>
        /// 비활성화 시간
        /// </summary>
        [Tooltip("비활성화 시간")]
        [Range(1.0f, 10.0f), SerializeField]
        private float deactiveTime;

        /// <summary>
        /// 제어할 스프라이트 렌더러
        /// </summary>
        private SpriteRenderer spriteRenderer;

        /// <summary>
        /// 제어할 콜라이더
        /// </summary>
        private Collider2D coll;
        #endregion

        #region Unity Methods
        private void Start()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            coll = GetComponent<Collider2D>();

            StartCoroutine(Cycle());
        }
        #endregion

        #region Methods
        /// <summary>
        /// 활성화 상태로 전환합니다.
        /// </summary>
        private void Activate()
        {
            coll.enabled = true;
            spriteRenderer.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        }

        /// <summary>
        /// 비활성화 상태로 전환합니다.
        /// </summary>
        private void Deactivate()
        {
            coll.enabled = false;
            spriteRenderer.color = new Color(1.0f, 1.0f, 1.0f, 0.2f);
        }

        /// <summary>
        /// 활성화/비활성화 주기를 관리합니다.
        /// </summary>
        /// <returns>IEnumerator</returns>
        private IEnumerator Cycle()
        {
            while (true)
            {
                yield return new WaitForSeconds(activeTime);

                Deactivate();

                yield return new WaitForSeconds(deactiveTime);

                Activate();
            }
        }
        #endregion
    }
}
