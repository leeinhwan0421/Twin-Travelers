using UnityEngine;
using TwinTravelers.Management;

namespace TwinTravelers.Core.Gimmicks
{
    public class EndPoint : MonoBehaviour
    {
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
