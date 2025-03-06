using TwinTravelers.Core.Actor;
using UnityEngine;
using TwinTravelers.Core.Utility;
using TwinTravelers.Management;

namespace TwinTravelers.Core.Gimmicks
{
    public class Jumppad : InteractableTrigger
    {
        [Header("Preset")]
        [Range(1.0f, 5.0f)][SerializeField] private float jumpMultiplier = 1.0f;

        protected override void EnterEvent(Collider2D collision)
        {
            AudioManager.Instance.PlaySFX("Jumppad");
            collision.GetComponent<Player>().jumpMultiplier = this.jumpMultiplier;
        }

        protected override void ExitEvent(Collider2D collision)
        {
            collision.GetComponent<Player>().jumpMultiplier = 1.0f;
        }
    }
}
