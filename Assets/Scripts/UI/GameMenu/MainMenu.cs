using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
	[SerializeField] private Button _controlButton;
	[SerializeField] private Button _creditsButton;
	[SerializeField] private Button _backButtonControls;
	[SerializeField] private Button _backButtonCredits;
	[SerializeField] private GameObject _controlScheme;
	[SerializeField] private GameObject _credits;

	private bool _controlSchemeActive;
	private bool _creditsActive;

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
		SceneLoader.Instance.LoadScene(_levelIndex);
	}

	
	public void EquipWeaponsForBoss()
    {
		GameManager.Instance.bossCheat = true;
    }

	public void StartCredits()
	{
		_creditsActive= !_creditsActive;

		if (_creditsActive)
		{
			_credits.SetActive(true);
			_backButtonCredits.gameObject.SetActive(true);
			_backButtonCredits.interactable = true;
			_backButtonCredits.Select();
		}
		else
		{
			_credits.SetActive(false);
			_creditsButton.Select();
			_backButtonControls.gameObject.SetActive(false);
		}
	}

	public void DeactivateCreditsButton()
    {
		_backButtonCredits.interactable = false;
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
			_backButtonControls.gameObject.SetActive(true);
            _backButtonControls.Select();
		}
		else
        {
			_controlScheme.SetActive(false);
			_controlButton.Select();
			_backButtonControls.gameObject.SetActive(false);
		}
	}

}
