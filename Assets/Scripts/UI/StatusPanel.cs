using UnityEngine;
using Photon.Pun;
using TMPro;
using TwinTravelers.Core.Network;

namespace TwinTravelers.UI
{
    /// <summary>
    /// 현재 플레이 상태를 보여주는 패널
    /// </summary>
    public class StatusPanel : Panel
    {
        #region Fields
        /// <summary>
        /// 에러 텍스트
        /// </summary>
        [Tooltip("에러 텍스트")]
        [SerializeField]
        private string errorText;

        /// <summary>
        /// 싱글 플레이 텍스트
        /// </summary>
        [Tooltip("싱글 플레이 텍스트")]
        [SerializeField]
        private string singleText;

        /// <summary>
        /// 멀티 플레이 텍스트
        /// </summary>
        [Tooltip("멀티 플레이 텍스트")]
        [SerializeField]
        private string multiText;

        /// <summary>
        /// 패널 내부 텍스트
        /// </summary>
        [Space(10.0f)]
        [Tooltip("패널 내부 텍스트")]
        [SerializeField]
        private TextMeshProUGUI text;

        /// <summary>
        /// 방 나가기 버튼
        /// </summary>
        [Tooltip("방 나가기 버튼")]
        [SerializeField]
        private GameObject button;
        #endregion

        #region Methods
        /// <summary>
        /// 패널을 활성화합니다.
        /// </summary>
        public new void Enable()
        {
            base.Enable();

            switch (RoomManager.Instance.playmode)
            {
                case Playmode.None: // 에러
                    button.SetActive(false);
                    text.text = errorText;
                    break;
                case Playmode.Single: // 오프라인이거나 싱글 플레이
                    button.SetActive(false);
                    text.text = singleText;
                    break;
                case Playmode.Multi: // 멀티 플레이 중
                    button.SetActive(true);
                    text.text = $"{multiText}\n게임 번호: {PhotonNetwork.CurrentRoom.Name}";
                    break;
            }
        }

        /// <summary>
        /// 방을 나갑니다.
        /// </summary>
        public void LeaveRoom()
        {
            RoomManager.Instance.LeaveRoom();
        }
        #endregion
    }
}
