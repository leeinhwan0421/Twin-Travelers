using UnityEngine;

namespace TwinTravelers.Management
{
    /// <summary>
    /// 코나미 커맨드를 입력 시, 치트가 적용되도록 하였습니다.
    /// </summary>
    public class CheatCodeManager : MonoBehaviour
    {
        /// <summary>
        /// 치트 코드 순서
        /// </summary>
        private KeyCode[] cheatCode = new KeyCode[]
        {
            KeyCode.UpArrow, KeyCode.UpArrow, KeyCode.DownArrow, KeyCode.DownArrow,
            KeyCode.LeftArrow, KeyCode.RightArrow, KeyCode.LeftArrow, KeyCode.RightArrow
        };

        /// <summary>
        /// 현재 입력된 치트 코드 인덱스
        /// </summary>
        private int cheatCodeIndex = 0;

        /// <summary>
        /// 치트 코드 입력 시, 치트가 적용되도록 하는 로직
        /// </summary>
        private void Update()
        {
            if (Input.GetKeyDown(cheatCode[cheatCodeIndex]))
            {
                cheatCodeIndex++;

                if (cheatCodeIndex >= cheatCode.Length)
                {
#if UNITY_EDITOR
                    Debug.Log("All stages unlocked!");
#endif
                    LevelManager.UnlockAllStages();
                    AudioManager.Instance.PlaySFX("UIConfirm");
                    LoadSceneManager.LoadScene("TitleScene");
                    cheatCodeIndex = 0;
                }
            }
            else if (Input.anyKeyDown)
            {
                cheatCodeIndex = 0;
            }
        }
    }
}
