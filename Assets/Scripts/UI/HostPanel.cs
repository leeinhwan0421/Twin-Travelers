using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using TwinTravelers.Core.Network;

namespace TwinTravelers.UI
{
    /// <summary>
    /// 호스트 패널
    /// </summary>
    public class HostPanel : Panel
    {
        #region Fields
        /// <summary>
        /// 부모 패널
        /// </summary>
        [Tooltip("부모 패널")]
        [SerializeField]
        private Panel parent;

        /// <summary>
        /// 비활성화할 오브젝트 리스트
        /// </summary>
        [Tooltip("비활성화할 오브젝트 리스트")]
        [SerializeField]
        private List<GameObject> disableList;

        /// <summary>
        /// 텍스트
        /// </summary>
        [Space(10.0f)]
        [Tooltip("표시할 텍스트")]
        [SerializeField] 
        private TextMeshProUGUI text;
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
        /// 패널을 비활성화합니다.
        /// </summary>
        public void Connected()
        {
            Disable();
            parent.Disable();

            foreach (var item in disableList)
            {
                item.SetActive(false);
            }
        }

        /// <summary>
        /// 텍스트를 설정합니다.
        /// </summary>
        /// <param name="text">Draw할 텍스트</param>
        public void SetText(string text)
        {
            this.text.text = text;
        }

        /// <summary>
        /// 방을 생성하고 방 코드를 표시합니다.
        /// </summary>
        /// <returns>IEnumerator</returns>
        private IEnumerator MakeRoomAndShowRoomCode()
        {
            RoomManager.Instance.LeaveRoom();

            text.text = "방을 나가고 있습니다...";

            while (PhotonNetwork.InRoom)
            {
                yield return new WaitForSeconds(0.1f);
            }

            bool send = RoomManager.Instance.CreateRoom();

            if (send == false)
            {
                text.text = "오류가 발생하였습니다.\r\n잠시 뒤 다시 시도해주세요.";
                yield break;
            }

            text.text = $"게임을 생성하고 있습니다...";

            yield break;
        }
        #endregion
    }
}
