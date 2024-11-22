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

        text.text = "방을 나가고 있습니다...";

        while (PhotonNetwork.InRoom)
        {
            yield return new WaitForSeconds(0.1f);
        }

        string roomCode = RoomManager.Instance.CreateRoom();

        if (roomCode.IsNullOrEmpty())
        {
            text.text = "오류가 발생하였습니다. 다시 시도해주세요.";
            yield break;
        }

        text.text = $"게임 참가 번호는 [{roomCode}] 입니다.";
    }
}
