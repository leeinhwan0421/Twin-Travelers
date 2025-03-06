using UnityEngine;
using TwinTravelers.Core.Actor;
using TwinTravelers.Core.Utility;

namespace TwinTravelers.Core.Gimmicks
{
    /// <summary>
    /// 트리거에 충돌 시, 플레이어의 풍선을 활성화 시키는 클래스
    /// </summary>
    public class BalloonTrigger : InteractableTrigger
    {
        protected override void EnterEvent(Collider2D collision)
        {
            Player player = collision.GetComponent<Player>();

            if (player == null || player.PlayerBalloon == null)
            {
                return;
            }

            player.PlayerBalloon.gameObject.SetActive(true);
        }

        protected override void ExitEvent(Collider2D collision)
        {
            // Nothing
        }
    }
}
