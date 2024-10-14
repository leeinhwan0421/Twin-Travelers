using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New BGM", menuName = "Audio/BGM Source", order = 2)]
public class BGMData : ScriptableObject
{
    public string stageName; // 스테이지 이름
    public AudioClip clip;
}
