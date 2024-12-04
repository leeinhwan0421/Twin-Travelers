using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;

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

    protected override void EnterEvent(Collider2D collision)
    {
        if (!isActivate)
            return;

        isActivate = false;

        switch (RoomManager.Instance.playmode)
        {
            case RoomManager.Playmode.Multi:

                if (TryGetComponent<PhotonView>(out PhotonView photonView))
                {
                    photonView.RPC("EarnedCoin", RpcTarget.All);

                    if (photonView.IsMine)
                    {
                        PhotonNetwork.Destroy(gameObject);
                    }
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
