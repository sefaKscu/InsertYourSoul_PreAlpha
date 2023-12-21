using UnityEngine;

namespace InsertYourSoul
{
    public class ChangeSceneButton : MonoBehaviour
    {
        public void ChangeScene(string sceneName)
        {
            if (LevelManager.Instance != null)
                LevelManager.Instance.LoadScene(sceneName);
            else
                Debug.Log("<color=red> Level Manager is not available </color>");
        }
    }
}
