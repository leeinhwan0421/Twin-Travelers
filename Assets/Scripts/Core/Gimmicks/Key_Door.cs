using UnityEngine;

namespace TwinTravelers.Core.Gimmicks
{
    public class Key_Door : MonoBehaviour
    {
        [Header("Presets")]
        [SerializeField] private Color color;
        [Space(10.0f)]
        [SerializeField] private GameObject key;
        [Space(10.0f)]
        [SerializeField] private Animator doorAnim;

        private void Start()
        {
            SpriteRenderer[] sr = GetComponentsInChildren<SpriteRenderer>();

            for (int i = 0; i < sr.Length; i++)
            {
                sr[i].color = color;
            }
        }

        public void Open()
        {
            doorAnim.SetTrigger("Open");
        }

        public void ResetKeyDoor()
        {
            doorAnim.Rebind();
            doorAnim.ResetTrigger("Open");

            key.SetActive(true);
        }
    }
}
