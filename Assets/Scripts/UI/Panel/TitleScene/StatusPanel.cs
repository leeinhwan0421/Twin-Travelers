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
                text.text = errorText; // ����
                break;
            case RoomManager.Playmode.Single:
                text.text = singleText; // ���������̰ų� �̱� �÷���
                break;
            case RoomManager.Playmode.Multi:
                text.text = $"{multiText}\n���� ��ȣ: {PhotonNetwork.CurrentRoom.Name}";
                break;
        }
    }
}
