using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StageObject : MonoBehaviour
{
    [Header("Preset")]
    [SerializeField] private Sprite unlockSprite;
    [SerializeField] private Sprite lockSprite;

    [Header("Child")]
    [SerializeField] private GameObject closeIcon;

    [Header("Star")]
    [SerializeField] private GameObject starGroup;
    [SerializeField] private List<GameObject> fill;

    [Header("Text")]
    [SerializeField] private TextMeshProUGUI stageNumber;

    private Stage stage;
    private string sceneName;

    public void SetStageObject(Stage stage, string sceneName)
    {
        this.stage = stage;
        this.sceneName = sceneName;

        if (stage.isUnlocked)
        {
            Unlock();
        }
        else
        {
            Lock();
        }
    }

    private void Unlock()
    {
        GetComponent<Image>().sprite = unlockSprite;

        stageNumber.text = stage.stageName.Split(' ')[1];
        stageNumber.gameObject.SetActive(true);

        if (stage.starCount == 0)
        {
            starGroup.SetActive(false);
        }
        else
        {
            starGroup.SetActive(true);

            for (int i = 0; i < stage.starCount; i++)
            {
                fill[i].SetActive(true);
            }
        }
    }

    private void Lock()
    {
        GetComponent<Image>().sprite = lockSprite;
        closeIcon.SetActive(true);
        starGroup.SetActive(false);
    }

    public void OnClick()
    {
        if (!stage.isUnlocked)
            return;

        LoadSceneManager.LoadScene(sceneName);
    }
}
