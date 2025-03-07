using UnityEngine;

namespace TwinTravelers.UI
{
    /// <summary>
    /// 특정 URL 창이 열리는 버튼
    /// </summary>
    public class URLButton : MonoBehaviour
    {
        /// <summary>
        /// URL 링크
        /// </summary>
        [Header("Preset")]
        [Tooltip("URL 링크")]
        [SerializeField] 
        private string link;

        public void OnClick()
        {
#if UNITY_EDITOR
            Debug.Log($"OpenURL : {link}");
#endif
            Application.OpenURL(link);
        }
    }
}
