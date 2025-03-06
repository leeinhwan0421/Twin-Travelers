using UnityEngine;
using TwinTravelers.UI;
using TwinTravelers.Management;

namespace TwinTravelers.Core.Utility
{
    public class SignInteraction : InteractableTrigger
    {
        [Header("Presets")]
        [TextArea(1, 3)][SerializeField] private string text;
        [Space(10.0f)]
        [SerializeField] private Panel panel;
        [SerializeField] private TextWriter textWriter;

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
