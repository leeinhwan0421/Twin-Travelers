using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AudioSettings : MonoBehaviour
{
    [Header("Presets")]
    [SerializeField] private Slider bgmVolumeSlider;
    [SerializeField] private TextMeshProUGUI bgmVolumeText;
    [SerializeField] private Image bgmVolumeIcon;
    [SerializeField] private Sprite bgmOn;
    [SerializeField] private Sprite bgmOff;
    [Space(10.0f)]
    [SerializeField] private Slider sfxVolumeSlider;
    [SerializeField] private TextMeshProUGUI sfxVolumeText;
    [SerializeField] private Image sfxVolumeIcon;
    [SerializeField] private Sprite sfxOn;
    [SerializeField] private Sprite sfxOff;

    private void Awake()
    {
        bgmVolumeSlider.value = SettingManager.BGMVolume;
        sfxVolumeSlider.value = SettingManager.SFXVolume;

        ChangeBGMVolume(bgmVolumeSlider);
        ChangeSFXVolume(sfxVolumeSlider);
    }

    public void ChangeBGMVolume(Slider slider)
    {
        int volume = (int)slider.value;

        bgmVolumeText.text = $"{volume}";
        bgmVolumeIcon.sprite = volume != 0 ? bgmOn : bgmOff;

        AudioManager.Instance.ChangeBGMVolume(volume);

        SettingManager.SetBGMVolume(volume);
    }

    public void ChangeSFXVolume(Slider slider)
    {
        int volume = (int)slider.value;

        sfxVolumeText.text = $"{volume}";
        sfxVolumeIcon.sprite = volume != 0 ? sfxOn : sfxOff;

        AudioManager.Instance.ChangeSFXVolume(volume);

        SettingManager.SetSFXVolume(volume);
    }
}
