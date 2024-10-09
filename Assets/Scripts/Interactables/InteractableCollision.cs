using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InteractableCollision : MonoBehaviour
{
    [Header("Preset")]
    [SerializeField] private string interatableTag;

    private void Start()
    {
#if SHOW_DEBUG_MESSAGES
        if (GetComponent<Collider2D>().isTrigger)
        {
            Debug.Log($"{gameObject.name} Collider2D is Trigger, this scripts need off trigger mode");
        }
#endif
    }

    protected abstract void EnterEvent(Collision2D collision);
    protected abstract void ExitEvent(Collision2D collision);

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag(interatableTag))
        {
            EnterEvent(collision);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.CompareTag(interatableTag))
        {
            ExitEvent(collision);
        }
    }
}
