using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeController : MonoBehaviour
{
    [Header("Preset")]

    [Range(0f, 1f)] [SerializeField] private float maxAlpha;
    [Range(0f, 1f)] [SerializeField] private float minAlpha;

    [Range(0.01f, 3f)] [SerializeField] private float fadeSpeed;
    [SerializeField] private float fadeInterval;

    // private value..
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        StartCoroutine(FadeCoroutine());
    }

    private IEnumerator FadeCoroutine()
    {
        while (true)
        {
            yield return ChangeAlphaOverTime(maxAlpha, fadeSpeed);

            yield return new WaitForSeconds(fadeInterval / 2);

            yield return ChangeAlphaOverTime(minAlpha, fadeSpeed);

            yield return new WaitForSeconds(fadeInterval / 2);
        }
    }

    private IEnumerator ChangeAlphaOverTime(float target, float lerpSpeed)
    {
        while(Mathf.Abs(spriteRenderer.color.a - target) > 0.01f)
        {
            float newAlpha = Mathf.Lerp(spriteRenderer.color.a, target, lerpSpeed * Time.deltaTime);

            spriteRenderer.color = new Color(spriteRenderer.color.r,
                                             spriteRenderer.color.g,
                                             spriteRenderer.color.b,
                                             newAlpha);

            yield return null;
        }

        spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, target);
    }
}
