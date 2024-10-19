using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InteractableTrigger : MonoBehaviour
{
    [Header("Preset")]
    [SerializeField] private string interatableTag;

    private void Start()
    {
#if SHOW_DEBUG_MESSAGES
        if (TryGetComponent<Collider2D>(out Collider2D coll2D))
        {
            if (coll2D.isTrigger == false)
            {
                Debug.Log($"{gameObject.name} Collider2D is not Trigger, this scripts need trigger mode");
            }
        }

        if (TryGetComponent<Collider>(out Collider coll))
        {
            if (coll.isTrigger == false)
            {
                Debug.Log($"{gameObject.name} Collider is not Trigger, this scripts need trigger mode");
            }
        }
#endif
    }

    protected abstract void EnterEvent(Collider2D collision);
    protected abstract void ExitEvent(Collider2D collision);

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(interatableTag))
        {
            EnterEvent(collision);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag(interatableTag))
        {
            ExitEvent(collision);
        }
    }
}
