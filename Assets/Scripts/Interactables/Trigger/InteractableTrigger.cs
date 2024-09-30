using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InteractableTrigger : MonoBehaviour
{
    [Header("Preset")]
    [SerializeField] private string interatableTag;
    
    protected abstract void Event(Collider2D collision);

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(interatableTag))
        {
            Event(collision);
        }
    }
}
