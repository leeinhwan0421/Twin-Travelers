using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Press : Obstacle
{
    [Header("Preset")]
    [SerializeField] private float minY;
    [SerializeField] private float maxY;
    [SerializeField] private float cycleTime;
    [SerializeField] private float pressSpeed = 10f;
    [SerializeField] private float returnSpeed = 2f;

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

    private IEnumerator PressCycle()
    {
        while (true)
        {
            yield return MoveToPosition(maxY, returnSpeed);

            yield return new WaitForSecondsRealtime(cycleTime / 2);

            yield return MoveToPosition(minY, pressSpeed);

            yield return new WaitForSecondsRealtime(cycleTime / 2);
        }
    }

    private IEnumerator MoveToPosition(float targetY, float speed)
    {
        Vector3 targetPosition = new Vector3(transform.position.x, targetY, transform.position.z);

        while (Mathf.Abs(transform.position.y - targetY) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.unscaledDeltaTime);
            yield return null;
        }

        transform.position = targetPosition;
    }
}
