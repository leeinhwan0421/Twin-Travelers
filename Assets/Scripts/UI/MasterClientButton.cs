using System.Collections;
using UnityEngine;
using Photon.Pun;
using TwinTravelers.Core.Network;

namespace TwinTravelers.UI
{
    /// <summary>
    /// 마스터 클라이언트만 상호작용 가능한 버튼
    /// </summary>
    public class MasterClientButton : MonoBehaviour
    {
        /// <summary>
        /// 클릭할 수 있는 버튼
        /// </summary>
        private SoundButton button;

        private void OnEnable()
        {
            button = GetComponent<SoundButton>();
            StartCoroutine(Coroutine());
        }

        private void OnDisable()
        {
            StopAllCoroutines();
        }

        /// <summary>
        /// 버튼 상호작용 가능 여부 코루틴
        /// </summary>
        /// <returns>IEnumerator</returns>
        private IEnumerator Coroutine()
        {
            while (true)
            {
                if (RoomManager.Instance.playmode == Playmode.Single)
                {
                    button.interactable = true;
                }
                else
                {
                    button.interactable = PhotonNetwork.IsMasterClient;
                }

                yield return new WaitForSecondsRealtime(0.25f);
            }
        }
    }
}
