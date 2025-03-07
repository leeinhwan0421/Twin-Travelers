using UnityEngine;
using Photon.Pun;
using TwinTravelers.Core.Utility;
using TwinTravelers.Core.Network;
using TwinTravelers.Management;

namespace TwinTravelers.Core.Gimmicks
{
    public class Coin : InteractableTrigger
    {
        #region Fields
        /// <summary>
        /// 획득 이펙트
        /// </summary>
        [Header("Preset")]
        [Tooltip("획득 이펙트")]
        [SerializeField]
        private GameObject earnEffect;

        /// <summary>
        /// 코인이 활성화되었는지 여부
        /// </summary>
        private bool isActivate = true;
        #endregion

        #region Methods
        /// <summary>
        /// 획득 이펙트를 생성합니다.
        /// </summary>
        private void InstantiateEarnEffect()
        {
            Instantiate(earnEffect, transform.position, Quaternion.identity);
        }

        /// <summary>
        /// 코인을 모든 클라이언트에서 획득합니다.
        /// </summary>
        [PunRPC]
        private void EarnedCoin()
        {
            GameManager.Instance.EarnCoin(1);
            AudioManager.Instance.PlaySFX("earnedCoin");

            InstantiateEarnEffect();
        }

        /// <summary>
        /// 코인을 모든 클라이언트에서 파괴합니다.
        /// </summary>
        [PunRPC]
        private void DestroyCoin()
        {
            if (PhotonNetwork.IsMasterClient)
            {
                PhotonNetwork.Destroy(gameObject);
            }
        }

        protected override void EnterEvent(Collider2D collision)
        {
            if (!isActivate)
                return;

            isActivate = false;

            switch (RoomManager.Instance.playmode)
            {
                case Playmode.Multi:
                    if (!collision.gameObject.TryGetComponent<PhotonView>(out PhotonView playerPhotonView))
                        return;

                    if (!playerPhotonView.IsMine)
                        return;

                    if (TryGetComponent<PhotonView>(out PhotonView photonView))
                    {
                        photonView.RPC("EarnedCoin", RpcTarget.AllBuffered);
                        photonView.RPC("DestroyCoin", RpcTarget.MasterClient);
                    }
                    break;
                case Playmode.Single:
                    EarnedCoin();
                    Destroy(gameObject);
                    break;
            }
        }

        protected override void ExitEvent(Collider2D collision)
        {
            // Nothing
        }
        #endregion
    }
}
