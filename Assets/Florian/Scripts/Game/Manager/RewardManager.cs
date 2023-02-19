using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RewardManager : Singleton<RewardManager>
{
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

	private void OnCompletedSceneLoad()
	{
		if (SceneManager.GetActiveScene().name == "SCENE_Weapon_Crafting")
		{
			UIManager.Instance.CraftingMenu.PopulateRewardUI(drawnRewards);
		}

		ClearRewards();
	}
}
