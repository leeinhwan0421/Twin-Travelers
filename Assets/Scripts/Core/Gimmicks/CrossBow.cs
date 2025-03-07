using UnityEngine;
using TwinTravelers.Management;

namespace TwinTravelers.Core.Gimmicks
{
    /// <summary>
    /// 화살을 발사하는 크로스보우 클래스
    /// </summary>
    public class CrossBow : MonoBehaviour
    {
        /// <summary>
        /// 화살이 발사될 위치
        /// </summary>
        [Header("Presets")]
        [SerializeField] 
        private Transform muzzle;

        [Space(10.0f)]

        /// <summary>
        /// 화살 발사 속도
        /// </summary>
        [Tooltip("화살 발사 속도")]
        [Range(0.05f, 1.0f)]
        [SerializeField] 
        private float shootSpeed;

        /// <summary>
        /// 화살 발사 주기
        /// </summary>
        [Tooltip("화살 발사 주기")]
        [Range(1.0f, 10.0f)]
        [SerializeField] 
        private float shootTime;

        /// <summary>
        /// 화살 프리팹
        /// </summary>
        [Tooltip("화살 프리팹")]
        [Header("Prefab")]
        [SerializeField] 
        private GameObject arrow;

        /// <summary>
        /// 크로스보우 애니메이터
        /// </summary>
        private Animator animator;

        /// <summary>
        /// 타이머 변수
        /// </summary>
        private float timer = 0.0f;

        private void Awake()
        {
            animator = GetComponent<Animator>();
        }

        private void Update()
        {
            timer += Time.deltaTime;

            if (timer >= shootTime)
            {
                Shot();
                timer = 0.0f;
            }
        }

        /// <summary>
        /// 크로스보우 발사 메서드
        /// </summary>
        private void Shot()
        {
            animator.SetTrigger("Shot");

            AudioManager.Instance.PlaySFX("Crossbow");
        }

        /// <summary>
        /// 애니메이션 이벤트에서 호출되는 화살 발사 메서드
        /// </summary>
        private void ShotArrow()
        {
            GameObject arrow = Instantiate(this.arrow, muzzle.position, muzzle.rotation);
            arrow.GetComponent<Rigidbody2D>().AddForce(muzzle.right * shootSpeed);

            timer = 0.0f;
        }
    }
}
