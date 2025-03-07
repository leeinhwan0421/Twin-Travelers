using UnityEngine;
using TwinTravelers.Core.Utility;

namespace TwinTravelers.Core.Gimmicks
{
    /// <summary>
    /// 플랫폼에 닿았을 때, 플레이어를 부모로 설정하는 클래스
    /// </summary>
    public class PlatformAttachment : InteractableCollision
    {
        protected override void EnterEvent(Collision2D collision)
        {
            collision.transform.parent = transform;
        }

        protected override void ExitEvent(Collision2D collision)
        {
            collision.transform.parent = null;
        }
    }
}
