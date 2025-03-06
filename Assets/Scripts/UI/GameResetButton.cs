using TwinTravelers.Management;
using UnityEngine;

namespace TwinTravelers.UI
{
    public class GameResetButton : MonoBehaviour
    {
        public void ResetGameProgress()
        {
#if UNITY_EDITOR
            Debug.Log("Game progress reset triggered. Returning to Title Scene...");
#endif

            LevelManager.ResetProgress();
            LevelManager.LoadProgress();

            LoadSceneManager.LoadScene("TitleScene");
        }
    }
}
