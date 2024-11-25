using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using TMPro;
using Photon.Pun;

public class JoinPanel : Panel
{
    [SerializeField] private Panel parent;
    [SerializeField] private List<GameObject> disableList;
    [Space(10.0f)]
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

    public void JoinRoom(TMP_InputField field)
    {
        if (field.text.Length < RoomManager.Instance.roomCodeLength)
        {
            // 자릿수
        }

        bool Joined = RoomManager.Instance.JoinRoom(field.text);

        if (Joined)
        {
            Disable();
            parent.Disable();
            
            foreach(var item in disableList)
            {
                item.SetActive(false);
            }
        }
        else
        {
            // other
        }
    }
}
