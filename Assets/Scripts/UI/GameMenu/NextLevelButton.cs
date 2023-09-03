using UnityEngine;

public class NextLevelButton : MonoBehaviour
{
	public void StartNextLevel()
	{
		if (GameManager.Instance._newGamePlus)
		{
			int random = GameManager.Instance.lastLevelNumbers.y;

			while (random == GameManager.Instance.lastLevelNumbers.y)
			{
				random = Random.Range(GameManager.Instance.firstLevelNumberInBuild, GameManager.Instance.lastLevelNumberInBuild + 1);
			}

			GameManager.Instance.lastLevelNumbers.x = GameManager.Instance.lastLevelNumbers.y;
			GameManager.Instance.lastLevelNumbers.y = random;

			SceneLoader.Instance.LoadScene(random);
		}
		else
		{
			SceneLoader.Instance.LoadScene(GameManager.Instance._currentLevel + 1);
		}
	}
}
