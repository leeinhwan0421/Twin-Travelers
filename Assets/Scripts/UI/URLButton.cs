using UnityEngine;

namespace TwinTravelers.UI
{
    public class URLButton : MonoBehaviour
    {
        [Header("Preset")]
        [SerializeField] private string link;

        public void OnClick()
        {
#if UNITY_EDITOR
            Debug.Log($"OpenURL : {link}");
#endif
            Application.OpenURL(link);
        }
    }
}
