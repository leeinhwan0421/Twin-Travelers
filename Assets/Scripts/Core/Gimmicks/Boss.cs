using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TwinTravelers.Management;

namespace TwinTravelers.Core.Gimmicks
{
    /// <summary>
    /// ���� Ŭ����
    /// </summary>
    public sealed class Boss : MonoBehaviour
    {
        #region Fields
        /// <summary>
        /// ������ �ӵ�
        /// </summary>
        [Header("Presets")]
        [Tooltip("������ �ӵ�")]
        [SerializeField]
        [Range(1.0f, 10.0f)] 
        private float moveSpeed = 1f;

        /// <summary>
        /// ���� Loop �ð�
        /// </summary>
        [Tooltip("���� Loop �ð�")]
        [SerializeField]
        [Range(3.0f, 10.0f)] 
        private float patternTime = 5.0f;

        /// <summary>
        /// ù��° ����: ��ֹ� ��ġ ������
        /// </summary>
        [Header("Patterns")]
        [Tooltip("ù��° ����: ��ֹ� ��ġ ������")]
        [SerializeField] 
        private List<Transform> pattern1 = new List<Transform>();

        /// <summary>
        /// ù��° ����: ��ֹ� ��ġ ������
        /// </summary>
        [Tooltip("ù��° ����: ��ֹ� ��ġ ������")]
        [SerializeField]
        private List<Transform> pattern2 = new List<Transform>();

        /// <summary>
        /// ��ֹ� ������
        /// </summary>
        [Tooltip("��ֹ� ������")]
        [SerializeField]
        private GameObject obstaclePrefab;

        /// <summary>
        /// ���� ������Ʈ�� ���� ����
        /// </summary>
        private readonly int patternCount = 2;

        /// <summary>
        /// ���� ������Ʈ�� �ִϸ����� ������Ʈ
        /// </summary>
        private Animator animator;

        /// <summary>
        /// ������ ��ֹ� ����Ʈ
        /// </summary>
        private List<GameObject> obstacles = new List<GameObject>();

        private float direction = 0.0f;
        #endregion

        #region LifeCycle
        private void Awake()
        {
            animator = GetComponent<Animator>();
        }

        private void Start()
        {
            StartCoroutine(PatternCoroutine());
        }

        private void Update()
        {
            UpdateAnimatorParameters();
        }

        private void OnDestroy()
        {
            foreach (var obstacle in obstacles)
            {
                if (obstacle != null)
                {
                    Destroy(obstacle);
                }
            }

            obstacles.Clear();
        }
        #endregion

        #region Animator Setting
        /// <summary>
        /// �ִϸ����� �Ķ���͸� ������Ʈ�ϴ� �޼���
        /// </summary>
        private void UpdateAnimatorParameters()
        {
            animator.SetBool("IsRun", Mathf.Abs(direction) > 0.2f);
        }
        #endregion

        #region Movement
        /// <summary>
        /// �� �÷��̾��� �߽������� �̵��ϴ� �޼���
        /// </summary>
        private void Move()
        {
            var playerList = GameManager.Instance.spawnManager.Players;

            if (playerList == null || playerList.Count == 0)
            {
                return;
            }

            float x = 0.0f;

            foreach (var player in playerList)
            {
                x += player.transform.position.x;
            }

            x /= playerList.Count;
            direction = x - transform.position.x;
            direction = Mathf.Clamp(direction, -1.0f, 1.0f);

            transform.position += moveSpeed * Time.deltaTime * new Vector3(direction, 0.0f, 0.0f);

            if (direction > 0.0f)
            {
                transform.rotation = Quaternion.Euler(transform.eulerAngles.x, 0.0f, transform.eulerAngles.z);
            }
            else if (direction < 0.0f)
            {
                transform.rotation = Quaternion.Euler(transform.eulerAngles.x, 180.0f, transform.eulerAngles.z);
            }

        }
        #endregion

        #region Pattern Method
        /// <summary>
        /// ���� ���� ��, �������� ������ ����մϴ�.
        /// </summary>
        private void StartPattern()
        {
            int pattern = Random.Range(0, patternCount);

            pattern += 1;

            animator.SetTrigger($"Attack_{pattern}");

            if (pattern == 1)
            {
                StartCoroutine(Pattern1Coroutine());
            }
            else if (pattern == 2)
            {
                StartCoroutine(Pattern2Coroutine());
            }
        }

        /// <summary>
        /// ������ Ư�� �ð����� ����ϴ� �ڷ�ƾ
        /// </summary>
        /// <returns>IEnumerator</returns>
        private IEnumerator PatternCoroutine()
        {
            while (true)
            {
                float time = 0.0f;

                while (time < patternTime)
                {
                    Move();

                    time += Time.deltaTime;

                    yield return null;
                }

                direction = 0.0f;

                StartPattern();

                yield break;
            }
        }
        #endregion

        #region Patterns

        /// <summary>
        /// ���� 1 �ڷ�ƾ
        /// </summary>
        /// <returns>IEnumerator</returns>
        private IEnumerator Pattern1Coroutine()
        {
            foreach (var item in pattern1)
            {
                item.gameObject.SetActive(true);
            }

            yield return new WaitForSeconds(0.5f);

            for (int i = 0; i < obstacles.Count; i++)
            {
                if (obstacles[i] == null)
                {
                    obstacles.RemoveAt(i);
                }
            }

            foreach (var item in pattern1)
            {
                obstacles.Add(Instantiate(obstaclePrefab, item.transform.position, Quaternion.identity));
                item.gameObject.SetActive(false);
            }
        }

        /// <summary>
        /// ���� 2 �ڷ�ƾ
        /// </summary>
        /// <returns>IEnumerator</returns>
        private IEnumerator Pattern2Coroutine()
        {
            foreach (var item in pattern2)
            {
                item.gameObject.SetActive(true);
            }

            yield return new WaitForSeconds(0.2f);


            for (int i = 0; i < obstacles.Count; i++)
            {
                if (obstacles[i] == null)
                {
                    obstacles.RemoveAt(i);
                }
            }

            foreach (var item in pattern2)
            {
                obstacles.Add(Instantiate(obstaclePrefab, item.transform.position, Quaternion.identity));
                item.gameObject.SetActive(false);

                yield return new WaitForSeconds(0.05f);
            }
        }

        #endregion
    }
}
