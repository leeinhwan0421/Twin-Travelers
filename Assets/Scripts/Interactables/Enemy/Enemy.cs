using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Presets")]
    [SerializeField] private float lerpSpeed; // LerpSpeed;
    [SerializeField] private float cycleTime;
    [SerializeField] private Vector2 max;
    [SerializeField] private Vector2 min;

    private Vector2 drawFrom = Vector2.zero;
    private Vector2 drawTo = Vector2.zero;

    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();

        min += (Vector2)transform.position;
        max += (Vector2)transform.position;

        drawFrom = min;
        drawTo = max;
    }

    private void Start()
    {
        StartCoroutine(MoveCycle());
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;

        if (drawFrom == Vector2.zero && drawTo == Vector2.zero)
        {
            Gizmos.DrawLine((Vector2)transform.position + min, (Vector2)transform.position + max);
        }

        Gizmos.DrawLine(drawFrom, drawTo);
    }

    private IEnumerator MoveCycle()
    {
        yield return new WaitForSeconds(2.0f);

        while (true)
        {
            yield return MoveToPosition(max, lerpSpeed); // 무조건 오른쪽이죠..

            yield return new WaitForSeconds(cycleTime / 2);

            yield return MoveToPosition(min, lerpSpeed);

            yield return new WaitForSeconds(cycleTime / 2);
        }
    }

    private IEnumerator MoveToPosition(Vector2 position, float speed)
    {
        Vector2 s = transform.position;
        Vector2 e = position;

        float distance  = Vector2.Distance(s, e);
        float time      = distance / speed;
        float timer     = 0.0f;

        if ((e - s).normalized.x == -1)
        {
            transform.localEulerAngles = new Vector3(0f, 180f, 0f);
        }
        else if ((e - s).normalized.x == 1)
        {
            transform.localEulerAngles = new Vector3(0f, 0f, 0f);
        }

        animator.SetBool("IsMove", true);

        while(timer <= time)
        {
            transform.position = Vector2.Lerp(s, e, timer / time);
            timer += Time.deltaTime;

            yield return null;
        }

        animator.SetBool("IsMove", false);

        transform.position = e;
    }
}
