using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using TwinTravelers.Core.Network;

namespace TwinTravelers.UI
{
    /// <summary>
    /// 참가 패널
    /// </summary>
    public class JoinPanel : Panel
    {
        #region Fields
        /// <summary>
        /// 부모 패널
        /// </summary>
        [Tooltip("부모 패널")]
        [SerializeField] 
        private Panel parent;

        /// <summary>
        /// 비활성화 리스트
        /// </summary>
        [Tooltip("비활성화 리스트")]
        [SerializeField]
        private List<GameObject> disableList;

        /// <summary>
        /// 방을 나가고 있을 동안 출력될 오브젝트
        /// </summary>
        [Space(10.0f)]
        [Tooltip("방을 나가고 있을 동안 출력될 오브젝트")]
        [SerializeField]
        private GameObject loadObject;

        /// <summary>
        /// 방을 나가고 난 뒤 출력될 오브젝트
        /// </summary>
        [Tooltip("방을 나가고 난 뒤 출력될 오브젝트")]
        [SerializeField]
        private GameObject loadedObject;

        /// <summary>
        /// 오류 텍스트
        /// </summary>
        [Space(10.0f)]
        [Tooltip("오류 텍스트")]
        [SerializeField]
        private TextMeshProUGUI errorText;
        #endregion

        #region Methods
        /// <summary>
        /// 패널을 활성화합니다.
        /// </summary>
        public new void Enable()
        {
            base.Enable();

            StartCoroutine(MakeRoomAndShowRoomCode());
        }

        /// <summary>
        /// 방을 만들고 방 코드를 출력합니다.
        /// </summary>
        /// <returns>IEnumeratorreturns>
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

        /// <summary>
        /// 방 코드를 대문자로 변경합니다.
        /// </summary>
        /// <param name="field"></param>
        public void UpperRoomCode(TMP_InputField field)
        {
            field.text = field.text.ToUpper();
        }

        /// <summary>
        /// 방에 참가합니다.
        /// </summary>
        /// <param name="field"></param>
        public void JoinRoom(TMP_InputField field)
        {
            if (field.text.Length == RoomManager.Instance.roomCodeLength)
            {
                RoomManager.Instance.JoinRoom(field.text);
                return;
            }

            WriteErrorText("방 코드는 6자리로 이루어져 있습니다.");
        }

        /// <summary>
        /// 패널을 비활성화합니다.
        /// </summary>
        public void CloseJoinPanel()
        {
            Disable();
            parent.Disable();

            foreach (var item in disableList)
            {
                item.SetActive(false);
            }
        }

        /// <summary>
        /// 오류 텍스트를 출력합니다.
        /// </summary>
        /// <param name="text">출력할 텍스트</param>
        public void WriteErrorText(string text)
        {
            errorText.text = text;
        }
        #endregion
    }
}
