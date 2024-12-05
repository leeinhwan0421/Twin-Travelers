using System.Collections.Generic;
using UnityEngine;

public abstract class InteractableTrigger : MonoBehaviour
{
    [HideInInspector] public List<string> selectedTags = new List<string>();

    private void Start()
    {
#if UNITY_EDITOR 
        if (TryGetComponent<Collider2D>(out Collider2D coll2D))
        {
            if (coll2D.isTrigger == false)
            {
                Debug.LogWarning($"{gameObject.name}: Collider2D is not set as a trigger. This script requires the Collider to be in trigger mode.");
            }
        }

        if (TryGetComponent<Collider>(out Collider coll))
        {
            if (coll.isTrigger == false)
            {
                Debug.LogWarning($"{gameObject.name}: Collider is not set as a trigger. This script requires the Collider to be in trigger mode.");
            }
        }

        if (selectedTags.Count == 0)
        {
            Debug.LogWarning($"{gameObject.name}: No tags specified in the InteractableTrigger component. This may cause unintended behavior.");
        }
#endif
    }

    protected abstract void EnterEvent(Collider2D collision);
    protected abstract void ExitEvent(Collider2D collision);

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (selectedTags.Contains(collision.tag))
        {
            EnterEvent(collision);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (selectedTags.Contains(collision.tag))
        {
            ExitEvent(collision);
        }
    }
}
