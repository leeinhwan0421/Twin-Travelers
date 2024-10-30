using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("Timer")]
    [SerializeField] private TextMeshProUGUI timeText;

    [Header("Coins")]
    [SerializeField] private TextMeshProUGUI earnedCoinText;

    [Header("UI Element")]
    [SerializeField] private GameObject resultBackgroundPanel;
    [SerializeField] private DefeatPanel defeatPanel;
    [SerializeField] private VictoryPanel victoryPanel;
    [SerializeField] private PausePanel pausePanel;

    // Properties
    public GameObject ResultBackgroundPanel { get {return resultBackgroundPanel; } }
    public DefeatPanel DefeatPanel { get { return defeatPanel; } }
    public VictoryPanel VictoryPanel { get { return victoryPanel; } }
    public PausePanel PausePanel { get { return pausePanel; } }


    public void SetTimeText(float playTime)
    {
        int min = (int)(playTime / 60.0f);
        int sec = (int)(playTime % 60.0f);

        timeText.text = $"{min:00} : {sec:00}";
    }

    public void SetEarnedCoinText(int earnedCoinCount)
    {
        earnedCoinText.text = $"{earnedCoinCount}";
    }
}
