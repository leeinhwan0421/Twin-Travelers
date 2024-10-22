using System.Collections;
using UnityEngine;

public class Button_Door : MonoBehaviour
{
    [Header("Presets")]
    [SerializeField] private Transform door;
    [SerializeField] private float doorSpeed = 2f;
    [Space(10.0f)]
    [SerializeField] private Vector3 openLocalPosition;
    [SerializeField] private Vector3 closeLocalPosition;
    [Space(10.0f)]
    [SerializeField] private Sprite openSprite;
    [SerializeField] private Sprite closeSprite;

    private SpriteRenderer spriteRenderer;

    private int currentCount;
    private bool isOpening;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        ResetButtonDoor();
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player") && !collision.CompareTag("Moveable"))
        {
            return;
        }

        currentCount++;

        if (currentCount == 1 && !isOpening)
        {
            StopAllCoroutines();
            StartCoroutine(MoveDoor(openLocalPosition));

            AudioManager.Instance.PlaySFX("ButtonEnter");

            spriteRenderer.sprite = openSprite;
            isOpening = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player") && !collision.CompareTag("Moveable"))
        {
            return;
        }

        currentCount--;

        if (currentCount == 0 && isOpening)
        {
            StopAllCoroutines();
            StartCoroutine(MoveDoor(closeLocalPosition));

            AudioManager.Instance.PlaySFX("ButtonExit");

            spriteRenderer.sprite = closeSprite;
            isOpening = false;
        }
    }

    private IEnumerator MoveDoor(Vector3 targetPosition)
    {
        while (Vector3.Distance(door.localPosition, targetPosition) > 0.01f)
        {
            door.localPosition = Vector3.MoveTowards(door.localPosition, targetPosition, doorSpeed * Time.deltaTime);
            yield return null;
        }

        door.localPosition = targetPosition;
    }

    public void ResetButtonDoor()
    {
        StopAllCoroutines();

        door.localPosition = closeLocalPosition;
        spriteRenderer.sprite = closeSprite;
        currentCount = 0;
        isOpening = false;
    }
}
