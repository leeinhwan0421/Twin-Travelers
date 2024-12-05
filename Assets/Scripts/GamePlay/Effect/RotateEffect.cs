using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateEffect : MonoBehaviour
{
    [Header("Preset")]
    [SerializeField] private Vector3 rotation;
    [SerializeField] private float speed;

    private void Update()
    {
        transform.Rotate(speed * Time.deltaTime * rotation.normalized);
    }
}
