using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBalloon : MonoBehaviour
{
    [Header("Preset")]
    [SerializeField] private GameObject player;

    [Header("Gravity Settings")]
    [Range(0.01f, 0.99f)] [SerializeField] private float gravityScaleMultipler;
    private float originalGravityScale;

    [Header("Velocity Limit Settings")]
    [SerializeField] private Vector2 min;
    [SerializeField] private Vector2 max;

    [Header("Effect")]
    [SerializeField] private GameObject effect;

    private void Awake()
    {
        originalGravityScale = player.GetComponent<Rigidbody2D>().gravityScale;
    }

    private void OnEnable()
    {
        player.GetComponent<Rigidbody2D>().gravityScale = originalGravityScale * gravityScaleMultipler;
        AudioManager.Instance.PlaySFX("BalloonEquipped");
    }

    private void OnDisable()
    {
        player.GetComponent<Rigidbody2D>().gravityScale = originalGravityScale;
    }

    private void Update()
    {
        Rigidbody2D rigid = player.GetComponent<Rigidbody2D>();
        Vector2 velocity = rigid.velocity;

        float x = Mathf.Clamp(velocity.x, min.x, max.x);
        float y = Mathf.Clamp(velocity.y, min.y, max.y);
        rigid.velocity = new Vector2(x, y);
    }

    public void ExplodeBalloon()
    {
        Instantiate(effect, transform.position, Quaternion.identity);
        AudioManager.Instance.PlaySFX("BalloonUnprepared");
        gameObject.SetActive(false);
    }
}
