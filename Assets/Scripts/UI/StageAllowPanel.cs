using UnityEngine.SceneManagement;
using UnityEngine;
using TMPro;
using TwinTravelers.Management;

namespace TwinTravelers.UI
{
    /// <summary>
    /// 스테이지 시작 알림 패널
    /// </summary>
    public class StageAllowPanel : MonoBehaviour
    {
        /// <summary>
        /// 스테이지 이름을 출력할 텍스트
        /// </summary>
        [Header("Preset")]
        [SerializeField] 
        private TextMeshProUGUI text;

        private void Awake()
        {
            text.text = SceneManager.GetActiveScene().name;
        }

        private void StartGame()
        {
            GameManager.Instance.InitializeStage();

            gameObject.SetActive(false);
        }
    }
}
