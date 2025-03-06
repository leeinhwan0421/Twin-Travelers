using System.Collections.Generic;
using UnityEngine;
using TwinTravelers.Management;

namespace TwinTravelers.Core.Gimmicks
{
    public class Stealth : MonoBehaviour
    {
        [Header("Presets")]
        [Range(0.01f, 10.0f)][SerializeField] private float minRange;
        [Range(0.01f, 100.0f)][SerializeField] private float maxRange;

        private SpriteRenderer spriteRenderer;

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

        private void SetAlpha(float alpha)
        {
            Color color = spriteRenderer.color;
            color.a = alpha;
            spriteRenderer.color = color;
        }

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
