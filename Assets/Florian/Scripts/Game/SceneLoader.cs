using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : Singleton<SceneLoader>
{
    private Scene _currentScene;
    private int _sceneToLoad;

    public void LoadScene(int sceneToLoad)
    {
        _sceneToLoad = sceneToLoad;
        _currentScene = SceneManager.GetActiveScene();

        InputManager.Instance.PlayerInputActions.Disable();

        StartCoroutine(FadeOut());
    }

    private IEnumerator FadeOut()
    {
        WaitForSeconds waitTime = new WaitForSeconds(UIManager.Instance.SceneFader.FadeOut());
        yield return waitTime;
        UnloadScene();
    }

    private void UnloadScene()
    {
        SceneManager.UnloadSceneAsync(_currentScene).completed += OnUnloadCompleted;
    }

    private void OnUnloadCompleted(AsyncOperation asyncOperation)
    {
        SceneManager.LoadSceneAsync(_sceneToLoad, LoadSceneMode.Additive).completed += OnLoadCompleted;
    }

    private void OnLoadCompleted(AsyncOperation asyncOperation)
    {
        SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(_sceneToLoad));

        InputManager.Instance.PlayerInputActions.Enable();

        StartCoroutine(FadeIn());
    }

    private IEnumerator FadeIn()
    {
        WaitForSeconds waitTime = new WaitForSeconds(UIManager.Instance.SceneFader.FadeIn());
        yield return waitTime;
    }
}
