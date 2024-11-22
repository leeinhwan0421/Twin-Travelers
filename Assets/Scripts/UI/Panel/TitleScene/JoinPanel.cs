using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WebSocketSharp;

public class JoinPanel : Panel
{
    [SerializeField] private GameObject loadObject;   // 방을 나가고 있는 도중에 출력될 Object
    [SerializeField] private GameObject loadedObject; // 방을 나가고 난 뒤 출력될 Object

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
