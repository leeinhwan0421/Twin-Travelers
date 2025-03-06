using System.Collections;
using UnityEngine;

namespace TwinTravelers.Core.Gimmicks
{
    public class Laser : Obstacle
    {
        [Header("Presets")]
        [SerializeField] private bool isStatic;
        [Space(10.0f)]
        [Range(0.01f, 10.0f)][SerializeField] private float activeTime;
        [Range(0.01f, 10.0f)][SerializeField] private float deactiveTime;

        private LineRenderer lineRenderer;
        private EdgeCollider2D coll;

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

        private void Activate()
        {
            // AudioManager.Instance.PlaySFX("Laser");
            StartCoroutine(WidthAnimation(1.0f));
            coll.enabled = true;
        }

        private void Deactivate()
        {
            StartCoroutine(WidthAnimation(0.0f));
            coll.enabled = false;
        }

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
    }
}
