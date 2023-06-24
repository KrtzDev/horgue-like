using UnityEngine;

public class MainMenu : MonoBehaviour
{
	public void StartGame()
	{
		SceneLoader.Instance.LoadScene(2);
	}

	public void LevelButton(int _levelIndex)
	{
		SceneLoader.Instance.LoadScene(_levelIndex);
	}

	public void Quit()
	{
		Application.Quit();
	}

}
