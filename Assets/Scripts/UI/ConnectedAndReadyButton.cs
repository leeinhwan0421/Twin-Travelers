using System.Collections;
using UnityEngine;
using Photon.Pun;

namespace TwinTravelers.UI
{
    /// <summary>
    /// 플레이어가 연결되어 있고 준비가 되었을 때만 상호작용 가능한 버튼
    /// </summary>
    public class ConnectedAndReadyButton : MonoBehaviour
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
                button.interactable = PhotonNetwork.IsConnectedAndReady;

                yield return new WaitForSecondsRealtime(0.25f);
            }
        }
    }
}
