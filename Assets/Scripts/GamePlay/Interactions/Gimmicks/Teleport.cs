using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Teleport : InteractableTrigger
{
    [Header("Preset")]
    [SerializeField] private Teleport target;

    [Header("Prefab")]
    [SerializeField] private GameObject teleportEffect;

    // private value..
    private HashSet<GameObject> recents = new HashSet<GameObject>();

    private void Awake()
    {
        if (RoomManager.Instance.playmode != RoomManager.Playmode.Multi)
        {
            return;
        }

        PhotonView photonView = GetComponent<PhotonView>();

        if (PhotonNetwork.IsMasterClient && photonView.ViewID <= 0)
        {
            photonView.ViewID = PhotonNetwork.AllocateViewID(true);
        }
    }

    protected override void EnterEvent(Collider2D collision)
    {
        if (recents.Contains(collision.gameObject))
        {
            return;
        }

        switch(RoomManager.Instance.playmode)
        {
            case RoomManager.Playmode.Multi:
                var photonView = collision.GetComponent<PhotonView>();

                if (collision.CompareTag("Player"))
                {
                    collision.GetComponent<Player>().MoveToPosition(target.transform.position);
                }
                else
                {
                    collision.transform.position = target.transform.position;
                }

                target.GetComponent<PhotonView>().RPC("AddRecent", RpcTarget.All, photonView.ViewID);
                break;

            case RoomManager.Playmode.Single:
                collision.transform.position = target.transform.position;
                target.recents.Add(collision.gameObject);
                break;
        }

        AudioManager.Instance.PlaySFX("Teleport");

        Instantiate(teleportEffect, transform.position, Quaternion.identity);
        Instantiate(teleportEffect, target.transform.position, Quaternion.identity);
    }

    protected override void ExitEvent(Collider2D collision)
    {
        var photonView = collision.GetComponent<PhotonView>();

        switch (RoomManager.Instance.playmode)
        {
            case RoomManager.Playmode.Multi:
                GetComponent<PhotonView>().RPC("RemoveRecent", RpcTarget.All, photonView.ViewID);
                break;
            case RoomManager.Playmode.Single:
                recents.Remove(collision.gameObject);
                break;
        }
    }

    [PunRPC]
    private void AddRecent(int viewID)
    {
        var obj = PhotonView.Find(viewID)?.gameObject;
        if (obj != null)
        {
            recents.Add(obj);
        }
    }

    [PunRPC]
    private void RemoveRecent(int viewID)
    {
        var obj = PhotonView.Find(viewID)?.gameObject;
        if (recents.Contains(obj))
        {
            recents.Remove(obj);
        }
    }
}
