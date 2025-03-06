using UnityEngine;
using TwinTravelers.UI;
using TwinTravelers.Management;

namespace TwinTravelers.Core.Utility
{
    public class SignInteraction : InteractableTrigger
    {
        /// <summary>
        /// 출력할 텍스트
        /// </summary>
        [Header("Presets")]
        [SerializeField, TextArea(1, 3)] 
        private string text;

        /// <summary>
        /// 출력할 패널
        /// </summary>
        [SerializeField, Space(10.0f)] 
        private Panel panel;

        /// <summary>
        /// 사용할 텍스쳐 라이터 컴포넌트
        /// </summary>
        [SerializeField] 
        private TextWriter textWriter;

        /// <summary>
        /// 플레이어가 두명이기 때문에 중복을 막으려고 선언
        /// </summary>
        private int playerCount = 0;

        protected override void EnterEvent(Collider2D collision)
        {
            if (playerCount == 0)
            {
                AudioManager.Instance.PlaySFX("Sign");

                panel.Enable();
                textWriter.WriteText(text);
            }

            playerCount++;
        }

        protected override void ExitEvent(Collider2D collision)
        {
            playerCount--;

            if (playerCount == 0)
            {
                panel.Disable();
            }
        }
    }
}
