using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TwinTravelers.Management;

namespace TwinTravelers.Core.Gimmicks
{
    /// <summary>
    /// 보스 클래스
    /// </summary>
    public sealed class Boss : MonoBehaviour
    {
        #region Fields
        /// <summary>
        /// 움직일 속도
        /// </summary>
        [Header("Presets")]
        [Tooltip("움직일 속도")]
        [SerializeField]
        [Range(1.0f, 10.0f)] 
        private float moveSpeed = 1f;

        /// <summary>
        /// 패턴 Loop 시간
        /// </summary>
        [Tooltip("패턴 Loop 시간")]
        [SerializeField]
        [Range(3.0f, 10.0f)] 
        private float patternTime = 5.0f;

        /// <summary>
        /// 첫번째 패턴: 장애물 위치 프리셋
        /// </summary>
        [Header("Patterns")]
        [Tooltip("첫번째 패턴: 장애물 위치 프리셋")]
        [SerializeField] 
        private List<Transform> pattern1 = new List<Transform>();

        /// <summary>
        /// 첫번째 패턴: 장애물 위치 프리셋
        /// </summary>
        [Tooltip("첫번째 패턴: 장애물 위치 프리셋")]
        [SerializeField]
        private List<Transform> pattern2 = new List<Transform>();

        /// <summary>
        /// 장애물 프리펩
        /// </summary>
        [Tooltip("장애물 프리펩")]
        [SerializeField]
        private GameObject obstaclePrefab;

        /// <summary>
        /// 보스 컴포넌트의 패턴 개수
        /// </summary>
        private readonly int patternCount = 2;

        /// <summary>
        /// 보스 오브젝트의 애니메이터 컴포넌트
        /// </summary>
        private Animator animator;

        /// <summary>
        /// 생성한 장애물 리스트
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
        /// 애니메이터 파라미터를 업데이트하는 메서드
        /// </summary>
        private void UpdateAnimatorParameters()
        {
            animator.SetBool("IsRun", Mathf.Abs(direction) > 0.2f);
        }
        #endregion

        #region Movement
        /// <summary>
        /// 두 플레이어의 중심점으로 이동하는 메서드
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
        /// 패턴 시작 시, 랜덤으로 패턴을 재생합니다.
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
        /// 패턴을 특정 시간마다 재생하는 코루틴
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
        /// 패턴 1 코루틴
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
        /// 패턴 2 코루틴
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
