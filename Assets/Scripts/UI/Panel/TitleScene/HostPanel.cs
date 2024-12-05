using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;

using TMPro;
using WebSocketSharp;

public class HostPanel : Panel
{
    [SerializeField] private Panel parent;
    [SerializeField] private List<GameObject> disableList;
    [Space(10.0f)]
    [SerializeField] private TextMeshProUGUI text;

    public new void Enable()
    {
        base.Enable();

        StartCoroutine(MakeRoomAndShowRoomCode());
    }

    public void Connected()
    {
        Disable();
        parent.Disable();

        foreach (var item in disableList)
        {
            item.SetActive(false);
        }
    }

    public void LeaveRoom()
    {
        StopAllCoroutines();
        RoomManager.Instance.LeaveRoom();
    }

    public void SetText(string text)
    {
        this.text.text = text;
    }

    private IEnumerator MakeRoomAndShowRoomCode()
    {
        RoomManager.Instance.LeaveRoom();

        text.text = "���� ������ �ֽ��ϴ�...";

        while (PhotonNetwork.InRoom)
        {
            yield return new WaitForSeconds(0.1f);
        }

        bool send = RoomManager.Instance.CreateRoom();

        if (send == false)
        {
            text.text = "������ �߻��Ͽ����ϴ�.\r\n��� �� �ٽ� �õ����ּ���.";
            yield break;
        }

        text.text = $"������ �����ϰ� �ֽ��ϴ�...";

        yield break;
    }
}
