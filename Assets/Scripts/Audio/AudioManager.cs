using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    #region properties
    // ½Ì±ÛÅÏ ±¸Çö
    private static AudioManager instance;
    public static AudioManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<AudioManager>();
                if (instance == null)
                {
                    Debug.LogError("AudioManager instance is null.");
                }
            }
            return instance;
        }
    }

    [Header("BGM")]
    [SerializeField] private List<BGMData> bgmDatas = new List<BGMData>();

    [Header("SFX")]
    [SerializeField] private List<SFXData> sfxDatas = new List<SFXData>();

    [Header("SFX Pooling")]
    private int poolSize = 10;
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
            Debug.LogWarning($"wrong background music: {name}");
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
    #endregion

    #region Sound FX
    public void PlaySFX(string name)
    {
        if (!sfxs.ContainsKey(name) || sfxs[name] == null)
        {
            Debug.LogWarning($"wrong Sound FX: {name}");
            return;
        }

        AudioSource source = GetPooledSFXSource();

        var sfx = sfxs[name];
        source.clip = sfx[Random.Range(0, sfx.Length)];
        source.gameObject.SetActive(true);
        source.Play();

        StartCoroutine(ReturnSFXAfterPlay(source));
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
