using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class OfflinePanel : Panel
{
    [SerializeField] private GameObject networkPanel;
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private float timeToGoTitle = 3.0f;

    public new void Enable()
    {
        base.Enable();

        StartCoroutine(UpdateCoroutine());
    }

    public new void Disable()
    {
        base.Disable();

        StopAllCoroutines();
    }

    private IEnumerator UpdateCoroutine()
    {
        float timeRemaining = timeToGoTitle;

        while (timeRemaining > 0)
        {
            text.text = $"�������� ���� ��ȯ�Ǿ����ϴ�.\r\n{Mathf.RoundToInt(timeRemaining).ToString()} �� �� Ÿ��Ʋ�� ���ư��ϴ�.\r\n���ͳݿ� ���� �� ���\r\n�����ϱ� ����� ����� �� �ֽ��ϴ�.";

            yield return new WaitForSeconds(1.0f);

            timeRemaining -= 1.0f;
        }

        LoadSceneManager.LoadScene("TitleScene");

        networkPanel.SetActive(false);
        Disable();
    }
}
