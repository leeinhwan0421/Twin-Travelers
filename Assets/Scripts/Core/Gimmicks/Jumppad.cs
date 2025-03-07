using TwinTravelers.Core.Actor;
using UnityEngine;
using TwinTravelers.Core.Utility;
using TwinTravelers.Management;

namespace TwinTravelers.Core.Gimmicks
{
    /// <summary>
    /// 트리거에 충돌 시, 플레이어의 점프력을 증가시키는 클래스
    /// </summary>
    public class Jumppad : InteractableTrigger
    {
        /// <summary>
        /// 점프 패드의 점프력 배수
        /// </summary>
        [Header("Preset")]
        [Tooltip("점프 패드의 점프력 배수")]
        [Range(1.0f, 5.0f), SerializeField] 
        private float jumpMultiplier = 1.0f;

        protected override void EnterEvent(Collider2D collision)
        {
            AudioManager.Instance.PlaySFX("Jumppad");
            collision.GetComponent<Player>().jumpMultiplier = this.jumpMultiplier;
        }

        protected override void ExitEvent(Collider2D collision)
        {
            collision.GetComponent<Player>().jumpMultiplier = 1.0f;
        }
    }
}
