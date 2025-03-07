using System.Collections;
using UnityEngine;

namespace TwinTravelers.Core.Gimmicks
{
    /// <summary>
    /// 프레스 장애물 클래스
    /// </summary>
    public class Press : Obstacle
    {
        #region Fields
        /// <summary>
        /// 최소 높이
        /// </summary>
        [Header("Preset")]
        [Tooltip("최소 높이")]
        [SerializeField] 
        private float minY;

        /// <summary>
        /// 최대 높이
        /// </summary>
        [Tooltip("최대 높이")]
        [SerializeField]
        private float maxY;

        /// <summary>
        /// 주기
        /// </summary>
        [Tooltip("주기")]
        [SerializeField]
        private float cycleTime;

        /// <summary>
        /// 눌릴 때의 속도
        /// </summary>
        [Tooltip("눌릴 때의 속도")]
        [SerializeField]
        private float pressSpeed = 10f;

        /// <summary>
        /// 복귀 속도
        /// </summary>
        [Tooltip("복귀 속도")]
        [SerializeField]
        private float returnSpeed = 2f;
        #endregion

        #region Unity Methods
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;

            Vector2 from = new Vector2(transform.position.x, transform.position.y + minY);
            Vector2 to = new Vector2(transform.position.x, transform.position.y + maxY);

            Gizmos.DrawLine(from, to);
        }

        private void Start()
        {
            minY += transform.position.y;
            maxY += transform.position.y;

            StartCoroutine(PressCycle());
        }
        #endregion

        #region Methods
        /// <summary>
        /// 프레스 사이클
        /// </summary>
        /// <returns>IEnumerator</returns>
        private IEnumerator PressCycle()
        {
            while (true)
            {
                yield return MoveToPosition(maxY, returnSpeed);

                yield return new WaitForSeconds(cycleTime / 2);

                yield return MoveToPosition(minY, pressSpeed);

                yield return new WaitForSeconds(cycleTime / 2);
            }
        }

        /// <summary>
        /// 특정 위치로 이동합니다.
        /// </summary>
        /// <param name="targetY">목표 높이</param>
        /// <param name="speed">속도</param>
        /// <returns>IEnumerator</returns>
        private IEnumerator MoveToPosition(float targetY, float speed)
        {
            Vector3 targetPosition = new Vector3(transform.position.x, targetY, transform.position.z);

            while (Mathf.Abs(transform.position.y - targetY) > 0.01f)
            {
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
                yield return null;
            }

            transform.position = targetPosition;
        }
        #endregion
    }
}
