using UnityEngine;
using TwinTravelers.Core.Utility;
using TwinTravelers.Management;

namespace TwinTravelers.Core.Gimmicks
{
    public class TrapPlatform : InteractableCollision
    {
        [Header("Presets")]
        [SerializeField] private GameObject effect;

        private int count = 0;

        public void ResetTrapPlatform()
        {
            count = 0;
            gameObject.SetActive(true);
        }

        protected override void EnterEvent(Collision2D collision)
        {
            count++;

            if (count >= 2)
            {
                gameObject.SetActive(false);
                AudioManager.Instance.PlaySFX("BreakWood");
                Instantiate(effect, transform.position, Quaternion.identity);
            }
        }

        protected override void ExitEvent(Collision2D collision)
        {
            count--;
        }
    }
}
