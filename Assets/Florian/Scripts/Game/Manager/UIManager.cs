using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : Singleton<UIManager>
{

	public Endscreen Endscreen { get; private set; }
	public WaveEndScreen WaveEndScreen { get; private set; }
	public PauseMenu PauseMenu { get; private set; }
	public CraftingMenu CraftingMenu { get; private set; }
	public GameObject GameUI { get; private set; }


	[SerializeField]
	private Endscreen _endScreenUI_prefab;
	[SerializeField]
	private WaveEndScreen _waveEndScreenUI_prefab;
	[SerializeField]
	private PauseMenu _pauseMenuUI_prefab;
	[SerializeField]
	private CraftingMenu _craftingMenuUI_prefab;
	[SerializeField]
	private GameObject _gameUI_prefab;
	[SerializeField]
	private RewardUI _rewardUI_prefab;

	private void Start()
	{
		SceneLoader.Instance.CompletedSceneLoad += OnCompletedSceneLoad;
	}

	public void ShowLevelEndScreen(LevelStatus levelStatus)
	{
		switch (levelStatus)
		{
			case LevelStatus.Won:
				Endscreen.TitleText.text = "Won";
				break;
			case LevelStatus.Lost:
				Endscreen.TitleText.text = "Lost";
				break;
			default:
				break;
		}
		Endscreen.gameObject.SetActive(true);
	}

	public void ShowWaveEndScreen(LevelStatus levelStatus)
	{
		switch (levelStatus)
		{
			case LevelStatus.Won:
				WaveEndScreen.TitleText.text = "Wave Results";
				break;
			case LevelStatus.Lost:
				WaveEndScreen.TitleText.text = "Lost";
				break;
			default:
				break;
		}
		WaveEndScreen.gameObject.SetActive(true);
	}

	public void DisplayRewards(List<Reward> rewards)
	{
		foreach (Reward reward in rewards)
		{
			RewardUI newReward = Instantiate(_rewardUI_prefab,Endscreen.RewardParent);
			newReward.RewardImage.sprite = reward.weaponPartReward.WeaponPartUISprite;
		}
	}

	private void OnCompletedSceneLoad()
	{
		if (SceneManager.GetActiveScene().name == "SCENE_Main_Menu")
			return;

		if (SceneManager.GetActiveScene().name == "SCENE_Weapon_Crafting")
		{
			CraftingMenu = Instantiate(_craftingMenuUI_prefab);
			CraftingMenu.gameObject.SetActive(true);
			return;
		}

		PauseMenu = Instantiate(_pauseMenuUI_prefab);
		PauseMenu.gameObject.SetActive(false);
		Endscreen = Instantiate(_endScreenUI_prefab);
		Endscreen.gameObject.SetActive(false);
		WaveEndScreen = Instantiate(_waveEndScreenUI_prefab);
		WaveEndScreen.gameObject.SetActive(false);
		GameUI = Instantiate(_gameUI_prefab);
		GameUI.gameObject.SetActive(true);
	}
}
