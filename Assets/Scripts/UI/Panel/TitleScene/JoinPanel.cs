using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WebSocketSharp;

public class JoinPanel : Panel
{
    [SerializeField] private GameObject loadObject;   // ���� ������ �ִ� ���߿� ��µ� Object
    [SerializeField] private GameObject loadedObject; // ���� ������ �� �� ��µ� Object

    public new void Enable()
    {
        base.Enable();

        StartCoroutine(MakeRoomAndShowRoomCode());
    }

    private IEnumerator MakeRoomAndShowRoomCode()
    {
        loadObject.SetActive(true);
        loadedObject.SetActive(false);

        RoomManager.Instance.LeaveRoom();

        while (PhotonNetwork.InRoom)
        {
            yield return new WaitForSeconds(0.1f);
        }

        loadObject.SetActive(false);
        loadedObject.SetActive(true);
    }
}
