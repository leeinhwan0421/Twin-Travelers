using System.Collections.Generic;
using UnityEngine;

namespace TwinTravelers.Core.Utility
{
    /// <summary>
    /// 특정 태그와 반응하는 콜라이더를 정의합니다.
    /// </summary>
    public abstract class InteractableCollision : MonoBehaviour
    {
        [HideInInspector] 
        public List<string> selectedTags = new List<string>();

        /// <summary>
        /// 콜라이더가 트리거 모드거나, 태그가 지정되지 않았을 경우 경고 메시지를 출력합니다.
        /// </summary>
        private void Start()
        {
#if UNITY_EDITOR
            if (GetComponent<Collider2D>().isTrigger)
            {
                Debug.Log($"{gameObject.name} Collider2D is Trigger, this scripts need off trigger mode");
            }

            if (selectedTags.Count == 0)
            {
                Debug.LogWarning($"{gameObject.name}: No tags specified in the InteractableTrigger component. This may cause unintended behavior.");
            }
#endif
        }

        /// <summary>
        /// 충돌 이벤트를 정의합니다.
        /// </summary>
        /// <param name="collision">충돌체</param>
        protected abstract void EnterEvent(Collision2D collision);

        /// <summary>
        /// 충돌 이벤트를 정의합니다.
        /// </summary>
        /// <param name="collision">충돌체</param>
        protected abstract void ExitEvent(Collision2D collision);

        /// <summary>
        /// 태그 비교 후 메서드 호출
        /// </summary>
        /// <param name="collision">충돌체</param>
        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (selectedTags.Contains(collision.gameObject.tag))
            {
                EnterEvent(collision);
            }
        }

        /// <summary>
        /// 태그 비교 후 메서드 호출
        /// </summary>
        /// <param name="collision">충돌체</param>
        private void OnCollisionExit2D(Collision2D collision)
        {
            if (selectedTags.Contains(collision.gameObject.tag))
            {
                ExitEvent(collision);
            }
        }
    }
}
