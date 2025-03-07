using UnityEngine;

namespace TwinTravelers.Core.Gimmicks
{
    /// <summary>
    /// 웨이포인트 사이를 이동하는 클래스
    /// </summary>
    public class MoveBetweenWaypoints : MonoBehaviour
    {
        #region Fields
        /// <summary>
        /// 웨이포인트 배열
        /// </summary>
        [Header("WayPoints")]
        [Tooltip("웨이포인트 배열")]
        [SerializeField] 
        private Transform[] waypoints;

        /// <summary>
        /// 이동 속도
        /// </summary>
        [Header("Presets")]
        [Tooltip("이동 속도")]
        [Range(1.0f, 10.0f), SerializeField] 
        private float moveSpeed;

        /// <summary>
        /// 현재 웨이포인트 인덱스
        /// </summary>
        private int curIndex;
        #endregion

        #region Unity Methods
        private void OnDrawGizmos()
        {
            if (waypoints == null)
            {
                return;
            }

            if (waypoints.Length <= 1)
            {
                return;
            }

            Gizmos.color = Color.cyan;

            for (int i = 0; i < waypoints.Length - 1; i++)
            {
                Gizmos.DrawLine(waypoints[i].position, waypoints[i + 1].position);
            }

            Gizmos.DrawLine(waypoints[waypoints.Length - 1].position, waypoints[0].position);
        }

        private void Update()
        {
            Move();
        }
        #endregion

        #region Methods
        private void Move()
        {
            if (Vector2.Distance(transform.position, waypoints[curIndex].position) < moveSpeed * Time.deltaTime)
            {
                transform.position = waypoints[curIndex].position;
                curIndex = (curIndex + 1) % waypoints.Length;
            }

            transform.position = Vector3.MoveTowards(transform.position, waypoints[curIndex].position, moveSpeed * Time.deltaTime);
        }
        #endregion
    }
}
