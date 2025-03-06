using TwinTravelers.Management;
using UnityEngine;

namespace TwinTravelers.Core.Gimmicks
{
    /// <summary>
    /// 커서를 올리면 움직이는 새 클래스
    /// </summary>
    public class Bird : MonoBehaviour
    {
        /// <summary>
        /// 새 오브젝트의 애니메이터 컴포넌트
        /// </summary>
        private Animator anim;

        /// <summary>
        /// 새 오브젝트의 위치
        /// </summary>
        private Vector3 offset;

        /// <summary>
        /// 새 오브젝트의 목표 위치
        /// </summary>
        private Vector3 targetPosition;

        [Header("Offset")]
        public Vector3 min;
        public Vector3 max;
        public float lerpSpeed;

        private void Awake()
        {
            anim = GetComponent<Animator>();

            offset = transform.position;
            targetPosition = offset;
        }

        private void FixedUpdate()
        {
            transform.position = Vector3.Slerp(transform.position, targetPosition, Time.fixedDeltaTime * lerpSpeed);
        }

        private void OnMouseEnter()
        {
            if (anim == null) return;

            anim.SetBool("OnCursor", true);

            AudioManager.Instance.PlaySFX("Bird");

            float x = Random.Range(min.x, max.x);
            float y = Random.Range(min.y, max.y);
            float z = Random.Range(min.z, max.z);

            targetPosition = offset + new Vector3(x, y, z);
        }

        private void OnMouseExit()
        {
            if (anim == null) return;

            anim.SetBool("OnCursor", false);

            targetPosition = offset;
        }
    }
}
