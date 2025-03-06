using Photon.Pun;
using System.Collections.Generic;
using UnityEngine;
using TwinTravelers.Core.Utility;
using TwinTravelers.Core.Network;
using TwinTravelers.Core.Actor;
using TwinTravelers.Management;

namespace TwinTravelers.Core.Gimmicks
{
    public class Teleport : InteractableTrigger
    {
        [Header("Preset")]
        [SerializeField] private Teleport target;

        [Header("Prefab")]
        [SerializeField] private GameObject teleportEffect;

        // private value..
        private HashSet<GameObject> recents = new HashSet<GameObject>();

        protected override void EnterEvent(Collider2D collision)
        {
            if (recents.Contains(collision.gameObject))
            {
                return;
            }

            switch (RoomManager.Instance.playmode)
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
                    break;

                case RoomManager.Playmode.Single:
                    collision.transform.position = target.transform.position;
                    break;
            }

            target.recents.Add(collision.gameObject);

            AudioManager.Instance.PlaySFX("Teleport");

            Instantiate(teleportEffect, transform.position, Quaternion.identity);
            Instantiate(teleportEffect, target.transform.position, Quaternion.identity);
        }

        protected override void ExitEvent(Collider2D collision)
        {
            if (recents.Contains(collision.gameObject))
            {
                recents.Remove(collision.gameObject);
            }
        }
    }
}
