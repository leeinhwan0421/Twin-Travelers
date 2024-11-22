using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;

using TMPro;
using WebSocketSharp;

public class HostPanel : Panel
{
    [SerializeField] private TextMeshProUGUI text;

    public new void Enable()
    {
        base.Enable();

        StartCoroutine(MakeRoomAndShowRoomCode());
    }

    public void LeaveRoom()
    {
        StopAllCoroutines();
        RoomManager.Instance.LeaveRoom();
    }

    private IEnumerator MakeRoomAndShowRoomCode()
    {
        RoomManager.Instance.LeaveRoom();

        text.text = "���� ������ �ֽ��ϴ�...";

        while (PhotonNetwork.InRoom)
        {
            yield return new WaitForSeconds(0.1f);
        }

        string roomCode = RoomManager.Instance.CreateRoom();

        if (roomCode.IsNullOrEmpty())
        {
            text.text = "������ �߻��Ͽ����ϴ�. �ٽ� �õ����ּ���.";
            yield break;
        }

        text.text = $"���� ���� ��ȣ�� [{roomCode}] �Դϴ�.";
    }
}
