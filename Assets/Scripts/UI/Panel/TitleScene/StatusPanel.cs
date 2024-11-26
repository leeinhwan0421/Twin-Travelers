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

    public new void Enable()
    {
        base.Enable();

        switch (RoomManager.Instance.playmode)
        {
            case RoomManager.Playmode.None:
                text.text = errorText; // 에러
                break;
            case RoomManager.Playmode.Single:
                text.text = singleText; // 오프라인이거나 싱글 플레이
                break;
            case RoomManager.Playmode.Multi:
                text.text = $"{multiText}\n게임 번호: {PhotonNetwork.CurrentRoom.Name}";
                break;
        }
    }
}
