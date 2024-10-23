using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Two_Button_Door : MonoBehaviour
{
    [Header("Presets")]
    [SerializeField] private Two_Button_Door other;
    [SerializeField] private Animator door;

    [Header("Sprites")]
    [SerializeField] private Sprite activeSprite;
    [SerializeField] private Sprite deactiveSprite;

    private SpriteRenderer spriteRenderer;

    public bool isActive = false;
    private int cnt = 0;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void ResetTwoButtonDoor()
    {
        isActive = false;
        cnt = 0;

        door.Rebind();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player") && !collision.CompareTag("Moveable"))
        {
            return;
        }

        cnt++;

        if (cnt == 1)
        {
            isActive = true;
            spriteRenderer.sprite = activeSprite;
            AudioManager.Instance.PlaySFX("ButtonEnter");


            if (other.isActive == true)
            {
                door.SetTrigger("Open");
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player") && !collision.CompareTag("Moveable"))
        {
            return;
        }

        cnt--;

        if (cnt == 0)
        {
            isActive = false;
            spriteRenderer.sprite = deactiveSprite;
            AudioManager.Instance.PlaySFX("ButtonExit");
        }
    }
}
