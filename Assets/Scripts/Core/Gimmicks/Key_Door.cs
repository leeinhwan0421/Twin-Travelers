using UnityEngine;

namespace TwinTravelers.Core.Gimmicks
{
    public class Key_Door : MonoBehaviour
    {
        /// <summary>
        /// 문의 색상
        /// </summary>
        [Header("Presets")]
        [Tooltip("문의 색상")]
        [SerializeField] 
        private Color color;

        [Space(10.0f)]

        /// <summary>
        /// 열쇠 오브젝트
        /// </summary>
        [Tooltip("열쇠 오브젝트")]
        [SerializeField]
        private GameObject key;

        [Space(10.0f)]

        /// <summary>
        /// 문의 애니메이터 컴포넌트
        /// </summary>
        [Tooltip("문의 애니메이터 컴포넌트")]
        [SerializeField]
        private Animator doorAnim;

        private void Start()
        {
            SpriteRenderer[] sr = GetComponentsInChildren<SpriteRenderer>();

            for (int i = 0; i < sr.Length; i++)
            {
                sr[i].color = color;
            }
        }

        /// <summary>
        /// 문을 엽니다.
        /// </summary>
        public void Open()
        {
            doorAnim.SetTrigger("Open");
        }

        /// <summary>
        /// 문과 열쇠을 초기화합니다.
        /// </summary>
        public void ResetKeyDoor()
        {
            doorAnim.Rebind();
            doorAnim.ResetTrigger("Open");

            key.SetActive(true);
        }
    }
}
