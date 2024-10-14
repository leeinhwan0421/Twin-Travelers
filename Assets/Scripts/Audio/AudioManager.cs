using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    // Singletion Property
    private AudioManager instance;
    public AudioManager Instance
    {
        get
        {
            if (instance == null)
                return null;

            return instance;
        }
    }

    // Audio Source Property
    [Header("BGM")]
    [SerializeField] private List<BGMData> bgmDatas = new List<BGMData>();
    [Header("SFX")]
    [SerializeField] private List<SFXData> sfxDatas = new List<SFXData>();

    private Dictionary<string, AudioClip> bgms = new Dictionary<string, AudioClip>();
    private Dictionary<string, AudioClip[]> sfxs = new Dictionary<string, AudioClip[]>();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        foreach (BGMData data in bgmDatas)
        {
            bgms.Add(data.stageName, data.clip);
        }

        foreach (SFXData data in sfxDatas)
        {
            sfxs.Add(data.soundName, data.clip);
        }
    }
}
