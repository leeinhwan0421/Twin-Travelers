using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityPortal : InteractableTrigger
{
    [Serializable]
    public enum GravityPortalType
    {
        Up,
        Down
    }

    [Header("Preset")]
    [SerializeField] private GravityPortalType type;

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
}
