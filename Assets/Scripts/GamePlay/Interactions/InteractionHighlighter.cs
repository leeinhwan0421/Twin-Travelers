using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InteractionHighlighter : MonoBehaviour
{
    [Header("Presets")]
    [SerializeField] private List<SpriteRenderer> activeList = new List<SpriteRenderer>();

    [Header("Materials")]
    [SerializeField] private Material defaultMaterial;
    [SerializeField] private Material highlightPrefab;

    private void OnMouseEnter()
    {
        foreach (var item in activeList)
        {
            item.material = highlightPrefab;
        }
    }

    private void OnMouseExit()
    {
        foreach (var item in activeList)
        {
            item.material = defaultMaterial;
        }
    }
}
