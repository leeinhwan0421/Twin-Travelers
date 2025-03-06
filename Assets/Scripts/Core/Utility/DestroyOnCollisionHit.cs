using UnityEngine;

namespace TwinTravelers.Core.Utility
{
    public class DestroyOnCollisionHit : InteractableTrigger
    {
        [Header("Preset")]
        [SerializeField] private GameObject effect;

        protected override void EnterEvent(Collider2D collision)
        {
            if (effect != null)
            {
                Instantiate(effect, transform.position, Quaternion.identity);
                effect.GetComponent<SpriteRenderer>().flipX = GetComponent<SpriteRenderer>().flipX;
            }

            Destroy(gameObject);
        }

        protected override void ExitEvent(Collider2D collision)
        {
            // Nothing
        }
    }
}
