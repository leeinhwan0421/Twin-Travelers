using TwinTravelers.Management;
using UnityEngine;

namespace TwinTravelers.Core.Actor
{
    /// <summary>
    /// 플레이어 벌룬 클래스
    /// </summary>
    public class PlayerBalloon : MonoBehaviour
    {
        #region Fields
        /// <summary>
        /// 플레이어 클래스를 가지고 있는 오브젝트
        /// </summary>
        [Header("Preset")]
        [Tooltip("플레이어 클래스를 가지고 있는 오브젝트")]
        [SerializeField] 
        private GameObject player;

        /// <summary>
        /// 풍선이 활성화 된 상태에서의 중력 스케일
        /// </summary>
        [Header("Gravity Settings")]
        [Range(0.01f, 0.99f)][SerializeField] private float gravityScaleMultipler;

        /// <summary>
        /// 원본 중력 스케일
        /// </summary>
        private float originalGravityScale;

        /// <summary>
        /// 최소 속도 제한
        /// </summary>
        [Header("Velocity Limit Settings")]
        [Tooltip("최소 속도 제한")]
        [SerializeField] 
        private Vector2 min;

        /// <summary>
        /// 최대 속도 제한
        /// </summary>
        [Tooltip("최대 속도 제한")]
        [SerializeField] 
        private Vector2 max;

        /// <summary>
        /// 터질때 생기는 이펙트
        /// </summary>
        [Header("Effect")]
        [Tooltip("터질때 생기는 이펙트")]
        [SerializeField]
        private GameObject effect;
        #endregion

        private void Awake()
        {
            originalGravityScale = player.GetComponent<Rigidbody2D>().gravityScale;
        }

        private void OnEnable()
        {
            player.GetComponent<Rigidbody2D>().gravityScale = originalGravityScale * gravityScaleMultipler;
            AudioManager.Instance.PlaySFX("BalloonEquipped");
        }

        private void OnDisable()
        {
            player.GetComponent<Rigidbody2D>().gravityScale = originalGravityScale;
        }

        private void Update()
        {
            Rigidbody2D rigid = player.GetComponent<Rigidbody2D>();
            Vector2 velocity = rigid.velocity;

            float x = Mathf.Clamp(velocity.x, min.x, max.x);
            float y = Mathf.Clamp(velocity.y, min.y, max.y);
            rigid.velocity = new Vector2(x, y);
        }

        /// <summary>
        /// 이펙트를 생성하고 풍선을 비활성화 시킵니다.
        /// </summary>
        public void ExplodeBalloon()
        {
            Instantiate(effect, transform.position, Quaternion.identity);
            AudioManager.Instance.PlaySFX("BalloonUnprepared");
            gameObject.SetActive(false);
        }
    }
}
