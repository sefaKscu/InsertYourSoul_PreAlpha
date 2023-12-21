using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace InsertYourSoul
{
    public class ShowSceneName : MonoBehaviour
    {
        private Text sceneName;
        private void Awake()
        {
            sceneName = GetComponent<Text>();
            sceneName.text = SceneManager.GetActiveScene().name;
        }
    }
}
