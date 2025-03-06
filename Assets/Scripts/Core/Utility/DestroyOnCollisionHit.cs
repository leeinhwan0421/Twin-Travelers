using UnityEngine;

namespace TwinTravelers.Core.Utility
{
    /// <summary>
    /// 충돌 시 삭제되는 오브젝트를 정의하는 클래스
    /// </summary>
    public class DestroyOnCollisionHit : InteractableTrigger
    {
        /// <summary>
        /// 삭제 될 때 이펙트
        /// </summary>
        [Header("Preset")]
        [Tooltip("삭제 될 때 이펙트")]
        [SerializeField] 
        private GameObject effect;

        protected override void EnterEvent(Collider2D collision)
        {
            if (effect != null)
            {
                Instantiate(effect, transform.position, Quaternion.identity);
                effect.GetComponent<SpriteRenderer>().flipX = GetComponent<SpriteRenderer>().flipX;
            }

            Destroy(gameObject);
        }

        protected override void ExitEvent(Collider2D collision)
        {
            // Nothing
        }
    }
}
