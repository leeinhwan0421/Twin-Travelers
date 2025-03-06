using UnityEngine.Rendering.Universal;
using UnityEngine;

namespace TwinTravelers.Core.Utility
{
    public class SpotLightRadiusController : MonoBehaviour
    {
        #region Field
        /// <summary>
        /// 내부 반지름 최소값
        /// </summary>
        [Header("Presets")]
        [Tooltip("내부 반지름 최소값")]
        [SerializeField] 
        private float minInner;

        /// <summary>
        /// 내부 반지름 최대값
        /// </summary>
        [Tooltip("내부 반지름 최대값")]
        [SerializeField]
        private float maxInner;

        /// <summary>
        /// 외부 반지름 최소값
        /// </summary>
        [Tooltip("외부 반지름 최소값")]
        [SerializeField, Space(10.0f)]
        private float minOuter;

        /// <summary>
        /// 외부 반지름 최대값
        /// </summary>
        [Tooltip("외부 반지름 최대값")]
        [SerializeField]
        private float maxOuter;

        /// <summary>
        /// 반지름 변경 주기
        /// </summary>
        [Tooltip("반지름 변경 주기")]
        [SerializeField, Space(10.0f)]
        private float time = 1.0f;

        /// <summary>
        /// 2D 라이트 컴포넌트
        /// </summary>
        private Light2D light2D;

        /// <summary>
        /// 타이머
        /// </summary>
        private float timer = 0.0f;
        #endregion

        private void Awake()
        {
            light2D = GetComponent<Light2D>();
        }

        private void Update()
        {
            timer += Time.deltaTime;

            if (time <= timer)
            {
                ChangeRadius();
                timer = 0.0f;
            }
        }

        private void ChangeRadius()
        {
            light2D.pointLightInnerRadius = Random.Range(minInner, maxInner);
            light2D.pointLightOuterRadius = Random.Range(minOuter, maxOuter);
        }
    }
}
