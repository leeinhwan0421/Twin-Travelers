using UnityEngine;
using Photon.Pun;
using TwinTravelers.Core.Utility;
using TwinTravelers.Core.Network;
using TwinTravelers.Management;

namespace TwinTravelers.Core.Gimmicks
{
    public class Coin : InteractableTrigger
    {
        [Header("Preset")]
        [SerializeField] private GameObject earnEffect;

        private bool isActivate = true;

        private void InstantiateEarnEffect()
        {
            Instantiate(earnEffect, transform.position, Quaternion.identity);
        }

        [PunRPC]
        private void EarnedCoin()
        {
            GameManager.Instance.EarnCoin(1);
            AudioManager.Instance.PlaySFX("earnedCoin");

            InstantiateEarnEffect();
        }

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
                case RoomManager.Playmode.Multi:
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
                case RoomManager.Playmode.Single:
                    EarnedCoin();
                    Destroy(gameObject);
                    break;
            }
        }

        protected override void ExitEvent(Collider2D collision)
        {
            // Nothing
        }
    }
}
