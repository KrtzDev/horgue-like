using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : Singleton<SceneLoader>
{
    public Action CompletedSceneLoad;

    public SceneFader SceneFader { get; private set; }

    [SerializeField]
    private SceneFader _sceneFaderUI_prefab;

    private Scene _currentScene;
    private int _sceneToLoad;

    protected override void Awake()
    {
        base.Awake();
        SceneFader = Instantiate(_sceneFaderUI_prefab);
    }

    public void LoadScene(string sceneToLoad)
    {
        _sceneToLoad = SceneManager.GetSceneByName(sceneToLoad).buildIndex;
        _currentScene = SceneManager.GetActiveScene();

        InputManager.Instance.DisableCharacterInputs();

        StartCoroutine(FadeOut());
    }

    public void LoadScene(int sceneToLoad)
    {
        _sceneToLoad = sceneToLoad;
        _currentScene = SceneManager.GetActiveScene();

        InputManager.Instance.DisableCharacterInputs();

        StartCoroutine(FadeOut());
    }

    private IEnumerator FadeOut()
    {
        WaitForSeconds waitTime = new WaitForSeconds(SceneFader.FadeOut());
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

        InputManager.Instance.EnableCharacterInputs();

        CompletedSceneLoad?.Invoke();

        StartCoroutine(FadeIn());
    }

    private IEnumerator FadeIn()
    {
        WaitForSeconds waitTime = new WaitForSeconds(SceneFader.FadeIn());
        yield return waitTime;
    }
}
