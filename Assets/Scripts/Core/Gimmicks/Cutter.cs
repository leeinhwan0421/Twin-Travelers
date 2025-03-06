using UnityEngine;
using TwinTravelers.Core.Actor;
using TwinTravelers.Core.Utility;

namespace TwinTravelers.Core.Gimmicks
{
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
