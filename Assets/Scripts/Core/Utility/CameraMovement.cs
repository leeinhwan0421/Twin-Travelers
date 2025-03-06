using System.Collections.Generic;
using UnityEngine;
using TwinTravelers.Management;

namespace TwinTravelers.Core.Utility
{
    /// <summary>
    /// 카메라 이동을 정의하는 클래스
    /// </summary>
    public class CameraMovement : MonoBehaviour
    {
        #region Field
        /// <summary>
        /// 최소 카메라 경계
        /// </summary>
        [Header("MapBounds")]
        [Tooltip("최소 카메라 경계")]
        [SerializeField] 
        private Vector2 min;

        /// <summary>
        /// 최대 카메라 경계
        /// </summary>
        [Tooltip("최대 카메라 경계")]
        [SerializeField]
        private Vector2 max;

        /// <summary>
        /// 카메라의 정투영 최소 사이즈
        /// </summary>
        [Header("Preset")]
        [Tooltip("카메라의 정투영 최소 사이즈")]
        [SerializeField]
        private float minSize;

        /// <summary>
        /// 카메라의 정투영 최대 사이즈
        /// </summary>
        [Tooltip("카메라의 정투영 최대 사이즈")]
        [SerializeField]
        private float maxSize;

        [Tooltip("카메라 이동 반영 속도")]
        [SerializeField]
        private float smoothTime;

        /// <summary>
        /// 설정된 카메라의 사이즈 곱연산 변수
        /// </summary>
        [Tooltip("설정된 카메라의 사이즈 곱연산 변수")]
        [SerializeField, Range(1.0f, 3.0f)]
        private float sizePreset;

        /// <summary>
        /// 축소 시 false 됩니다.
        /// </summary>
        private bool isMaxCamera = false;
        public bool IsMaxCamera
        {
            get
            {
                return isMaxCamera;
            }
            set
            {
                isMaxCamera = value;
            }
        }

        /// <summary>
        /// 카메라 컴포넌트
        /// </summary>
        private Camera cam;

        /// <summary>
        /// 속도 저장용 변수
        /// </summary>
        private Vector3 velocity = Vector3.zero;

        /// <summary>
        /// 줌 속도 저장용 변수
        /// </summary>
        private float zoomVelocity = 0f;
        #endregion

        #region Unity Methods
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
#if UNITY_EDITOR
                Debug.Log("This Scripts only support Orthographic Camera");
#endif
            }

            SetMaxSize();
        }

        private void LateUpdate()
        {
            SetPositionAndZoom();
        }
        #endregion

        #region Methods
        /// <summary>
        /// 카메라의 최대 사이즈를 자동으로 설정합니다.
        /// </summary>
        private void SetMaxSize()
        {
            float mapWidth = max.x - min.x;
            float mapHeight = max.y - min.y;

            float maxCameraSizeX = mapWidth / (2f * cam.aspect);
            float maxCameraSizeY = mapHeight / 2f;

            maxSize = Mathf.Min(maxCameraSizeX, maxCameraSizeY);
        }

        /// <summary>
        /// 카메라의 위치와 줌을 설정합니다.
        /// </summary>
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

        /// <summary>
        /// 플레이어들의 중심점을 반환합니다.
        /// </summary>
        /// <param name="players">플레이어 리스트</param>
        /// <returns>계산된 플레이어들의 중심점</returns>
        private Vector3 GetCenterPoint(List<GameObject> players)
        {
            var bounds = new Bounds(players[0].transform.position, Vector3.zero);

            for (int i = 1; i < players.Count; i++)
            {
                bounds.Encapsulate(players[i].transform.position);
            }

            return bounds.center;
        }

        /// <summary>
        /// 카메라의 사이즈를 반환합니다.
        /// </summary>
        /// <param name="players">플레이어 리스트</param>
        /// <returns>게산된 카메라 사이즈</returns>
        private float GetCameraSize(List<GameObject> players)
        {
            if (isMaxCamera)
            {
                return maxSize;
            }

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

        /// <summary>
        /// 카메라 위치를 Clamp하여 화면 밖으로 나가지 않도록 하며, Clamp된 값을 반환합니다.
        /// </summary>
        /// <param name="targetPosition">카메라의 위치</param>
        /// <returns>카메라 위치를 Clamp하여 화면 밖으로 나가지 않도록 하며, Clamp된 값을 반환합니다.</returns>
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
        #endregion
    }
}
