using System.Collections.Generic;
using UnityEngine;
using TwinTravelers.Core.Utility;

namespace TwinTravelers.Core.Gimmicks
{
    /// <summary>
    /// 바람을 일으키는 선풍기
    /// </summary>
    public class Wind_Fan : InteractableTrigger
    {
        #region Fields
        /// <summary>
        /// 물체를 밀어내는 속도
        /// </summary>
        [Header("Presets")]
        [Tooltip("물체를 밀어내는 속도")]
        [Range(0.01f, 100.0f), SerializeField] 
        private float windSpeed;

        /// <summary>
        /// 바람 소리
        /// </summary>
        private AudioSource source;

        /// <summary>
        /// 충돌하고 있는 물체들
        /// </summary>
        private HashSet<GameObject> colls = new HashSet<GameObject>();
        #endregion

        #region Unity Methods
        private void Awake()
        {
            source = GetComponent<AudioSource>();
        }

        private void Update()
        {
            foreach (var obj in colls)
            {
                obj.transform.position += Time.deltaTime * windSpeed * transform.right;
                obj.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
            }
        }
        #endregion

        #region Methods
        protected override void EnterEvent(Collider2D collision)
        {
            colls.Add(collision.gameObject);

            if (colls.Count > 0)
            {
                source.Play();
            }
        }

        protected override void ExitEvent(Collider2D collision)
        {
            colls.Remove(collision.gameObject);

            if (colls.Count <= 0)
            {
                source.Stop();
            }
        }
        #endregion
    }
}
