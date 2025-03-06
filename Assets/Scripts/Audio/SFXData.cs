using UnityEngine;

namespace TwinTravelers.Audio
{
    /// <summary>
    /// SFX 데이터
    /// </summary>
    [CreateAssetMenu(fileName = "New SFX", menuName = "Audio/SFX Source", order = 2)]
    public class SFXData : ScriptableObject
    {
        /// <summary>
        /// sfx 이름
        /// </summary>
        public string soundName;

        /// <summary>
        /// 랜덤으로 사용할 사운드 클립 리스트
        /// </summary>
        public AudioClip[] clip;
    }
}
