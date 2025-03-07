using UnityEngine;
using TwinTravelers.Core.Utility;
using TwinTravelers.Management;

namespace TwinTravelers.Core.Gimmicks
{
    public class Two_Button_Door : InteractableTrigger
    {
        #region Fields
        /// <summary>
        /// 다른 버튼
        /// </summary>
        [Header("Presets")]
        [Tooltip("다른 버튼")]
        [SerializeField] 
        private Two_Button_Door other;

        /// <summary>
        /// 문 애니메이터
        /// </summary>
        [Tooltip("문 애니메이터")]
        [SerializeField]
        private Animator door;

        /// <summary>
        /// 활성화된 스프라이트 에셋
        /// </summary>
        [Header("Sprites")]
        [Tooltip("활성화된 스프라이트 에셋")]
        [SerializeField] 
        private Sprite activeSprite;

        /// <summary>
        /// 비활성화된 스프라이트 에셋
        /// </summary>
        [Tooltip("비활성화된 스프라이트 에셋")]
        [SerializeField]
        private Sprite deactiveSprite;

        /// <summary>
        /// 컨트롤 할 스프라이트 렌더러
        /// </summary>
        private SpriteRenderer spriteRenderer;

        /// <summary>
        /// 버튼 활성화 여부
        /// </summary>
        public bool isActive = false;

        /// <summary>
        /// 버튼 카운트
        /// </summary>
        private int cnt = 0;
        #endregion

        #region Unity Methods
        private void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }
        #endregion

        #region Methods
        /// <summary>
        /// 버튼 및 문을 초기화합니다.
        /// </summary>
        public void ResetTwoButtonDoor()
        {
            isActive = false;
            cnt = 0;

            door.Rebind();
        }

        protected override void EnterEvent(Collider2D collision)
        {
            if (!collision.CompareTag("Player") && !collision.CompareTag("Moveable"))
            {
                return;
            }

            cnt++;

            if (cnt == 1)
            {
                isActive = true;
                spriteRenderer.sprite = activeSprite;
                AudioManager.Instance.PlaySFX("ButtonEnter");


                if (other.isActive == true)
                {
                    door.SetTrigger("Open");
                }
            }
        }

        protected override void ExitEvent(Collider2D collision)
        {
            if (!collision.CompareTag("Player") && !collision.CompareTag("Moveable"))
            {
                return;
            }

            cnt--;

            if (cnt == 0)
            {
                isActive = false;
                spriteRenderer.sprite = deactiveSprite;
                AudioManager.Instance.PlaySFX("ButtonExit");
            }
        }
        #endregion
    }
}
