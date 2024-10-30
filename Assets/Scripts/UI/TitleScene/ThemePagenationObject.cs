using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ThemePagenationObject : MonoBehaviour
{
    [Header("Presets")]
    [SerializeField] private Sprite onSprite;
    [SerializeField] private Sprite offSprite;

    private Image image;

    private void Awake()
    {
        image = GetComponent<Image>();
    }

    public void ChangeOnSprite()
    {
        image.sprite = onSprite;
    }

    public void ChangeOffSprite()
    {
        image.sprite = offSprite;
    }
}
