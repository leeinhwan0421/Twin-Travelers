using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using TwinTravelers.Core.Network;

namespace TwinTravelers.UI
{
    public class JoinPanel : Panel
    {
        [SerializeField] private Panel parent;
        [SerializeField] private List<GameObject> disableList;
        [Space(10.0f)]
        [SerializeField] private GameObject loadObject;   // ���� ������ �ִ� ���߿� ��µ� Object
        [SerializeField] private GameObject loadedObject; // ���� ������ �� �� ��µ� Object
        [Space(10.0f)]
        [SerializeField] private TextMeshProUGUI errorText;

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

            WriteErrorText("");

            loadObject.SetActive(false);
            loadedObject.SetActive(true);
        }

        public void UpperRoomCode(TMP_InputField field)
        {
            field.text = field.text.ToUpper();
        }

        public void JoinRoom(TMP_InputField field)
        {
            if (field.text.Length == RoomManager.Instance.roomCodeLength)
            {
                RoomManager.Instance.JoinRoom(field.text);
                return;
            }

            WriteErrorText("�� �ڵ�� 6�ڸ��� �̷���� �ֽ��ϴ�.");
        }

        public void CloseJoinPanel()
        {
            Disable();
            parent.Disable();

            foreach (var item in disableList)
            {
                item.SetActive(false);
            }
        }

        public void WriteErrorText(string text)
        {
            errorText.text = text;
        }
    }
}
