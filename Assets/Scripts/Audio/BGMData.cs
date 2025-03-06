using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TwinTravelers.Audio
{
    /// <summary>
    /// BGM ������
    /// </summary>
    [CreateAssetMenu(fileName = "New BGM", menuName = "Audio/BGM Source", order = 2)]
    public class BGMData : ScriptableObject
    {
        /// <summary>
        /// �������� �̸�
        /// </summary>
        public string stageName;

        /// <summary>
        /// ����� ���� Ŭ��
        /// </summary>
        public AudioClip clip;
    }
}
