using UnityEngine;

public class PauseMenu : MonoBehaviour
{
	public void MainMenu()
	{
		Time.timeScale = 1;
		GameManager.Instance._gameIsPaused = false;
		UIManager.Instance.PauseMenu.gameObject.SetActive(false);
		SceneLoader.Instance.LoadScene(1); 
	}

	public void Continue()
    {
		if(GameManager.Instance._currentAbility != null)
        {
			Time.timeScale = 1;
		}
		GameManager.Instance._gameIsPaused = false;
		UIManager.Instance.PauseMenu.gameObject.SetActive(false);
	}
}
