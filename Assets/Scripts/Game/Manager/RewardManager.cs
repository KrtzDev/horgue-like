using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RewardManager : Singleton<RewardManager>
{
	public List<Weapon> equippedWeapons = new List<Weapon>();
	public List<WeaponPart> drawnRewards = new List<WeaponPart>();

	[SerializeField]
	private List<WeaponPart> _weaponPartRewards = new List<WeaponPart>();

	private WeaponPart _lastDrawnReward;

	private Dictionary<Type, int> _weightBiasByPartType = new Dictionary<Type, int>();

	private void Start()
	{
		SceneLoader.Instance.CompletedSceneLoad += OnCompletedSceneLoad;

		UpdateTypeBiasWeightTable();
	}

	private void UpdateTypeBiasWeightTable()
	{
		if (_lastDrawnReward == null)
		{
			_weightBiasByPartType.Add(typeof(Grip), 16);
			_weightBiasByPartType.Add(typeof(Barrel), 20);
			_weightBiasByPartType.Add(typeof(Magazine), 16);
			_weightBiasByPartType.Add(typeof(Ammunition), 16);
			_weightBiasByPartType.Add(typeof(TriggerMechanism), 16);
			_weightBiasByPartType.Add(typeof(Sight), 16);
		}
		else
		{
			foreach (var partType in _weightBiasByPartType)
			{
				if (partType.Key == _lastDrawnReward.GetType())
					_weightBiasByPartType[partType.Key] -= 5;
				else
					_weightBiasByPartType[partType.Key]++;
			}
		}
	}

	private async void OnCompletedSceneLoad()
	{
		await Task.Delay(10);
		if (SceneManager.GetActiveScene().name == "SCENE_Level_00" ||
			SceneManager.GetActiveScene().name == "SCENE_Level_00" ||
			SceneManager.GetActiveScene().name == "SCENE_Level_00")
		{
			equippedWeapons = GameObject.FindObjectOfType<PlayerCharacter>().GetComponent<WeaponHolster>().weapons;
		}

		ClearRewards();
	}

	public WeaponPart GetReward()
	{
		WeaponPart drawnReward = GetBiasedPart();
		WeaponPart newReward = Instantiate(drawnReward);
		newReward.levelObtained = GameManager.Instance._currentLevel;
		ScaleWeaponPartToLevel(newReward);
		drawnRewards.Add(newReward);
		return newReward;
	}

	private WeaponPart GetRandomPart()
	{
		return _weaponPartRewards[UnityEngine.Random.Range(0, _weaponPartRewards.Count - 1)];
	}

	private WeaponPart GetBiasedPart()
	{
		foreach (var part in _weaponPartRewards)
		{
			ApplyTypeBias(part);
			ApplyRarityBias(part);
			ApplySpecificBias(part);
		}

		List<WeaponPart> _biasedrewards = _weaponPartRewards.OrderBy(e => e.weight).ToList();
		WeaponPart drawnReward = null;

		//with this loop mainly "common" items will be drawn as the rarity influnces the weight the most and high weighted Items are first in the List because the orderBy
		foreach (var part in _biasedrewards)
		{
			if (part.weight > UnityEngine.Random.Range(0, 100))
			{
				drawnReward = part;
				break;
			}
		}

		if (drawnReward == null)
			drawnReward = GetRandomPart();

		_lastDrawnReward = drawnReward;
		UpdateTypeBiasWeightTable();

		return drawnReward;
	}

	private void ApplyTypeBias(WeaponPart weaponPart)
	{
		weaponPart.weight = GetBiasWeightFromType(weaponPart);
	}

	private int GetBiasWeightFromType(WeaponPart weaponPart)
	{
		return _weightBiasByPartType[weaponPart.GetType()];
	}

	private void ApplyRarityBias(WeaponPart weaponPart)
	{

	}

	private void ApplySpecificBias(WeaponPart weaponPart)
	{

	}

	private void ScaleWeaponPartToLevel(WeaponPart weaponPart)
	{
		if (weaponPart.baseDamage > 0)
		{
			weaponPart.baseDamage *= 1 + GameManager.Instance.GameManagerValues[0]._weaponPartMultiplierPerLevel * (weaponPart.levelObtained - 1);
		}

		if (weaponPart.attackSpeed > 0)
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

		if (weaponPart.critChance > 0)
		{
			weaponPart.critChance *=  1 + GameManager.Instance.GameManagerValues[0]._weaponPartMultiplierPerLevel * (weaponPart.levelObtained - 1);
		}

		if (weaponPart.critDamage > 0)
		{
			weaponPart.critDamage *=  1 + GameManager.Instance.GameManagerValues[0]._weaponPartMultiplierPerLevel *  (weaponPart.levelObtained - 1);
		}

		if (weaponPart.range > 0)
		{
			weaponPart.range *=  1 + GameManager.Instance.GameManagerValues[0]._weaponPartMultiplierPerLevel * (weaponPart.levelObtained - 1);
		}

		if (weaponPart.cost > 0)
		{
			weaponPart.cost *= 1 + GameManager.Instance.GameManagerValues[0]._weaponPartMultiplierPerLevel * (weaponPart.levelObtained - 1);
			Mathf.RoundToInt(weaponPart.cost);
		}

		// check if over cap ?

	}

	public void ClearRewards()
	{
		drawnRewards.Clear();
	}
}
