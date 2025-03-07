using UnityEngine;
using TwinTravelers.Core.Utility;
using TwinTravelers.Management;

namespace TwinTravelers.Core.Gimmicks
{
    /// <summary>
    /// 장애물을 담당하는 클래스
    /// </summary>
    public class Obstacle : InteractableTrigger
    {
        protected override void EnterEvent(Collider2D collision)
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.DefeatStage();
            }
        }

        protected override void ExitEvent(Collider2D collision)
        {
            // Nothing!
        }
    }
}
