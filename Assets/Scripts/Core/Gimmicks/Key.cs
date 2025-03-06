using UnityEngine;
using TwinTravelers.Core.Utility;
using TwinTravelers.Management;

namespace TwinTravelers.Core.Gimmicks
{
    public class Key : InteractableTrigger
    {
        [Header("Presets")]
        [SerializeField] private Key_Door _door;
        [SerializeField] private GameObject effect;

        protected override void EnterEvent(Collider2D collision)
        {
            _door.Open();

            GameObject _effect = Instantiate(effect, transform.position, Quaternion.identity);
            _effect.GetComponent<SpriteRenderer>().color = GetComponent<SpriteRenderer>().color;

            AudioManager.Instance.PlaySFX("earnedKey");

            gameObject.SetActive(false);
        }

        protected override void ExitEvent(Collider2D collision)
        {
            // Nothing
        }
    }
}
