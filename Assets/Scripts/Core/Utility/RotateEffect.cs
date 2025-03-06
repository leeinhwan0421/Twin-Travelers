using UnityEngine;

namespace TwinTravelers.Core.Utility
{
    public class RotateEffect : MonoBehaviour
    {
        [Header("Preset")]

        /// <summary>
        /// 회전 방향
        /// </summary>
        [Tooltip("회전 방향")]
        [SerializeField]
        private Vector3 rotation;

        /// <summary>
        /// 회전 속도
        /// </summary>
        [Tooltip("회전 속도")]
        [SerializeField]
        private float speed;

        private void Update()
        {
            transform.Rotate(speed * Time.deltaTime * rotation.normalized);
        }
    }
}
