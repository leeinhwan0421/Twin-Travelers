using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
