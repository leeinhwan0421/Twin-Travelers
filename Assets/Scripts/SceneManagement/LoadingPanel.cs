using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoadingPanel : MonoBehaviour
{
    [Header("Element")]
    public Image progressBar;
    public TextMeshProUGUI loadingText;
    public TextMeshProUGUI TipText;

    [Header("Loading Preset")]
    [SerializeField] private List<string> cycleText;
    [SerializeField] private float cycleInterval = 0.5f;
    private int cycleIndex = 0;

    private void Start()
    {
        TipText.text = LoadTipsFromFile();

        StartCoroutine(CycleLoadingText());
    }

    private IEnumerator CycleLoadingText()
    {
        while (true)
        {
            loadingText.text = cycleText[cycleIndex];
            cycleIndex = (cycleIndex + 1) % cycleText.Count;
            yield return new WaitForSeconds(cycleInterval);
        }
    }

    private string LoadTipsFromFile()
    {
        TextAsset tip = Resources.Load<TextAsset>("Tips");

        if (tip != null)
        {
            string[] tips = tip.text.Split("\n");

            return tips[Random.Range(0, tips.Length)];
        }
        else
        {
            return "No tips Available";
        }
    }
}
