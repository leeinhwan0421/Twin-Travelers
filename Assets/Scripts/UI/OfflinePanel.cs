using System.Collections;
using TMPro;
using UnityEngine;
using TwinTravelers.Management;

namespace TwinTravelers.UI
{
    /// <summary>
    /// 오프라인 패널
    /// </summary>
    public class OfflinePanel : Panel
    {
        #region Fields
        /// <summary>
        /// 네트워크 패널
        /// </summary>
        [Tooltip("네트워크 패널")]
        [SerializeField] 
        private GameObject networkPanel;

        /// <summary>
        /// 패널 내부 텍스트
        /// </summary>
        [Tooltip("패널 내부 텍스트")]
        [SerializeField]
        private TextMeshProUGUI text;

        /// <summary>
        /// 타이틀로 돌아가는 시간
        /// </summary>
        [Tooltip("타이틀로 돌아가는 시간")]
        [SerializeField]
        private float timeToGoTitle = 3.0f;
        #endregion

        #region Methods
        public new void Enable()
        {
            base.Enable();

            StartCoroutine(UpdateCoroutine());
        }

        public new void Disable()
        {
            base.Disable();

            StopAllCoroutines();
        }

        /// <summary>
        /// 업데이트 코루틴
        /// </summary>
        /// <returns>IEnumerator</returns>
        private IEnumerator UpdateCoroutine()
        {
            float timeRemaining = timeToGoTitle;

            while (timeRemaining > 0)
            {
                text.text = $"오프라인 모드로 전환되었습니다.\r\n{Mathf.RoundToInt(timeRemaining).ToString()} 초 뒤 타이틀로 돌아갑니다.\r\n인터넷에 연결 될 경우\r\n같이하기 기능을 사용할 수 있습니다.";

                yield return new WaitForSeconds(1.0f);

                timeRemaining -= 1.0f;
            }

            LoadSceneManager.LoadScene("TitleScene");

            networkPanel.SetActive(false);
            Disable();
        }
        #endregion
    }
}
