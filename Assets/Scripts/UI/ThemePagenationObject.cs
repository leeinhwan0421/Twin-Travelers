using UnityEngine;
using UnityEngine.UI;

namespace TwinTravelers.UI
{
    /// <summary>
    /// 스테이지 페이징 클래스
    /// </summary>
    public class ThemePagenationObject : MonoBehaviour
    {
        #region Fields
        /// <summary>
        /// 이동 가능할 경우 활성화될 스프라이트
        /// </summary>
        [Header("Presets")]
        [SerializeField] 
        private Sprite onSprite;

        /// <summary>
        /// 이동 불가능할 경우 비활성화될 스프라이트
        /// </summary>
        [SerializeField] 
        private Sprite offSprite;

        /// <summary>
        /// 이미지 컴포넌트
        /// </summary>
        private Image image;
        #endregion

        #region Unity Methods
        private void Awake()
        {
            image = GetComponent<Image>();
        }
        #endregion

        #region Methods
        /// <summary>
        /// 활성화 스프라이트로 변경
        /// </summary>
        public void ChangeOnSprite()
        {
            image.sprite = onSprite;
        }

        /// <summary>
        /// 비활성화 스프라이트로 변경
        /// </summary>
        public void ChangeOffSprite()
        {
            image.sprite = offSprite;
        }
        #endregion
    }
}
