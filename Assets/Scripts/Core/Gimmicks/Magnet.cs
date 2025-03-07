using System.Collections.Generic;
using UnityEngine;
using TwinTravelers.Core.Utility;

namespace TwinTravelers.Core.Gimmicks
{
    /// <summary>
    /// 자석 클래스
    /// </summary>
    public class Magnet : InteractableTrigger
    {
        #region Fields
        /// <summary>
        /// 자석의 효과를 적용할 속도
        /// </summary>
        [Header("Preset")]
        [Tooltip("자석의 효과를 적용할 속도")]
        [Range(0.01f, 10.0f), SerializeField]
        private float pullSpeed;

        /// <summary>
        /// 자석의 효과를 적용할 게임 오브젝트 집합
        /// </summary>
        private HashSet<GameObject> colls = new HashSet<GameObject>();
        #endregion

        #region Unity Methods
        private void Update()
        {
            foreach (var coll in colls)
            {
                Vector3 dir = transform.position - coll.transform.position;

                if (coll.TryGetComponent<Rigidbody2D>(out Rigidbody2D rigid))
                {
                    rigid.velocity = Vector2.zero;
                }

                coll.transform.position += pullSpeed * Time.deltaTime * dir.normalized;
            }
        }
        #endregion

        #region Methods
        protected override void EnterEvent(Collider2D collision)
        {
            colls.Add(collision.gameObject);
        }

        protected override void ExitEvent(Collider2D collision)
        {
            colls.Remove(collision.gameObject);
        }
        #endregion
    }
}