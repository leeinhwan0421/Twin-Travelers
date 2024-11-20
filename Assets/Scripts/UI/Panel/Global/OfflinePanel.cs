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
            text.text = $"오프라인 모드로 전환되었습니다.\r\n{Mathf.RoundToInt(timeRemaining).ToString()} 초 뒤 타이틀로 돌아갑니다.\r\n인터넷에 연결 될 경우\r\n같이하기 기능을 사용할 수 있습니다.";

            yield return new WaitForSeconds(1.0f);

            timeRemaining -= 1.0f;
        }

        LoadSceneManager.LoadScene("TitleScene");

        networkPanel.SetActive(false);
        Disable();
    }
}
