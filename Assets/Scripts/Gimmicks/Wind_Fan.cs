using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wind_Fan : InteractableTrigger
{
    [Header("Presets")]
    [Range(0.01f, 100.0f)][SerializeField] private float windSpeed;

    private AudioSource source;

    private HashSet<GameObject> colls = new HashSet<GameObject>();

    private void Awake()
    {
        source = GetComponent<AudioSource>();
    }

    private void Update()
    {
        foreach (var obj in colls)
        {
            obj.transform.position += Time.unscaledDeltaTime * windSpeed * transform.right;
            obj.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
        }
    }

    protected override void EnterEvent(Collider2D collision)
    {
        colls.Add(collision.gameObject);

        if (colls.Count > 0)
        {
            source.Play();
        }
    }

    protected override void ExitEvent(Collider2D collision)
    {
        colls.Remove(collision.gameObject);

        if (colls.Count <= 0)
        {
            source.Stop();
        }
    }
}
