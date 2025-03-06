using TwinTravelers.Management;
using UnityEngine;

namespace TwinTravelers.Core.Actor
{
    /// <summary>
    /// �÷��̾� ���� Ŭ����
    /// </summary>
    public class PlayerBalloon : MonoBehaviour
    {
        #region Fields
        /// <summary>
        /// �÷��̾� Ŭ������ ������ �ִ� ������Ʈ
        /// </summary>
        [Header("Preset")]
        [Tooltip("�÷��̾� Ŭ������ ������ �ִ� ������Ʈ")]
        [SerializeField] 
        private GameObject player;

        /// <summary>
        /// ǳ���� Ȱ��ȭ �� ���¿����� �߷� ������
        /// </summary>
        [Header("Gravity Settings")]
        [Range(0.01f, 0.99f)][SerializeField] private float gravityScaleMultipler;

        /// <summary>
        /// ���� �߷� ������
        /// </summary>
        private float originalGravityScale;

        /// <summary>
        /// �ּ� �ӵ� ����
        /// </summary>
        [Header("Velocity Limit Settings")]
        [Tooltip("�ּ� �ӵ� ����")]
        [SerializeField] 
        private Vector2 min;

        /// <summary>
        /// �ִ� �ӵ� ����
        /// </summary>
        [Tooltip("�ִ� �ӵ� ����")]
        [SerializeField] 
        private Vector2 max;

        /// <summary>
        /// ������ ����� ����Ʈ
        /// </summary>
        [Header("Effect")]
        [Tooltip("������ ����� ����Ʈ")]
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
        /// ����Ʈ�� �����ϰ� ǳ���� ��Ȱ��ȭ ��ŵ�ϴ�.
        /// </summary>
        public void ExplodeBalloon()
        {
            Instantiate(effect, transform.position, Quaternion.identity);
            AudioManager.Instance.PlaySFX("BalloonUnprepared");
            gameObject.SetActive(false);
        }
    }
}
