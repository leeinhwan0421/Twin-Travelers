using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using TwinTravelers.Level;
using TwinTravelers.Management;

namespace TwinTravelers.UI
{
    /// <summary>
    /// 스테이지 선택 UI
    /// </summary>
    public class StageObject : MonoBehaviour
    {
        #region Fields
        /// <summary>
        /// 잠금 해제 스프라이트
        /// </summary>
        [Header("Preset")]
        [Tooltip("잠금 해제 스프라이트")]
        [SerializeField] 
        private Sprite unlockSprite;

        /// <summary>
        /// 잠금 스프라이트
        /// </summary>
        [Tooltip("잠금 스프라이트")]
        [SerializeField]
        private Sprite lockSprite;

        /// <summary>
        /// 닫기 아이콘
        /// </summary>
        [Header("Child")]
        [Tooltip("닫기 아이콘")]
        [SerializeField]
        private GameObject closeIcon;

        /// <summary>
        /// 잠금 해제 스프라이트
        /// </summary>
        [Header("Star")]
        [Tooltip("잠금 해제 스프라이트")]
        [SerializeField]
        private GameObject starGroup;

        /// <summary>
        /// 별 리스트
        /// </summary>
        [Tooltip("별 리스트")]
        [SerializeField]
        private List<GameObject> fill;

        /// <summary>
        /// 스테이지 번호
        /// </summary>
        [Header("Text")]
        [Tooltip("스테이지 번호")]
        [SerializeField] private TextMeshProUGUI stageNumber;

        /// <summary>
        /// 스테이지
        /// </summary>
        private Stage stage;

        /// <summary>
        /// 씬 이름
        /// </summary>
        private string sceneName;
        #endregion

        #region Methods
        /// <summary>
        /// 스테이지 오브젝트 설정
        /// </summary>
        /// <param name="stage">스테이지 정보</param>
        /// <param name="sceneName">이동할 씬 이름</param>
        public void SetStageObject(Stage stage, string sceneName)
        {
            this.stage = stage;
            this.sceneName = sceneName;

            if (stage.isUnlocked)
            {
                Unlock();
            }
            else
            {
                Lock();
            }
        }

        /// <summary>
        /// 잠금 해제 상태일 때 호출되는 메서드
        /// </summary>
        private void Unlock()
        {
            GetComponent<Image>().sprite = unlockSprite;
            GetComponent<SoundButton>().buttonType = Button_Type.Confirm;

            stageNumber.text = stage.stageName.Split(' ')[1];
            stageNumber.gameObject.SetActive(true);

            if (stage.starCount == 0)
            {
                starGroup.SetActive(false);
            }
            else
            {
                starGroup.SetActive(true);

                for (int i = 0; i < stage.starCount; i++)
                {
                    fill[i].SetActive(true);
                }
            }
        }

        /// <summary>
        /// 잠금 상태일 때 호출되는 메서드
        /// </summary>
        private void Lock()
        {
            GetComponent<Image>().sprite = lockSprite;
            GetComponent<SoundButton>().buttonType = Button_Type.Block;

            closeIcon.SetActive(true);
            starGroup.SetActive(false);
        }

        /// <summary>
        /// 클릭 이벤트
        /// </summary>
        public void OnClick()
        {
            if (!stage.isUnlocked)
                return;

            LoadSceneManager.LoadScene(sceneName);
        }
        #endregion
    }
}
