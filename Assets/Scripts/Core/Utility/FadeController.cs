using System.Collections;
using UnityEngine;

namespace TwinTravelers.Core.Utility
{
    /// <summary>
    /// Fade In / Out 효과를 주는 클래스
    /// </summary>
    public class FadeController : MonoBehaviour
    {
        #region Field
        /// <summary>
        /// 최대 알파값
        /// </summary>
        [Header("Preset")]
        [SerializeField, Range(0f, 1f)] 
        private float maxAlpha;

        /// <summary>
        /// 최소 알파값
        /// </summary>
        [SerializeField, Range(0f, 1f)] 
        private float minAlpha;

        /// <summary>
        /// 페이드 인/아웃 되는 속도
        /// </summary>
        [SerializeField, Range(0.01f, 3f)] 
        private float fadeSpeed;

        /// <summary>
        /// 페이드 인 / 아웃 간격
        /// </summary>
        [SerializeField] 
        private float fadeInterval;

        /// <summary>
        /// 클래스를 가지고 있는 오브젝트의 스프라이트 렌더러 컴포넌트
        /// </summary>
        private SpriteRenderer spriteRenderer;
        #endregion

        private void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        private void Start()
        {
            StartCoroutine(FadeCoroutine());
        }

        /// <summary>
        /// 페이드 루프 코루틴
        /// </summary>
        /// <returns>IEnumerator</returns>
        private IEnumerator FadeCoroutine()
        {
            while (true)
            {
                yield return ChangeAlphaOverTime(maxAlpha, fadeSpeed);

                yield return new WaitForSeconds(fadeInterval / 2);

                yield return ChangeAlphaOverTime(minAlpha, fadeSpeed);

                yield return new WaitForSeconds(fadeInterval / 2);
            }
        }

        /// <summary>
        /// 알파값을 서서히 변환시키는 코루틴
        /// </summary>
        /// <returns>IEnumerator</returns>
        private IEnumerator ChangeAlphaOverTime(float target, float lerpSpeed)
        {
            while (Mathf.Abs(spriteRenderer.color.a - target) > 0.01f)
            {
                float newAlpha = Mathf.Lerp(spriteRenderer.color.a, target, lerpSpeed * Time.deltaTime);

                spriteRenderer.color = new Color(spriteRenderer.color.r,
                                                 spriteRenderer.color.g,
                                                 spriteRenderer.color.b,
                                                 newAlpha);

                yield return null;
            }

            spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, target);
        }
    }
}
