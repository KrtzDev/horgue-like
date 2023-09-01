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
		GameManager.Instance._currentLevel = _levelIndex - 1;
		GameManager.Instance._currentLevelArray = _levelIndex - 2;
		SceneLoader.Instance.LoadScene(_levelIndex);
	}

	public void EquipWeaponsForBoss()
    {
		GameManager.Instance.bossCheat = true;
    }

	public void Quit()
	{
		Application.Quit();
	}

}
