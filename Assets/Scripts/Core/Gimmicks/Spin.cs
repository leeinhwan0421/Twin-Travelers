using UnityEngine;

namespace TwinTravelers.Core.Gimmicks
{
    public sealed class Spin : Obstacle
    {
        [Header("Preset")]
        [SerializeField] private float rotationSpeed;
        [SerializeField] private bool isLeftRotation;

        private void Update()
        {
            Vector3 direction = isLeftRotation ? Vector3.forward : -Vector3.forward;
            transform.Rotate(direction * rotationSpeed * Time.deltaTime);
        }
    }
}
