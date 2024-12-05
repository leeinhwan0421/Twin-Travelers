using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public sealed class VictoryPanel : Panel
{
    [Header("Preset")]
    [SerializeField] private List<GameObject> stars;
    [SerializeField] private TextMeshProUGUI earnedCoin;

    public void SetEarnedCoinText(int earnedCoin)
    {
        this.earnedCoin.text = $"{earnedCoin.ToString()}";
    }

    public void SetDisplayStarCount(int count)
    {
        if (stars.Count < count)
        {
#if UNITY_EDITOR
            Debug.Log($"SetDisplayStarCount called with an invalid parameter: {count.ToString()}");
#endif
            return;
        }

        for (int i = 0; i < stars.Count; i++)
        {
            stars[i].SetActive(false);
        }

        for (int i = 0; i < count; i++)
        {
            stars[i].SetActive(true);
        }
    }

    public void RestartGame()
    {
        GameManager.Instance.RestartStage();
    }
}
