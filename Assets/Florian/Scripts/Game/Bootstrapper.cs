using System.Collections.Generic;
using System.Linq;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.SceneManagement;

public class Bootstrapper
{
    private const string INIT_SCENE_NAME = "SCENE_Init";

    private static List<string> activeEditorScenes = new List<string>();
#if UNITY_EDITOR 

    [InitializeOnEnterPlayMode]
    private static void OnEnterPlayMode()
    {
        Debug.Log("Ping");
        EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
    }

    private static void OnPlayModeStateChanged(PlayModeStateChange state)
    {
        Debug.Log("Ping2");
        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            if(SceneManager.GetSceneAt(i).name != INIT_SCENE_NAME)
                activeEditorScenes.Add(SceneManager.GetSceneAt(i).name);
        }

        if (state == PlayModeStateChange.EnteredPlayMode)
        {
            SceneManager.LoadSceneAsync(INIT_SCENE_NAME).completed += LoadGameplayScene;
        }
    }

    private static void LoadGameplayScene(AsyncOperation asyncOperation)
    {
        if (activeEditorScenes.Count > 0)
        {
            foreach (var scene in activeEditorScenes)
            {
                SceneManager.LoadSceneAsync(scene, LoadSceneMode.Additive).completed += SetGameplaySceneActive;
            }
        }
    }

    private static void SetGameplaySceneActive(AsyncOperation asyncOperation)
    {
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(activeEditorScenes.Last()));
        SceneLoader.Instance.CompletedSceneLoad.Invoke();
    }

    ~Bootstrapper()
    {
        EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
    }
#else
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    private static void LoadMainMenuScene()
    {
        SceneManager.LoadSceneAsync("SCENE_Main_Menu",LoadSceneMode.Additive).completed += SetGameplaySceneActive;
        activeEditorScenes.Add("SCENE_Main_Menu");
    }

    private static void SetGameplaySceneActive(AsyncOperation asyncOperation)
    {
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(activeEditorScenes.Last()));
    }
#endif
}