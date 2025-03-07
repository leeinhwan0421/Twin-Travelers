using UnityEngine;
using UnityEngine.UI;
using TMPro;
using TwinTravelers.Management;

namespace TwinTravelers.UI
{
    /// <summary>
    /// 오디오 설정 UI 클래스
    /// </summary>
    public class AudioSettings : MonoBehaviour
    {
        #region Fields
        /// <summary>
        /// 배경음악 볼륨 슬라이더
        /// </summary>
        [Header("Presets")]
        [Tooltip("배경음악의 볼륨을 조절하는 슬라이더")]
        [SerializeField]
        private Slider bgmVolumeSlider;

        /// <summary>
        /// 배경음악 볼륨 텍스트
        /// </summary>
        [Tooltip("현재 배경음악 볼륨을 표시하는 텍스트")]
        [SerializeField]
        private TextMeshProUGUI bgmVolumeText;

        /// <summary>
        /// 배경음악 볼륨 아이콘
        /// </summary>
        [Tooltip("배경음악이 켜져 있는지 여부를 나타내는 아이콘")]
        [SerializeField]
        private Image bgmVolumeIcon;

        /// <summary>
        /// 배경음악 ON 아이콘
        /// </summary>
        [Tooltip("배경음악이 켜졌을 때 표시될 아이콘")]
        [SerializeField]
        private Sprite bgmOn;

        /// <summary>
        /// 배경음악 OFF 아이콘
        /// </summary>
        [Tooltip("배경음악이 꺼졌을 때 표시될 아이콘")]
        [SerializeField]
        private Sprite bgmOff;

        /// <summary>
        /// 효과음 볼륨 슬라이더
        /// </summary>
        [Space(10.0f)]
        [Tooltip("효과음의 볼륨을 조절하는 슬라이더")]
        [SerializeField]
        private Slider sfxVolumeSlider;

        /// <summary>
        /// 효과음 볼륨 텍스트
        /// </summary>
        [Tooltip("현재 효과음 볼륨을 표시하는 텍스트")]
        [SerializeField]
        private TextMeshProUGUI sfxVolumeText;

        /// <summary>
        /// 효과음 볼륨 아이콘
        /// </summary>
        [Tooltip("효과음이 켜져 있는지 여부를 나타내는 아이콘")]
        [SerializeField]
        private Image sfxVolumeIcon;

        /// <summary>
        /// 효과음 ON 아이콘
        /// </summary>
        [Tooltip("효과음이 켜졌을 때 표시될 아이콘")]
        [SerializeField]
        private Sprite sfxOn;

        /// <summary>
        /// 효과음 OFF 아이콘
        /// </summary>
        [Tooltip("효과음이 꺼졌을 때 표시될 아이콘")]
        [SerializeField]
        private Sprite sfxOff;
        #endregion

        #region Unity Methods
        private void Awake()
        {
            bgmVolumeSlider.value = SettingManager.BGMVolume;
            sfxVolumeSlider.value = SettingManager.SFXVolume;

            ChangeBGMVolume(bgmVolumeSlider);
            ChangeSFXVolume(sfxVolumeSlider);
        }
        #endregion

        #region Methods
        /// <summary>
        /// 배경음악 볼륨을 변경합니다.
        /// </summary>
        /// <param name="slider">Slider</param>
        public void ChangeBGMVolume(Slider slider)
        {
            int volume = (int)slider.value;

            bgmVolumeText.text = $"{volume.ToString()}";
            bgmVolumeIcon.sprite = volume != 0 ? bgmOn : bgmOff;

            AudioManager.Instance.ChangeBGMVolume(volume);

            SettingManager.SetBGMVolume(volume);
        }

        /// <summary>
        /// 효과음 볼륨을 변경합니다.
        /// </summary>
        /// <param name="slider">Slider</param>
        public void ChangeSFXVolume(Slider slider)
        {
            int volume = (int)slider.value;

            sfxVolumeText.text = $"{volume.ToString()}";
            sfxVolumeIcon.sprite = volume != 0 ? sfxOn : sfxOff;

            AudioManager.Instance.ChangeSFXVolume(volume);

            SettingManager.SetSFXVolume(volume);
        }
        #endregion
    }
}
