using System.Collections;
using UnityEngine;
using TwinTravelers.Core.Utility;
using TwinTravelers.Management;

namespace TwinTravelers.Core.Gimmicks
{
    public class Button_Door : InteractableTrigger
    {
        #region Fields
        /// <summary>
        /// 문 오브젝트의 트랜스폼
        /// </summary>
        [Header("Presets")]
        [Tooltip("문 오브젝트의 트랜스폼")]
        [SerializeField] 
        private Transform door;

        /// <summary>
        /// 문이 열리는 속도
        /// </summary>
        [Tooltip("문이 열리는 속도")]
        [SerializeField] 
        private float doorSpeed = 2f;

        /// <summary>
        /// 문이 열렸을 때의 로컬 좌표
        /// </summary>
        [Tooltip("문이 열렸을 때의 로컬 좌표")]
        [SerializeField, Space(10.0f)] 
        private Vector3 openLocalPosition;

        /// <summary>
        /// 문이 닫혔을 때의 로컬 좌표
        /// </summary>
        [Tooltip("문이 닫혔을 때의 로컬 좌표")]
        [SerializeField] 
        private Vector3 closeLocalPosition;

        /// <summary>
        /// 버튼이 눌린 상태일 때의 스프라이트
        /// </summary>
        [Tooltip("버튼이 눌린 상태일 때의 스프라이트")]
        [SerializeField, Space(10.0f)]
        private Sprite openSprite;

        /// <summary>
        /// 버튼이 눌리지 않은 상태일 때 스프라이트
        /// </summary>
        [Tooltip("버튼이 눌리지 않은 상태일 때 스프라이트")]
        [SerializeField]
        private Sprite closeSprite;

        /// <summary>
        /// 이 컴포넌트를 가지고 있는 오브젝트의 스프라이트 렌더러 컴포넌트
        /// </summary>
        private SpriteRenderer spriteRenderer;

        /// <summary>
        /// 현재 버튼에 눌린 플레이어 수
        /// </summary>
        private int currentCount;

        /// <summary>
        /// 열렸는지 여부
        /// </summary>
        private bool isOpening;
        #endregion

        #region Unity Methods
        private void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        private void Start()
        {
            ResetButtonDoor();
        }

        private void OnDisable()
        {
            StopAllCoroutines();
        }
        #endregion

        #region Methods
        protected override void EnterEvent(Collider2D collision)
        {
            currentCount++;

            if (currentCount == 1 && !isOpening)
            {
                StopAllCoroutines();
                StartCoroutine(MoveDoor(openLocalPosition));

                AudioManager.Instance.PlaySFX("ButtonEnter");

                spriteRenderer.sprite = openSprite;
                isOpening = true;
            }
        }

        protected override void ExitEvent(Collider2D collision)
        {
            currentCount--;

            if (currentCount == 0 && isOpening)
            {
                StopAllCoroutines();
                StartCoroutine(MoveDoor(closeLocalPosition));

                AudioManager.Instance.PlaySFX("ButtonExit");

                spriteRenderer.sprite = closeSprite;
                isOpening = false;
            }
        }

        /// <summary>
        /// 문을 열기 위한 코루틴
        /// </summary>
        /// <param name="targetPosition">문이 이동할 타겟 로컬 좌표</param>
        /// <returns>IEnumerator</returns>
        private IEnumerator MoveDoor(Vector3 targetPosition)
        {
            while (Vector3.Distance(door.localPosition, targetPosition) > 0.01f)
            {
                door.localPosition = Vector3.MoveTowards(door.localPosition, targetPosition, doorSpeed * Time.deltaTime);
                yield return null;
            }

            door.localPosition = targetPosition;
        }

        /// <summary>
        /// 버튼과 문을 초기화
        /// </summary>
        public void ResetButtonDoor()
        {
            StopAllCoroutines();

            door.localPosition = closeLocalPosition;
            spriteRenderer.sprite = closeSprite;
            currentCount = 0;
            isOpening = false;
        }
        #endregion
    }
}
