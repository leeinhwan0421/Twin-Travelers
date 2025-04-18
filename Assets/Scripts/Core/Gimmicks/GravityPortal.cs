using System;
using UnityEngine;
using TwinTravelers.Core.Actor;
using TwinTravelers.Core.Utility;

namespace TwinTravelers.Core.Gimmicks
{
    /// <summary>
    /// 중력을 변경하는 포탈 클래스
    /// </summary>
    public class GravityPortal : InteractableTrigger
    {
        [Serializable]
        public enum GravityPortalType
        {
            Up,
            Down
        }

        #region Fields
        /// <summary>
        /// 변경할 중력 방향
        /// </summary>
        [Header("Preset")]
        [Tooltip("변경할 중력 방향")]
        [SerializeField] 
        private GravityPortalType type;
        #endregion

        #region Methods
        protected override void EnterEvent(Collider2D collision)
        {
            if (collision.TryGetComponent<Player>(out Player playerUp))
            {
                playerUp.ChangeGravity(type);

                return;
            }

            if (collision.TryGetComponent<Rigidbody2D>(out Rigidbody2D rigid))
            {
                switch (type)
                {
                    case GravityPortalType.Up: // -9.81f, reverse
                        rigid.gravityScale = -Math.Abs(rigid.gravityScale);
                        collision.transform.rotation = Quaternion.Euler(180.0f, 0.0f, 0.0f);
                        break;

                    case GravityPortalType.Down:
                        rigid.gravityScale = Math.Abs(rigid.gravityScale);
                        collision.transform.rotation = Quaternion.identity;
                        break;
                }
                return;
            }
        }

        protected override void ExitEvent(Collider2D collision)
        {
            // Nothing
        }
        #endregion
    }
}
