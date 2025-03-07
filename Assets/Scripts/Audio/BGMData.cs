using UnityEngine;

namespace TwinTravelers.Audio
{
    /// <summary>
    /// BGM 데이터
    /// </summary>
    [CreateAssetMenu(fileName = "New BGM", menuName = "Audio/BGM Source", order = 2)]
    public class BGMData : ScriptableObject
    {
        /// <summary>
        /// 스테이지 이름
        /// </summary>
        public string stageName;

        /// <summary>
        /// 사용할 사운드 클립
        /// </summary>
        public AudioClip clip;
    }
}
