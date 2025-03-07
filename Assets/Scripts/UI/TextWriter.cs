using System.Collections;
using UnityEngine;
using TMPro;
using TwinTravelers.Management;

namespace TwinTravelers.UI
{
    /// <summary>
    /// 텍스트를 타이핑하는 효과로 작성하는 클래스
    /// </summary>
    public class TextWriter : MonoBehaviour
    {
        #region Fields
        /// <summary>
        /// 글자 타이핑 간격
        /// </summary>
        [Header("Preset")]
        [Tooltip("글자 타이핑 간격")]
        [Range(0.1f, 1.0f)][SerializeField] 
        private float typingInterval;

        /// <summary>
        /// 텍스트
        /// </summary>
        private TextMeshProUGUI text;
        #endregion

        #region Unity Methods
        private void Awake()
        {
            text = GetComponent<TextMeshProUGUI>();
        }

        private void OnDisable()
        {
            text.text = "";
        }
        #endregion

        #region Methods
        /// <summary>
        /// 텍스트를 타이핑하는 효과로 작성하는 코루틴 호출
        /// </summary>
        /// <param name="text">작성할 텍스트</param>
        public void WriteText(string text)
        {
            StopAllCoroutines();

            StartCoroutine(WriteTextCoroutine(text));
        }

        /// <summary>
        /// 텍스트를 타이핑하는 효과로 작성합니다.
        /// </summary>
        /// <param name="text">작성할 텍스트</param>
        /// <returns>IEnumerator</returns>
        private IEnumerator WriteTextCoroutine(string text)
        {
            this.text.text = "";

            for (int i = 0; i < text.Length; i++)
            {
                this.text.text += text[i];
                AudioManager.Instance.PlaySFX("SignTyping");
                yield return new WaitForSeconds(typingInterval);
            }

            this.text.text = text;
        }
        #endregion
    }
}
