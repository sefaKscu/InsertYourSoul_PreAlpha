using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace InsertYourSoul
{
    public class LevelManager : MonoBehaviour
    {
        public static LevelManager Instance;

        [SerializeField] private GameObject loaderCanvas;
        [SerializeField] private Image progressBar;

        private LoadingScreen loadingScreen;
        private float targetFillAmount;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
                loadingScreen = new LoadingScreen(loaderCanvas);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public async void LoadScene(string sceneName)
        {
            Reset();
            Debug.Log("<color=yellow> Loading level: </color>" + sceneName);
            var scene = SceneManager.LoadSceneAsync(sceneName);
            scene.allowSceneActivation = false;
            loadingScreen.ShowLoadingCanvas();

            do
            {
                await Task.Delay(100);
                targetFillAmount = scene.progress;
            }
            while (scene.progress < 0.9f);

            scene.allowSceneActivation = true;
            loadingScreen.HideLoadingCanvas();
            Debug.Log("<color=green> Level loaded succesfully!: </color>" + sceneName);
        }

        private void Reset()
        {
            targetFillAmount = 0f;
            progressBar.fillAmount = 0f;
        }

        private void Update()
        {
            UpdateProgressBar();
        }

        private void UpdateProgressBar()
        {
            progressBar.fillAmount = Mathf.MoveTowards(progressBar.fillAmount, targetFillAmount, 3 * Time.deltaTime);
        }


    }

    public class LoadingScreen
    {
        public LoadingScreen(GameObject loadingCanvas)
        {
            canvasGroup = loadingCanvas.GetComponent<CanvasGroup>();
            HideLoadingCanvas();
        }

        private CanvasGroup canvasGroup;

        public void ShowLoadingCanvas()
        {
            canvasGroup.alpha = 1f;
            canvasGroup.blocksRaycasts = true;
        }
        public void HideLoadingCanvas()
        {
            canvasGroup.alpha = 0f;
            canvasGroup.blocksRaycasts = false;
        }
    }
}
