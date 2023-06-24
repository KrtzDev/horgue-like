using UnityEngine;

public class NextLevelButton : MonoBehaviour
{
	public void StartNextLevel()
	{
		SceneLoader.Instance.LoadScene(GameManager.Instance._currentLevel + 1);
	}
}
