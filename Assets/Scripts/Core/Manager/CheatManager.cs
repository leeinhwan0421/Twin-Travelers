using UnityEngine;

public class CheatCodeManager : MonoBehaviour
{
    private KeyCode[] cheatCode = new KeyCode[]
    {
        KeyCode.UpArrow, KeyCode.UpArrow, KeyCode.DownArrow, KeyCode.DownArrow,
        KeyCode.LeftArrow, KeyCode.RightArrow, KeyCode.LeftArrow, KeyCode.RightArrow
    };

    private int cheatCodeIndex = 0;

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
