using System.Collections.Generic;
using System.Linq;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
#endif
using UnityEngine;
using UnityEngine.SceneManagement;

[InitializeOnLoad]
public class Bootstrapper
{
	private const string INIT_SCENE_PATH = "Assets/Scenes/Game Scenes/SCENE_Init.unity";
	private const string INIT_SCENE_NAME = "SCENE_Init";

	private static List<string> _activeEditorScenes = new List<string>();

#if UNITY_EDITOR
	static Bootstrapper()
	{
		if (!EditorApplication.isPlayingOrWillChangePlaymode)
			EditorSceneManager.playModeStartScene = AssetDatabase.LoadAssetAtPath<SceneAsset>(INIT_SCENE_PATH);

		EditorApplication.playModeStateChanged += OnPlayModeStateChanged;

		_activeEditorScenes.Clear();
		for (int i = 0; i < SceneManager.sceneCount; i++)
		{
			if (SceneManager.GetSceneAt(i).name != INIT_SCENE_NAME)
				_activeEditorScenes.Add(SceneManager.GetSceneAt(i).name);
		}
	}

	private static void OnPlayModeStateChanged(PlayModeStateChange state)
	{
		if (state == PlayModeStateChange.EnteredPlayMode)
			LoadGameplayScene();
	}

	private static void LoadGameplayScene()
	{
		foreach (var scene in _activeEditorScenes)
			SceneManager.LoadSceneAsync(scene, LoadSceneMode.Additive).completed += SetGameplaySceneActive;
	}

	private static void SetGameplaySceneActive(AsyncOperation asyncOperation)
	{
		SceneManager.SetActiveScene(SceneManager.GetSceneByName(_activeEditorScenes.Last()));
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