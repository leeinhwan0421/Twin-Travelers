using UnityEngine;
using TwinTravelers.Core.Utility;

namespace TwinTravelers.Core.Gimmicks
{
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
