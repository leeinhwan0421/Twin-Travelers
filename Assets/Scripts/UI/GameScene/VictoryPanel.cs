using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public sealed class VictoryPanel : Panel
{
    [Header("preset")]
    [SerializeField] private List<GameObject> stars;
    [SerializeField] private TextMeshProUGUI earnedCoin;

    public void SetEarnedCoinText(int earnedCoin)
    {
        this.earnedCoin.text = $"{earnedCoin}";
    }

    public void SetDisplayStarCount(int count)
    {
        if (stars.Count < count)
        {
#if SHOW_DEBUG_MESSAGES
            Debug.Log($"SetDisplayStarCount called with an invalid parameter: {count}");
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
}
