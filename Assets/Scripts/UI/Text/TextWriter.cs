using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextWriter : MonoBehaviour
{
    [Header("Preset")]
    [Range(0.1f, 1.0f)][SerializeField] private float typingInterval;

    private TextMeshProUGUI text;

    private void Awake()
    {
        text = GetComponent<TextMeshProUGUI>();
    }

    private void OnDisable()
    {
        text.text = "";
    }

    public void WriteText(string text)
    {
        StopAllCoroutines();

        StartCoroutine(WriteTextCoroutine(text));
    }

    private IEnumerator WriteTextCoroutine(string text)
    {
        this.text.text = "";

        for (int i = 0; i < text.Length; i++)
        {
            this.text.text += text[i];
            yield return new WaitForSeconds(typingInterval);
        }

        this.text.text = text;
    }
}
