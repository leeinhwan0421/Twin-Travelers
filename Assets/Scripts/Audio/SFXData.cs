using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New SFX", menuName = "Audio/SFX Source", order = 2)]
public class SFXData : ScriptableObject
{
    public string soundName; // ����Ʈ ���� �̸�
    public AudioClip[] clip; // �������� ����� ����
}
