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
    public Button_Type buttonType;

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
}
