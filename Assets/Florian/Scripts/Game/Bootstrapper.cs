#if UNITY_EDITOR
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Bootstrapper
{
    private const string INIT_SCENE_NAME = "SCENE_Init";

    private static List<string> activeEditorScenes = new List<string>();

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
            SceneManager.LoadScene(INIT_SCENE_NAME);

            if (activeEditorScenes.Count > 0)
            {
                foreach (var scene in activeEditorScenes)
                {
                    SceneManager.LoadSceneAsync(scene, LoadSceneMode.Additive).completed += SetGameplaySceneActive;
                }
            }
        }
    }

    private static void SetGameplaySceneActive(AsyncOperation obj)
    {
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(activeEditorScenes.Last()));
    }

    ~Bootstrapper()
    {
        EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
    }
}
#endif