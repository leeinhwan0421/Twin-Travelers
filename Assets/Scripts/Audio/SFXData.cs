using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New SFX", menuName = "Audio/SFX Source", order = 2)]
public class SFXData : ScriptableObject
{
    public string soundName; // 이펙트 사운드 이름
    public AudioClip[] clip; // 랜덤으로 사용할 예정
}
