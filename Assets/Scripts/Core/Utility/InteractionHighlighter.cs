using System.Collections.Generic;
using UnityEngine;

namespace TwinTravelers.Core.Utility
{
    public class InteractionHighlighter : MonoBehaviour
    {
        /// <summary>
        /// 하이라이트를 적용할 스프라이트 렌더러 리스트
        /// </summary>
        [Header("Presets")]
        [Tooltip("하이라이트를 적용할 스프라이트 렌더러 리스트")]
        [SerializeField]
        private List<SpriteRenderer> activeList = new List<SpriteRenderer>();

        /// <summary>
        /// 기본 머터리얼
        /// </summary>
        [Header("Materials")] 
        [Tooltip("기본 머터리얼")]
        [SerializeField]
        private Material defaultMaterial;

        /// <summary>
        /// 하이라이트 머터리얼
        /// </summary>
        [Tooltip("하이라이트 머터리얼")]
        [SerializeField] 
        private Material highlightPrefab;

        private void OnMouseEnter()
        {
            foreach (var item in activeList)
            {
                item.material = highlightPrefab;
            }
        }

        private void OnMouseExit()
        {
            foreach (var item in activeList)
            {
                item.material = defaultMaterial;
            }
        }
    }
}
