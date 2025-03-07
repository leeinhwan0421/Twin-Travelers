using System.Collections;
using UnityEngine;

namespace TwinTravelers.Core.Gimmicks
{
    /// <summary>
    /// 적 클래스
    /// </summary>
    public class Enemy : MonoBehaviour
    {
        #region Fields
        /// <summary>
        /// 적의 이동 속도
        /// </summary>
        [Header("Presets")]
        [Tooltip("적의 이동 속도")]
        [SerializeField] 
        private float lerpSpeed;

        /// <summary>
        /// 적의 이동 주기
        /// </summary>
        [Tooltip("적의 이동 주기")]
        [SerializeField]
        private float cycleTime;

        /// <summary>
        /// 적의 최대 이동 위치 
        /// </summary>
        [Tooltip("적의 최대 이동 위치 ")]
        [SerializeField]
        private Vector2 max;

        /// <summary>
        /// 적의 최소 이동 위치
        /// </summary>
        [Tooltip("적의 최소 이동 위치")]
        [SerializeField]
        private Vector2 min;

        private Vector2 drawFrom = Vector2.zero;
        private Vector2 drawTo = Vector2.zero;

        /// <summary>
        /// 애니메이터 컴포넌트
        /// </summary>
        private Animator animator;
        #endregion

        #region Unity Methods
        private void Start()
        {
            animator = GetComponent<Animator>();

            min += (Vector2)transform.position;
            max += (Vector2)transform.position;

            drawFrom = min;
            drawTo = max;

            StartCoroutine(MoveCycle());
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;

            if (drawFrom == Vector2.zero && drawTo == Vector2.zero)
            {
                Gizmos.DrawLine((Vector2)transform.position + min, (Vector2)transform.position + max);
            }

            Gizmos.DrawLine(drawFrom, drawTo);
        }
        #endregion

        #region Methods
        /// <summary>
        /// 적의 이동을 주기적으로 반복하는 코루틴
        /// </summary>
        /// <returns>IEnumerator</returns>
        private IEnumerator MoveCycle()
        {
            yield return new WaitForSeconds(2.0f);

            while (true)
            {
                yield return MoveToPosition(max, lerpSpeed);

                yield return new WaitForSeconds(cycleTime / 2);

                yield return MoveToPosition(min, lerpSpeed);

                yield return new WaitForSeconds(cycleTime / 2);
            }
        }

        /// <summary>
        /// 적을 특정 위치로 이동시키는 코루틴
        /// </summary>
        /// <param name="position">목표 위치</param>
        /// <param name="speed">속도</param>
        /// <returns>IEnumerator</returns>
        private IEnumerator MoveToPosition(Vector2 position, float speed)
        {
            Vector2 s = transform.position;
            Vector2 e = position;

            if (s == e)
            {
                yield break;
            }

            float distance = Vector2.Distance(s, e);
            float time = distance / speed;
            float timer = 0.0f;

            if ((e - s).normalized.x == -1)
            {
                transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, 180f, transform.localEulerAngles.z);
            }
            else if ((e - s).normalized.x == 1)
            {
                transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, 0f, transform.localEulerAngles.z);
            }

            animator.SetBool("IsMove", true);

            while (timer <= time)
            {
                transform.position = Vector2.Lerp(s, e, timer / time);
                timer += Time.deltaTime;

                yield return null;
            }

            animator.SetBool("IsMove", false);

            transform.position = e;
        }
        #endregion
    }
}
