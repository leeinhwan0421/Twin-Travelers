using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Stage
{
    public string   stageName;    // �������� �̸�
    public int      starCount;    // �� ȹ�� ����
    public bool     isUnlocked;   // �������� ��� ����
}

[System.Serializable]
public class Theme
{
    public string       themeName;  // �׸� �̸�
    public List<Stage>  stages;     // �ش� �׸��� ���Ե� �������� ���
}
