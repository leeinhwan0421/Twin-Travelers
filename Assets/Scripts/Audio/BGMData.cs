using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New BGM", menuName = "Audio/BGM Source", order = 2)]
public class BGMData : ScriptableObject
{
    public string stageName; // �������� �̸�
    public AudioClip clip;
    public bool loop; // �ظ��ϸ� true
}
