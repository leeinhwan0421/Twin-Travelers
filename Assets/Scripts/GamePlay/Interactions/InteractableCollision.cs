using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InteractableCollision : MonoBehaviour
{
    [HideInInspector] public List<string> selectedTags = new List<string>();

    private void Start()
    {
#if UNITY_EDITOR
        if (GetComponent<Collider2D>().isTrigger)
        {
            Debug.Log($"{gameObject.name} Collider2D is Trigger, this scripts need off trigger mode");
        }

        if (selectedTags.Count == 0)
        {
            Debug.LogWarning($"{gameObject.name}: No tags specified in the InteractableTrigger component. This may cause unintended behavior.");
        }
#endif
    }

    protected abstract void EnterEvent(Collision2D collision);
    protected abstract void ExitEvent(Collision2D collision);

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (selectedTags.Contains(collision.gameObject.tag))
        {
            EnterEvent(collision);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (selectedTags.Contains(collision.gameObject.tag))
        {
            ExitEvent(collision);
        }
    }
}
