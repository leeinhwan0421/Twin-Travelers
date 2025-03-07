using UnityEngine;
using TwinTravelers.Core.Utility;
using TwinTravelers.Management;

namespace TwinTravelers.Core.Gimmicks
{
    /// <summary>
    /// 열쇠를 획득하면 문을 열 수 있는 클래스
    /// </summary>
    public class Key : InteractableTrigger
    {
        /// <summary>
        /// 문 오브젝트의 Key_Door 컴포넌트
        /// </summary>
        [Header("Presets")]
        [Tooltip("문 오브젝트의 Key_Door 컴포넌트")]
        [SerializeField] 
        private Key_Door _door;

        /// <summary>
        /// 열쇠를 획득하면 나타나는 이펙트
        /// </summary>
        [Tooltip("열쇠를 획득하면 나타나는 이펙트")]
        [SerializeField]
        private GameObject effect;

        protected override void EnterEvent(Collider2D collision)
        {
            _door.Open();

            GameObject _effect = Instantiate(effect, transform.position, Quaternion.identity);
            _effect.GetComponent<SpriteRenderer>().color = GetComponent<SpriteRenderer>().color;

            AudioManager.Instance.PlaySFX("earnedKey");

            gameObject.SetActive(false);
        }

        protected override void ExitEvent(Collider2D collision)
        {
            // Nothing
        }
    }
}
