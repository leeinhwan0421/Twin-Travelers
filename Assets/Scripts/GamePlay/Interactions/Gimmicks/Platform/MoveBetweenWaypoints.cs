using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBetweenWaypoints : MonoBehaviour
{
    [Header("WayPoints")]
    [SerializeField] private Transform[] waypoints;

    [Header("Presets")]
    [Range(1.0f, 10.0f)] [SerializeField] private float moveSpeed;

    private int curIndex;

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

    private void Move()
    {
        if (Vector2.Distance(transform.position, waypoints[curIndex].position) < moveSpeed * Time.unscaledDeltaTime)
        {
            transform.position = waypoints[curIndex].position;
            curIndex = (curIndex + 1) % waypoints.Length;
        }

        transform.position = Vector3.MoveTowards(transform.position, waypoints[curIndex].position, moveSpeed * Time.unscaledDeltaTime);
    }
}
