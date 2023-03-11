using UnityEngine;
using UnityEngine.EventSystems;

public class FirstSelectedSetter : MonoBehaviour
{

	private void OnEnable()
	{
		if(SceneLoader.Instance)
			SceneLoader.Instance.CompletedSceneLoad += OnSceneLoaded;
	}

	private void OnDisable()
	{
		if(SceneLoader.Instance)
		SceneLoader.Instance.CompletedSceneLoad -= OnSceneLoaded;
	}

	private void OnSceneLoaded()
	{
		EventSystem.current.firstSelectedGameObject = gameObject;
		EventSystem.current.SetSelectedGameObject(gameObject);
	}
}
