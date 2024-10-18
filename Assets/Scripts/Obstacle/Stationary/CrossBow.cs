using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossBow : MonoBehaviour
{
    [Header("Presets")]
    [SerializeField] private Transform muzzle;
    [Space(10.0f)]
    [Range(0.05f, 1.0f)] [SerializeField] private float shootSpeed;
    [Range(3.0f, 10.0f)] [SerializeField] private float shootTime;

    [Header("Prefab")]
    [SerializeField] private GameObject arrow;

    private Animator animator;
    private float timer = 0.0f;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        timer += Time.deltaTime;

        if (timer >= shootTime)
        {
            Shot();
            timer = 0.0f;
        }
    }

    private void Shot()
    {
        animator.SetTrigger("Shot");

        AudioManager.Instance.PlaySFX("Crossbow");
    }

    private void ShotArrow()
    {
        GameObject arrow = Instantiate(this.arrow, muzzle.position, muzzle.rotation);
        arrow.GetComponent<Rigidbody2D>().AddForce(muzzle.right * shootSpeed);

        timer = 0.0f;
    }
}
