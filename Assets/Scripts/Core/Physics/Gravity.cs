using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gravity : MonoBehaviour
{
    private void Update()
    {
        transform.position += (Vector3)Physics2D.gravity * Time.unscaledDeltaTime;
    }
}
