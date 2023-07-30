using UnityEngine;

public class MainMenu : MonoBehaviour
{
    private void Start()
    {
		Time.timeScale = 1;
	}

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
