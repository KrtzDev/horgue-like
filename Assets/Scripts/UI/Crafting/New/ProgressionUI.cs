using UnityEngine;

public class ProgressionUI : MonoBehaviour
{
	[SerializeField]
	private HoldButton _nextLevelButton;

	private void Awake()
	{
		_nextLevelButton.OnButtonExecute += LoadNextLevel;
	}

	private void LoadNextLevel()
	{
		if (GameManager.Instance._newGamePlus)
		{
			int random = Random.Range(2, 10);
			SceneLoader.Instance.LoadScene(random);
		}
		else
		{
			SceneLoader.Instance.LoadScene(GameManager.Instance._currentLevel + 1);
		}
	}
}
