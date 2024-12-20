using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;
using Photon.Pun;

public class StatusPanel : Panel
{
    [SerializeField] private string errorText;
    [SerializeField] private string singleText;
    [SerializeField] private string multiText;
    [Space(10.0f)]
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private GameObject button;

    public new void Enable()
    {
        base.Enable();

        switch (RoomManager.Instance.playmode)
        {
            case RoomManager.Playmode.None: // 에러
                button.SetActive(false);
                text.text = errorText; 
                break;
            case RoomManager.Playmode.Single: // 오프라인이거나 싱글 플레이
                button.SetActive(false);
                text.text = singleText;
                break;
            case RoomManager.Playmode.Multi: // 멀티 플레이 중
                button.SetActive(true);
                text.text = $"{multiText}\n게임 번호: {PhotonNetwork.CurrentRoom.Name}";
                break;
        }
    }

    public void LeaveRoom()
    {
        RoomManager.Instance.LeaveRoom();
    }
}
