using System.Collections.Generic;
using UnityEngine;
using TwinTravelers.Management;

namespace TwinTravelers.Core.Gimmicks
{
    /// <summary>
    /// 플레이어가 가까이 와야 보이는 클래스
    /// </summary>
    public class Stealth : MonoBehaviour
    {
        #region Fields
        /// <summary>
        /// 최소 가시 거리
        /// </summary>
        [Header("Presets")]
        [Tooltip("최소 가시 거리")]
        [Range(0.01f, 10.0f), SerializeField] 
        private float minRange;

        /// <summary>
        /// 최대 가시 거리
        /// </summary>
        [Tooltip("최대 가시 거리")]
        [Range(0.01f, 100.0f), SerializeField]
        private float maxRange;

        /// <summary>
        /// 스프라이트 렌더러 컴포넌트
        /// </summary>
        private SpriteRenderer spriteRenderer;
        #endregion

        #region Unity Methods
        private void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        private void Update()
        {
            List<GameObject> playerObjects = GameManager.Instance.Player;

            if (playerObjects == null || playerObjects.Count == 0)
                return;

            float minDistance = 100.0f;

            // 플레이어와의 최소 거리를 계산
            foreach (var playerObject in playerObjects)
            {
                float distance = Vector2.Distance(transform.position, playerObject.transform.position);

                if (minDistance > distance)
                {
                    minDistance = distance;
                }
            }

            if (minDistance <= minRange)
            {
                SetAlpha(1.0f);
            }
            else if (minDistance > minRange && minDistance <= maxRange)
            {
                float t = (minDistance - minRange) / (maxRange - minRange);
                float alpha = Mathf.Lerp(1.0f, 0.0f, t);
                SetAlpha(alpha);
            }
            else
            {
                SetAlpha(0.0f);
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// 알파값을 설정합니다.
        /// </summary>
        /// <param name="alpha">변경할 값</param>
        private void SetAlpha(float alpha)
        {
            Color color = spriteRenderer.color;
            color.a = alpha;
            spriteRenderer.color = color;
        }
        #endregion

        #region Gizmos
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;

            DrawWireCircle(transform.position, minRange);
            DrawWireCircle(transform.position, maxRange);
        }

        void DrawWireCircle(Vector3 center, float radius)
        {
            int segments = 36;
            float angle = 0f;

            Vector3 prevPoint = center + new Vector3(Mathf.Cos(angle) * radius, Mathf.Sin(angle) * radius, 0);

            for (int i = 1; i <= segments; i++)
            {
                angle += 2 * Mathf.PI / segments;
                Vector3 newPoint = center + new Vector3(Mathf.Cos(angle) * radius, Mathf.Sin(angle) * radius, 0);
                Gizmos.DrawLine(prevPoint, newPoint);
                prevPoint = newPoint;
            }
        }
        #endregion
    }
}
