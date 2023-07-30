using UnityEngine;

public class NextLevelButton : MonoBehaviour
{
	public void StartNextLevel()
	{
		if(GameManager.Instance._newGamePlus)
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
