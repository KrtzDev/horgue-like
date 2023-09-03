using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
	[SerializeField] private Button _controlButton;
	[SerializeField] private Button _backButton;
	[SerializeField] private GameObject _controlScheme;

	private bool _controlSchemeActive;

    private void Start()
    {
		Time.timeScale = 1;
		_controlSchemeActive = false;

		if(!AudioManager.Instance.IsSoundPlaying("Theme"))
			AudioManager.Instance.PlaySound("Theme");
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

	public void Controls()
    {
		_controlSchemeActive = !_controlSchemeActive;

		if(_controlSchemeActive)
        {
			_controlScheme.SetActive(true);
			_backButton.gameObject.SetActive(true);
            _backButton.Select();
		}
		else
        {
			_controlScheme.SetActive(false);
			_controlButton.Select();
			_backButton.gameObject.SetActive(false);
		}
	}

}
