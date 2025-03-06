using System.Collections.Generic;

namespace TwinTravelers.Level
{
    [System.Serializable]
    public class Stage
    {
        public string stageName;    // 스테이지 이름
        public int starCount;    // 별 획득 개수
        public bool isUnlocked;   // 스테이지 잠금 여부
    }

    [System.Serializable]
    public class Theme
    {
        public string themeName;  // 테마 이름
        public List<Stage> stages;     // 해당 테마에 포함된 스테이지 목록
    }
}
