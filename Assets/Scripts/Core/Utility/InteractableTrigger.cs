using System.Collections.Generic;
using UnityEngine;

namespace TwinTravelers.Core.Utility
{
    public abstract class InteractableTrigger : MonoBehaviour
    {
        [HideInInspector] public List<string> selectedTags = new List<string>();

        /// <summary>
        /// 콜라이더가 트리거 모드가 아니거나, 태그가 지정되지 않았을 경우 경고 메시지를 출력합니다.
        /// </summary>
        private void Start()
        {
#if UNITY_EDITOR
            if (TryGetComponent<Collider2D>(out Collider2D coll2D))
            {
                if (coll2D.isTrigger == false)
                {
                    Debug.LogWarning($"{gameObject.name}: Collider2D is not set as a trigger. This script requires the Collider to be in trigger mode.");
                }
            }

            if (TryGetComponent<Collider>(out Collider coll))
            {
                if (coll.isTrigger == false)
                {
                    Debug.LogWarning($"{gameObject.name}: Collider is not set as a trigger. This script requires the Collider to be in trigger mode.");
                }
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
        protected abstract void EnterEvent(Collider2D collision);

        /// <summary>
        /// 충돌 이벤트를 정의합니다.
        /// </summary>
        /// <param name="collision">충돌체</param>
        protected abstract void ExitEvent(Collider2D collision);


        /// <summary>
        /// 태그 비교 후 메서드 호출
        /// </summary>
        /// <param name="collision">충돌체</param>
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (selectedTags.Contains(collision.tag))
            {
                EnterEvent(collision);
            }
        }

        /// <summary>
        /// 태그 비교 후 메서드 호출
        /// </summary>
        /// <param name="collision">충돌체</param>
        private void OnTriggerExit2D(Collider2D collision)
        {
            if (selectedTags.Contains(collision.tag))
            {
                ExitEvent(collision);
            }
        }
    }
}
