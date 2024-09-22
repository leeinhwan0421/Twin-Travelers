using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResultPanel : MonoBehaviour
{
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();    
    }

    public void Enable()
    {
        SetEnable();
        animator.SetTrigger("Enable");
    }

    public void Disable()
    {
        animator.SetTrigger("Disable");
    }

    private void SetEnable() => gameObject.SetActive(true);

    private void SetDisable() => gameObject.SetActive(false);
}
