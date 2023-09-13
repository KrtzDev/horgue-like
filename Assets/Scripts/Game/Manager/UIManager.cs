using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

public class UIManager : Singleton<UIManager>
{

	public Endscreen Endscreen { get; private set; }
	public WaveEndScreen WaveEndScreen { get; private set; }
	public PauseMenu PauseMenu { get; private set; }
	public CraftingMenu CraftingMenu { get; private set; }
	public GameObject GameUI { get; private set; }
	public GameObject JetPackUI { get; private set; }
	public ChooseAbility ChooseAbility { get; private set; }


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
	private WeaponPartUI _rewardUI_prefab;
	[SerializeField] private ChooseAbility _chooseAbility_prefab;
	[SerializeField] private AbilityUI _abilityUI_prefab;

	private void Start()
	{
		SceneLoader.Instance.CompletedSceneLoad += OnCompletedSceneLoad;
	}

	public void ShowLevelEndScreen(LevelStatus levelStatus)
	{
		StatsTracker.Instance.AddLevelStatsToTotal();

		switch (levelStatus)
		{
			case LevelStatus.Won:
				Endscreen.ShowWonStateUI();
				break;
			case LevelStatus.Lost:
				Endscreen.ShowLostStateUI();
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

	public void DisplayRewards(List<WeaponPart> rewards)
	{
		foreach (WeaponPart reward in rewards)
		{
			WeaponPartUI newReward = Instantiate(_rewardUI_prefab,Endscreen.RewardParent);
			newReward.Initialize(reward);
		}
	}

	private void OnCompletedSceneLoad()
	{
		if (SceneManager.GetActiveScene().name == "SCENE_Main_Menu")
			return;

		if (SceneManager.GetActiveScene().name == "SCENE_Weapon_Crafting")
		{
			//CraftingMenu = Instantiate(_craftingMenuUI_prefab);
			//CraftingMenu.gameObject.SetActive(true);
			return;
		}

		if(SceneManager.GetActiveScene().name.StartsWith("SCENE_Level"))
		{
			PauseMenu = Instantiate(_pauseMenuUI_prefab);
			PauseMenu.gameObject.SetActive(false);
			Endscreen = Instantiate(_endScreenUI_prefab);
			Endscreen.gameObject.SetActive(false);
			WaveEndScreen = Instantiate(_waveEndScreenUI_prefab);
			WaveEndScreen.gameObject.SetActive(false);

			GameUI = Instantiate(_gameUI_prefab);
			GameUI.gameObject.SetActive(true);

			JetPackUI = GameUI.gameObject.GetComponentInChildren<UIImageFillAmount_Jetpack>().gameObject;
			JetPackUI.SetActive(false);

			ChooseAbility = Instantiate(_chooseAbility_prefab);
			ChooseAbility.gameObject.SetActive(true);
			ChooseAbility.Initialize();
		}
	}
}
