using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TwinTravelers.Audio;

namespace TwinTravelers.Management
{
    /// <summary>
    /// 인게임 오디오를 관리하는 클래스
    /// </summary>
    public class AudioManager : MonoBehaviour
    {
        #region Singletion
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
        #endregion

        #region Field
        /// <summary>
        /// 사용할 BGM 데이터 리스트
        /// </summary>
        [Header("BGM")]
        [Tooltip("사용할 BGM 데이터 리스트")]
        [SerializeField] 
        private List<BGMData> bgmDatas = new List<BGMData>();
        private Dictionary<string, AudioClip> bgms = new Dictionary<string, AudioClip>();

        /// <summary>
        /// BGM AudioSource
        /// </summary>
        private AudioSource bgmSource;

        /// <summary>
        /// BGM 볼륨
        /// </summary>
        [HideInInspector]
        public int bgmVolume;

        /// <summary>
        /// 사용할 SFX 데이터 리스트
        /// </summary>
        [Header("SFX")]
        [Tooltip("사용할 SFX 데이터 리스트")]
        [SerializeField]
        private List<SFXData> sfxDatas = new List<SFXData>();
        private Dictionary<string, AudioClip[]> sfxs = new Dictionary<string, AudioClip[]>();

        /// <summary>
        /// SFX 볼륨
        /// </summary>
        [HideInInspector]
        public int sfxVolume;

        /// <summary>
        /// SFX Object Pool 크기
        /// </summary>
        private int poolSize = 20;

        /// <summary>
        /// 큐를 통해 사용하는 SFX Object Pool
        /// </summary>
        private Queue<AudioSource> sfxPool = new Queue<AudioSource>();
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

            foreach (var data in bgmDatas)
            {
                bgms.Add(data.stageName, data.clip);
            }

            foreach (var data in sfxDatas)
            {
                sfxs.Add(data.soundName, data.clip);
            }

            ChangeBGMVolume(SettingManager.BGMVolume);
            ChangeSFXVolume(SettingManager.SFXVolume);

            ChangeWithPlay(SceneManager.GetActiveScene().name);
            InitializeSFXPool();
        }

        /// <summary>
        /// SFX Object Pool 초기화
        /// </summary>
        private void InitializeSFXPool()
        {
            for (int i = 0; i < poolSize; i++)
            {
                GameObject sfxObject = new GameObject($"SFX_Source_Pool_{i.ToString()}");
                AudioSource source = sfxObject.AddComponent<AudioSource>();

                sfxObject.transform.SetParent(transform);
                sfxObject.SetActive(false);

                source.playOnAwake = false;
                sfxPool.Enqueue(source);
            }
        }
        #endregion

        #region Background Music
        /// <summary>
        /// BGM 재생
        /// </summary>
        public void PlayBGM()
        {
            bgmSource.Play();
        }

        /// <summary>
        /// BGM 변경
        /// </summary>
        /// <param name="name">변경할 BGM의 이름</param>
        public void ChangeBGM(string name)
        {
            if (!bgms.ContainsKey(name) || bgms[name] == null)
            {
#if UNITY_EDITOR
                Debug.LogWarning($"wrong background music: {name.ToString()}");
#endif
                return;
            }

            bgmSource.clip = bgms[name];
        }

        /// <summary>
        /// BGM 정지
        /// </summary>
        public void StopBGM()
        {
            bgmSource.Stop();
        }

        /// <summary>
        /// BGM 변경 후 재생
        /// </summary>
        /// <param name="name">변경할 BGM의 이름</param>
        public void ChangeWithPlay(string name)
        {
            ChangeBGM(name);
            PlayBGM();
        }

        /// <summary>
        /// BGM 볼륨 변경
        /// </summary>
        /// <param name="volume">변경할 볼륨 값</param>
        public void ChangeBGMVolume(int volume)
        {
            bgmVolume = volume;

            bgmSource.volume = bgmVolume / 100.0f;
        }
        #endregion

        #region Sound FX
        /// <summary>
        /// SFX 재생
        /// </summary>
        /// <param name="name">재생할 SFX 이름</param>
        public void PlaySFX(string name)
        {
            if (!sfxs.ContainsKey(name) || sfxs[name] == null)
            {
#if UNITY_EDITOR
                Debug.LogWarning($"wrong Sound FX: {name.ToString()}");
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

        /// <summary>
        /// SFX 볼륨 변경
        /// </summary>
        /// <param name="volume">변경할 볼륨</param>
        public void ChangeSFXVolume(int volume)
        {
            sfxVolume = volume;
        }

        /// <summary>
        /// SFX Object Pool에서 사용할 AudioSource 가져오기
        /// </summary>
        /// <returns>AudioSource</returns>
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

        /// <summary>
        /// SFX 재생 후 Object Pool로 반환
        /// </summary>
        /// <param name="source">SFX를 재생할 AudioSoruce</param>
        /// <returns>IEnumerator</returns>
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
}
