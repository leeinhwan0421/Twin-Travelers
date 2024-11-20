using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using TMPro;

public class StageAllowPanel : MonoBehaviour
{
    [Header("Preset")]
    [SerializeField] private TextMeshProUGUI text;

    private void Awake()
    {
        text.text = SceneManager.GetActiveScene().name;
    }

    private void StartGame()
    {
        GameManager.Instance.InitializeStage();

        gameObject.SetActive(false);
    }
}
