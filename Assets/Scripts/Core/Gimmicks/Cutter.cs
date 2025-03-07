using UnityEngine;
using TwinTravelers.Core.Actor;
using TwinTravelers.Core.Utility;

namespace TwinTravelers.Core.Gimmicks
{
    /// <summary>
    /// 트리거에 충돌 시, 플레이어의 풍선을 폭발시키는 클래스
    /// </summary>
    public class Cutter : InteractableTrigger
    {
        protected override void EnterEvent(Collider2D collision)
        {
            collision.GetComponent<PlayerBalloon>().ExplodeBalloon();
        }

        protected override void ExitEvent(Collider2D collision)
        {
            // Nothing
        }
    }
}
