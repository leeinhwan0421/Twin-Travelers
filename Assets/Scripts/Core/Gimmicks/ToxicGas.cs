using UnityEngine;
using TwinTravelers.Core.Utility;
using TwinTravelers.Management;

namespace TwinTravelers.Core.Gimmicks
{
    public class ToxicGas : InteractableTrigger
    {
        protected override void EnterEvent(Collider2D collision)
        {
            AudioManager.Instance.PlaySFX("ToxicGas");
        }

        protected override void ExitEvent(Collider2D collision)
        {
            // Nothing..
        }
    }
}
