using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bird : MonoBehaviour
{
    private Animator anim;

    private Vector3 offset;
    private Vector3 targetPosition;

    [Header("Offset")]
    public Vector3 min;
    public Vector3 max;
    public float lerpSpeed;

    private void Awake()
    {
        anim = GetComponent<Animator>();

        offset = transform.position;
        targetPosition = offset;
    }

    private void FixedUpdate()
    {
        transform.position = Vector3.Slerp(transform.position, targetPosition, Time.fixedDeltaTime * lerpSpeed);
    }

    private void OnMouseEnter()
    {
        if (anim == null) return;

        anim.SetBool("OnCursor", true);

        float x = Random.Range(min.x, max.x);
        float y = Random.Range(min.y, max.y);
        float z = Random.Range(min.z, max.z);

        targetPosition = offset + new Vector3(x, y, z);
    }

    private void OnMouseExit()
    {
        if (anim == null) return;

        anim.SetBool("OnCursor", false);

        targetPosition = offset;
    }
}
