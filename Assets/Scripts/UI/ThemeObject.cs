using System.Collections.Generic;
using TMPro;
using UnityEngine;
using TwinTravelers.Level;

namespace TwinTravelers.UI
{
    public class ThemeObject : MonoBehaviour
    {
        [Header("Theme")]
        [SerializeField] private TextMeshProUGUI themename;

        [Header("Stage")]
        [SerializeField] private GameObject stageGroup;
        [SerializeField] private GameObject stagePrefab;

        public void SetThemeName(string name)
        {
            themename.text = name;
        }

        public void SetStages(List<Stage> stages)
        {
            for (int i = 0; i < stages.Count; i++)
            {
                GameObject stage = Instantiate(stagePrefab, stageGroup.transform);
                StageObject stageObject = stage.GetComponent<StageObject>();

                stageObject.SetStageObject(stages[i], $"{themename.text} {stages[i].stageName}");
            }
        }
    }
}
