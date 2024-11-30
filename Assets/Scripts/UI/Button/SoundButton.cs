using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public enum Button_Type
{
    Normal = 0,
    Confirm = 1,
    Block = 2,
}

public class SoundButton : Button
{

    private Graphic[] graphics;
    protected Graphic[] Graphics
    {
        get
        {
            if (graphics == null)
                graphics = targetGraphic.transform.GetComponentsInChildren<Graphic>();
            return graphics;
        }
    }

    public Button_Type buttonType;

    #region Sound
    public void PlayHoverSound()
    {
        AudioManager.Instance.PlaySFX("UIHover");
    }

    public void PlayClickSound()
    {
        switch (buttonType)
        {
            case Button_Type.Normal:
                AudioManager.Instance.PlaySFX("UIClick");
                break;
            case Button_Type.Confirm:
                AudioManager.Instance.PlaySFX("UIConfirm");
                break;
            case Button_Type.Block:
                AudioManager.Instance.PlaySFX("UIBlock");
                break;
        }
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        base.OnPointerEnter(eventData);
        PlayHoverSound();
    }

    public override void OnPointerClick(PointerEventData eventData)
    {
        base.OnPointerClick(eventData);
        PlayClickSound();
    }
    #endregion

    #region Color Change
    protected override void DoStateTransition(SelectionState state, bool instant)
    {
        Color color;

        switch (state)
        {
            case SelectionState.Normal:
                color = colors.normalColor;
                break;
            case SelectionState.Highlighted:
                color = colors.highlightedColor;
                break;
            case SelectionState.Pressed:
                color = colors.pressedColor;
                break;
            case SelectionState.Selected:
                color = colors.selectedColor;
                break;
            case SelectionState.Disabled:
                color = colors.disabledColor;
                break;
            default:
                color = Color.black;
                break;
        }

        if (gameObject.activeInHierarchy)
        {
            switch (transition)
            {
                case Transition.ColorTint:
                    ColorTween(color * colors.colorMultiplier, instant);
                    break;
                default:
                    break;
            }
        }
    }
    private void ColorTween(Color targetColor, bool instant)
    {
        if (targetGraphic == null) return;
        for (int i = 0; i < Graphics.Length; ++i) Graphics[i].CrossFadeColor(targetColor, (!instant) ? colors.fadeDuration : 0f, true, true);
    }
#endregion
}
