using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RewardManager : Singleton<RewardManager>
{
	public List<Weapon> equippedWeapons = new List<Weapon>();
	public List<Reward> drawnRewards = new List<Reward>();

	[SerializeField]
	private List<WeaponPart> _WeaponPartRewards = new List<WeaponPart>();

	public Reward GetRandomReward()
	{
		Reward drawnReward = new Reward(_WeaponPartRewards[Random.Range(0, _WeaponPartRewards.Count - 1)]);
		drawnRewards.Add(drawnReward);
		return drawnReward;
	}

	public void ClearRewards()
	{
		drawnRewards.Clear();
	}

	private void Start()
	{
		SceneLoader.Instance.CompletedSceneLoad += OnCompletedSceneLoad;
	}

	private async void OnCompletedSceneLoad()
	{
		await Task.Delay(10);
		if (SceneManager.GetActiveScene().name == "SCENE_Weapon_Crafting")
		{
			UIManager.Instance.CraftingMenu.PopulateRewardUI(drawnRewards);
			UIManager.Instance.CraftingMenu.PopulateWeaponUI();
		}
		else if (	SceneManager.GetActiveScene().name == "SCENE_Level_00" ||
					SceneManager.GetActiveScene().name == "SCENE_Level_00" ||
					SceneManager.GetActiveScene().name == "SCENE_Level_00")
		{
			equippedWeapons = GameObject.Find("P_PlayerCharacter 1").GetComponent<WeaponHolster>().weapons;
		}

		ClearRewards();
	}
}
