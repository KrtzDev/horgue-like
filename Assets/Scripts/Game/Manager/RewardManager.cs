using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RewardManager : Singleton<RewardManager>
{
	public List<Weapon> equippedWeapons = new List<Weapon>();
	public List<WeaponPart> drawnRewards = new List<WeaponPart>();

	[SerializeField]
	private List<WeaponPart> _weaponPartRewards = new List<WeaponPart>();

	public WeaponPart GetRandomReward()
	{
		WeaponPart drawnReward = _weaponPartRewards[Random.Range(0, _weaponPartRewards.Count - 1)];
		WeaponPart newReward = Instantiate(drawnReward);
		newReward.levelObtained = GameManager.Instance._currentLevel;
		ScaleWeaponPartToLevel(newReward);
		drawnRewards.Add(newReward);
		return newReward;
	}

	private void ScaleWeaponPartToLevel(WeaponPart weaponPart)
	{
		if(weaponPart.baseDamage > 0)
		{
			weaponPart.baseDamage *= 1 + GameManager.Instance.GameManagerValues[0]._weaponPartMultiplierPerLevel * (weaponPart.levelObtained - 1);
		}

		if(weaponPart.attackSpeed > 0)
		{
			weaponPart.attackSpeed *= 1 + GameManager.Instance.GameManagerValues[0]._weaponPartMultiplierPerLevel * (weaponPart.levelObtained - 1);
		}

		/* 
		if(weaponPart.cooldown < 0)
		{
			weaponPart.cooldown *=  1 + GameManager.Instance.GameManagerValues[0]._weaponPartMultiplierPerLevel *  (weaponPart.levelObtained - 1);
		}

		if(weaponPart.projectileSize > 0)
		{
			weaponPart.projectileSize *=  1 + GameManager.Instance.GameManagerValues[0]._weaponPartMultiplierPerLevel *  (weaponPart.levelObtained - 1);
		}
		*/

		if(weaponPart.critChance > 0)
		{
			weaponPart.critChance *=  1 + GameManager.Instance.GameManagerValues[0]._weaponPartMultiplierPerLevel * (weaponPart.levelObtained - 1);
		}

		if(weaponPart.critDamage > 0)
		{
			weaponPart.critDamage *=  1 + GameManager.Instance.GameManagerValues[0]._weaponPartMultiplierPerLevel *  (weaponPart.levelObtained - 1);
		}

		if(weaponPart.range > 0)
		{
			weaponPart.range *=  1 + GameManager.Instance.GameManagerValues[0]._weaponPartMultiplierPerLevel * (weaponPart.levelObtained - 1);
		}

		// check if over cap ?

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
			//UIManager.Instance.CraftingMenu.PopulateRewardUI(drawnRewards);
			//UIManager.Instance.CraftingMenu.PopulateWeaponUI();
		}
		else if (	SceneManager.GetActiveScene().name == "SCENE_Level_00" ||
					SceneManager.GetActiveScene().name == "SCENE_Level_00" ||
					SceneManager.GetActiveScene().name == "SCENE_Level_00")
		{
			equippedWeapons = GameObject.FindObjectOfType<PlayerCharacter>().GetComponent<WeaponHolster>().weapons;
		}

		ClearRewards();
	}
}
