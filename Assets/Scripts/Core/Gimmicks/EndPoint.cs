using UnityEngine;
using TwinTravelers.Management;

namespace TwinTravelers.Core.Gimmicks
{
    /// <summary>
    /// 스테이지 클리어 조건을 담당하는 클래스
    /// </summary>
    public class EndPoint : MonoBehaviour
    {
        /// <summary>
        /// 도달한 플레이어 수
        /// </summary>
        private int count;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                count++;

                if (count >= 2)
                {
                    GameManager.Instance.VictoryStage();
                }
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                count--;
            }
        }
    }
}
