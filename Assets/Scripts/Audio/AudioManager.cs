using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    #region properties
    // 싱글턴 구현
    private static AudioManager instance;
    public static AudioManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<AudioManager>(); // 이래도 없다?

                if (instance == null)
                {
                    GameObject prefab = Resources.Load<GameObject>("AudioManger");

                    if (prefab != null)
                    {
                        GameObject obj = Instantiate(prefab);
                        instance = obj.GetComponent<AudioManager>();
                    }
                }
            }
            return instance;
        }
    }

    [Header("BGM")]
    [SerializeField] private List<BGMData> bgmDatas = new List<BGMData>();
    public int bgmVolume;

    [Header("SFX")]
    [SerializeField] private List<SFXData> sfxDatas = new List<SFXData>();
    public int sfxVolume;

    [Header("SFX Pooling")]
    private int poolSize = 20;
    private Queue<AudioSource> sfxPool = new Queue<AudioSource>();

    private Dictionary<string, AudioClip> bgms = new Dictionary<string, AudioClip>();
    private Dictionary<string, AudioClip[]> sfxs = new Dictionary<string, AudioClip[]>();

    private AudioSource bgmSource;
    #endregion

    #region Initalize
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
            return;
        }

        bgmSource = gameObject.GetComponent<AudioSource>();

        foreach (BGMData data in bgmDatas)
        {
            bgms.Add(data.stageName, data.clip);
        }

        foreach (SFXData data in sfxDatas)
        {
            sfxs.Add(data.soundName, data.clip);
        }

        ChangeBGMVolume(SettingManager.BGMVolume);
        ChangeSFXVolume(SettingManager.SFXVolume);

        ChangeWithPlay(SceneManager.GetActiveScene().name);
        InitializeSFXPool();
    }

    private void InitializeSFXPool()
    {
        for (int i = 0; i < poolSize; i++)
        {
            GameObject sfxObject = new GameObject($"SFX_Source_Pool_{i}");
            AudioSource source = sfxObject.AddComponent<AudioSource>();

            sfxObject.transform.SetParent(transform);
            sfxObject.SetActive(false);

            source.playOnAwake = false;
            sfxPool.Enqueue(source);
        }
    }
    #endregion

    #region Background Music
    public void PlayBGM()
    {
        bgmSource.Play();
    }

    public void ChangeBGM(string name)
    {
        if (!bgms.ContainsKey(name) || bgms[name] == null)
        {
#if SHOW_DEBUG_MESSAGES
            Debug.LogWarning($"wrong background music: {name}");
#endif
            return;
        }

        bgmSource.clip = bgms[name];
    }

    public void StopBGM()
    {
        bgmSource.Stop();
    }

    public void ChangeWithPlay(string name)
    {
        ChangeBGM(name);
        PlayBGM();
    }

    public void ChangeBGMVolume(int volume)
    {
        bgmVolume = volume;

        bgmSource.volume = bgmVolume / 100.0f;
    }
    #endregion

    #region Sound FX
    public void PlaySFX(string name)
    {
        if (!sfxs.ContainsKey(name) || sfxs[name] == null)
        {
#if SHOW_DEBUG_MESSAGES
            Debug.LogWarning($"wrong Sound FX: {name}");
#endif
            return;
        }

        AudioSource source = GetPooledSFXSource();

        var sfx = sfxs[name];
        source.clip = sfx[Random.Range(0, sfx.Length)];
        source.gameObject.SetActive(true);
        source.volume = sfxVolume / 100.0f;
        source.Play();

        StartCoroutine(ReturnSFXAfterPlay(source));
    }

    public void ChangeSFXVolume(int volume)
    {
        sfxVolume = volume;
    }

    private AudioSource GetPooledSFXSource()
    {
        if (sfxPool.Count > 0)
        {
            return sfxPool.Dequeue();
        }
        else
        {
            GameObject sfxObject = new GameObject("SFX_Source_Pool");
            AudioSource source = sfxObject.AddComponent<AudioSource>();

            sfxObject.transform.SetParent(transform);
            source.playOnAwake = false;

            return source;
        }
    }

    private IEnumerator ReturnSFXAfterPlay(AudioSource source)
    {
        yield return new WaitWhile(() => source.isPlaying);

        source.Stop();
        source.clip = null;
        source.gameObject.SetActive(false);

        sfxPool.Enqueue(source);
    }
    #endregion
}
