using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [Header("MapBounds")]
    [SerializeField] private Vector2 min;
    [SerializeField] private Vector2 max;

    [Header("Preset")]
    [SerializeField] private float minSize;
    [SerializeField] private float maxSize;
    [SerializeField] private float smoothTime;

    [Range(1.0f, 3.0f)]
    [SerializeField] private float sizePreset;

    private Camera cam;
    private Vector3 velocity = Vector3.zero;
    private float zoomVelocity = 0f;

    private void Awake()
    {
        cam = GetComponent<Camera>();
    }

    private void OnDrawGizmos()
    {
        // p1 --- p2
        // |       |
        // p4 --- p3

        Vector2 p1 = new Vector2(min.x, max.y);
        Vector2 p2 = new Vector2(max.x, max.y);
        Vector2 p3 = new Vector2(max.x, min.y);
        Vector2 p4 = new Vector2(min.x, min.y);

        Gizmos.color = Color.blue;

        Gizmos.DrawLine(p1, p2);
        Gizmos.DrawLine(p2, p3);
        Gizmos.DrawLine(p3, p4);
        Gizmos.DrawLine(p4, p1);
    }

    private void Start()
    {
        if (cam.orthographic == false)
        {
#if SHOW_DEBUG_MESSAGES
            Debug.Log("This Scripts only support Orthographic Camera");
#endif
        }

        SetMaxSize();
    }

    private void SetMaxSize()
    {
        float mapWidth = max.x - min.x;
        float mapHeight = max.y - min.y;

        float maxCameraSizeX = mapWidth / (2f * cam.aspect);
        float maxCameraSizeY = mapHeight / 2f;

        maxSize = Mathf.Min(maxCameraSizeX, maxCameraSizeY);
    }

    private void LateUpdate()
    {
        SetPositionAndZoom();
    }

    private void SetPositionAndZoom()
    {
        List<GameObject> playerObjects = GameManager.Instance.Player;

        if (playerObjects == null || playerObjects.Count == 0)
            return;

        Vector3 center = GetCenterPoint(playerObjects);

        Vector3 newPosition = new Vector3(center.x, center.y, transform.position.z);
        transform.position = Vector3.SmoothDamp(cam.transform.position, newPosition, ref velocity, smoothTime);

        float newSize = GetCameraSize(playerObjects);
        cam.orthographicSize = Mathf.SmoothDamp(cam.orthographicSize, newSize, ref zoomVelocity, smoothTime);

        transform.position = ClampCameraPosition(transform.position);
    }

    private Vector3 GetCenterPoint(List<GameObject> players)
    {
        var bounds = new Bounds(players[0].transform.position, Vector3.zero);

        for (int i = 1; i < players.Count; i++)
        {
            bounds.Encapsulate(players[i].transform.position);
        }

        return bounds.center;
    }

    private float GetCameraSize(List<GameObject> players)
    {
        var bounds = new Bounds(players[0].transform.position, Vector3.zero);

        for (int i = 1; i < players.Count; i++)
        {
            bounds.Encapsulate(players[i].transform.position);
        }

        float sizeX = bounds.size.x / sizePreset / cam.aspect;
        float sizeY = bounds.size.y / sizePreset;
        float size = Mathf.Max(sizeX, sizeY);

        size = Mathf.Clamp(size, minSize, maxSize);

        return size;
    }

    private Vector3 ClampCameraPosition(Vector3 targetPosition)
    {
        float cameraHeight = cam.orthographicSize;
        float cameraWidth = cam.orthographicSize * cam.aspect;

        float minX = min.x + cameraWidth;
        float maxX = max.x - cameraWidth;
        float minY = min.y + cameraHeight;
        float maxY = max.y - cameraHeight;

        return new Vector3(Mathf.Clamp(targetPosition.x, minX, maxX),
                           Mathf.Clamp(targetPosition.y, minY, maxY),
                           targetPosition.z);
    }
}
