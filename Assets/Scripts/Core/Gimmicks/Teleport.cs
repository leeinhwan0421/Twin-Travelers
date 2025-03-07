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
        #region Fields
        /// <summary>
        /// 이동할 위치
        /// </summary>
        [Header("Preset")]
        [Tooltip("이동할 위치")]
        [SerializeField] 
        private Teleport target;

        /// <summary>
        /// 이동 이펙트
        /// </summary>
        [Header("Prefab")]
        [Tooltip("이동 이펙트")]
        [SerializeField] 
        private GameObject teleportEffect;

        /// <summary>
        /// 최근에 이동한 오브젝트
        /// </summary>
        private HashSet<GameObject> recents = new HashSet<GameObject>();
        #endregion

        protected override void EnterEvent(Collider2D collision)
        {
            if (recents.Contains(collision.gameObject))
            {
                return;
            }

            switch (RoomManager.Instance.playmode)
            {
                case Playmode.Multi:
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

                case Playmode.Single:
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
