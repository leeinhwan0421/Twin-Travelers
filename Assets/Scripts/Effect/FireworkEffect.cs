using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireworkEffect : Effect
{
    [SerializeField] private float moveSpeed;

    private void Update()
    {
        transform.position += Vector3.up * moveSpeed * Time.deltaTime;    
    }

    private void PlayFlySound()
    {
        AudioManager.Instance.PlaySFX("FireworkFly");
    }

    private void PlayBombSound()
    {
        AudioManager.Instance.PlaySFX("FireworkBomb");
        moveSpeed *= 0.1f;
    }
}
