using System.Collections.Generic;

namespace TwinTravelers.Level
{
    [System.Serializable]
    public class Stage
    {
        public string stageName;    // �������� �̸�
        public int starCount;    // �� ȹ�� ����
        public bool isUnlocked;   // �������� ��� ����
    }

    [System.Serializable]
    public class Theme
    {
        public string themeName;  // �׸� �̸�
        public List<Stage> stages;     // �ش� �׸��� ���Ե� �������� ���
    }
}
