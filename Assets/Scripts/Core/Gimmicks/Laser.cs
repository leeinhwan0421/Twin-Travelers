using System.Collections;
using UnityEngine;

namespace TwinTravelers.Core.Gimmicks
{
    /// <summary>
    /// 레이저 클래스
    /// </summary>
    public class Laser : Obstacle
    {
        #region Fields
        /// <summary>
        /// 레이져가 계속 켜져있어야 하는지 여부
        /// </summary>
        [Header("Presets")]
        [Tooltip("레이져가 계속 켜져있어야 하는지 여부")]
        [SerializeField]
        private bool isStatic;

        [Space(10.0f)]

        /// <summary>
        /// 활성화 시간
        /// </summary>
        [Tooltip("활성화 시간")]
        [Range(0.01f, 10.0f), SerializeField] 
        private float activeTime;

        /// <summary>
        /// 비활성화 시간
        /// </summary>
        [Tooltip("비활성화 시간")]
        [Range(0.01f, 10.0f), SerializeField] 
        private float deactiveTime;

        /// <summary>
        /// 이 클래스를 가진 오브젝트의 라인 렌더러 컴포넌트
        /// </summary>
        private LineRenderer lineRenderer;

        /// <summary>
        /// 이 클래스를 가진 오브젝트의 엣지 콜라이더 컴포넌트
        /// </summary>
        private EdgeCollider2D coll;
        #endregion

        #region Unity Methods
        private void Awake()
        {
            lineRenderer = GetComponent<LineRenderer>();
            coll = GetComponent<EdgeCollider2D>();

            GenerateEdgeCollider2D();
            StartCoroutine(Cycle());
        }

        private void OnDisable()
        {
            StopAllCoroutines();
        }
        #endregion

        #region Methods
        /// <summary>
        /// 엣지 콜라이더 생성
        /// </summary>
        private void GenerateEdgeCollider2D()
        {
            int numPoints = lineRenderer.positionCount;
            Vector3[] positions = new Vector3[numPoints];
            lineRenderer.GetPositions(positions);

            Vector2[] colliderPoints = new Vector2[numPoints];
            for (int i = 0; i < numPoints; i++)
            {
                colliderPoints[i] = new Vector2(positions[i].x, positions[i].y);
            }

            coll.points = colliderPoints;
            coll.isTrigger = true;
        }

        /// <summary>
        /// 활성화
        /// </summary>
        private void Activate()
        {
            // AudioManager.Instance.PlaySFX("Laser");
            StartCoroutine(WidthAnimation(1.0f));
            coll.enabled = true;
        }

        /// <summary>
        /// 비활성화
        /// </summary>
        private void Deactivate()
        {
            StartCoroutine(WidthAnimation(0.0f));
            coll.enabled = false;
        }

        /// <summary>
        /// 레이저 사이클
        /// </summary>
        /// <returns>IEnumerator</returns>
        private IEnumerator Cycle()
        {
            if (isStatic == true)
            {
                yield break;
            }

            while (true)
            {
                yield return new WaitForSeconds(activeTime);

                Deactivate();

                yield return new WaitForSeconds(deactiveTime);

                Activate();
            }
        }

        /// <summary>
        /// 레이저가 부드럽게 켜지거나 꺼지게 하는 코루틴
        /// </summary>
        /// <param name="widthMultipler">목표 너비 multipler</param>
        /// <returns>IEnumerator</returns>
        private IEnumerator WidthAnimation(float widthMultipler)
        {
            float speed = 5.0f;
            float timer = 0.0f;

            float s = lineRenderer.widthMultiplier;
            float e = widthMultipler;

            while (timer <= 1.0f)
            {
                lineRenderer.widthMultiplier = Mathf.Lerp(s, e, timer);
                timer += Time.deltaTime * speed;

                yield return null;
            }

            lineRenderer.widthMultiplier = e;

            yield break;
        }
        #endregion
    }
}
