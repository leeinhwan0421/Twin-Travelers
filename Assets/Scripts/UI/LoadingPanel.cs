using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TwinTravelers.UI
{
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
                yield return new WaitForSecondsRealtime(cycleInterval);
            }
        }

        private string LoadTipsFromFile()
        {
            string tip = Encoding.UTF8.GetString(Resources.Load<TextAsset>("Tips").bytes);

            if (tip != null)
            {
                string[] tips = tip.Split("\n");

                return tips[Random.Range(0, tips.Length)];
            }
            else
            {
                return "No tips Available";
            }
        }
    }
}
