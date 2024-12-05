using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashPlatform : MonoBehaviour
{
    [Header("Presets")]
    // 활성화 유지 되는 시간
    [Range(1.0f, 10.0f)] [SerializeField] private float activeTime;
    // 비활성화 유지 되는 시간
    [Range(1.0f, 10.0f)] [SerializeField] private float deactiveTime;

    private SpriteRenderer spriteRenderer;
    private Collider2D coll;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        coll = GetComponent<Collider2D>();

        StartCoroutine(Cycle());
    }

    private void Activate()
    {
        coll.enabled = true;
        spriteRenderer.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
    }

    private void Deactivate()
    {
        coll.enabled = false;
        spriteRenderer.color = new Color(1.0f, 1.0f, 1.0f, 0.2f);
    }

    private IEnumerator Cycle()
    {
        while (true)
        {
            yield return new WaitForSeconds(activeTime);

            Deactivate();

            yield return new WaitForSeconds(deactiveTime);

            Activate();
        }
    }
}
