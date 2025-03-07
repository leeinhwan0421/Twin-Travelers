using UnityEngine;

namespace TwinTravelers.Core.Gimmicks
{
    /// <summary>
    /// 회전하는 장애물 클래스
    /// </summary>
    public sealed class Spin : Obstacle
    {
        /// <summary>
        /// 회전 속도
        /// </summary>
        [Header("Preset")]
        [SerializeField] 
        private float rotationSpeed;

        /// <summary>
        /// 왼쪽으로 회전하는지 여부
        /// </summary>
        [SerializeField] 
        private bool isLeftRotation;

        private void Update()
        {
            Vector3 direction = isLeftRotation ? Vector3.forward : -Vector3.forward;
            transform.Rotate(direction * rotationSpeed * Time.deltaTime);
        }
    }
}
