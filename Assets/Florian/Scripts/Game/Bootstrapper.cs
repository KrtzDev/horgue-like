#if UNITY_EDITOR
using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

[InitializeOnLoad]
public class Bootstrapper
{
    private const string INIT_SCENE_NAME = "SCENE_Init";

    private static string activeEditorScene;

    static Bootstrapper()
    {
        EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
    }

    private static void OnPlayModeStateChanged(PlayModeStateChange state)
    {
        activeEditorScene = SceneManager.GetActiveScene().name;

        if (state == PlayModeStateChange.EnteredPlayMode)
        {
            SceneManager.LoadScene(INIT_SCENE_NAME);

            if (activeEditorScene != String.Empty)
            {
                SceneManager.LoadSceneAsync(activeEditorScene, LoadSceneMode.Additive).completed += SetGameplaySceneActive;
            }
        }
    }

    private static void SetGameplaySceneActive(AsyncOperation obj)
    {
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(activeEditorScene));
    }

    ~Bootstrapper()
    {
        Debug.Log("destroyed Bootstraper");
        EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
    }
}
#endif