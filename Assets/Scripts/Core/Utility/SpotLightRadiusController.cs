using UnityEngine.Rendering.Universal;
using UnityEngine;

namespace TwinTravelers.Core.Utility
{
    public class SpotLightRadiusController : MonoBehaviour
    {
        [Header("Presets")]
        [SerializeField] private float minInner;
        [SerializeField] private float maxInner;
        [Space(10.0f)]
        [SerializeField] private float minOuter;
        [SerializeField] private float maxOuter;
        [Space(10.0f)]
        [SerializeField] private float time = 1.0f;

        private Light2D light2D;
        private float timer = 0.0f;

        private void Awake()
        {
            light2D = GetComponent<Light2D>();
        }

        private void Update()
        {
            timer += Time.deltaTime;

            if (time <= timer)
            {
                ChangeRadius();
                timer = 0.0f;
            }
        }

        private void ChangeRadius()
        {
            light2D.pointLightInnerRadius = Random.Range(minInner, maxInner);
            light2D.pointLightOuterRadius = Random.Range(minOuter, maxOuter);
        }
    }
}
