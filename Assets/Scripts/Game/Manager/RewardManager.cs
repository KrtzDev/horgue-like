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
	private Dictionary<string, float> _weightByRarity = new Dictionary<string, float>();

	private void Start()
	{
		SceneLoader.Instance.CompletedSceneLoad += OnCompletedSceneLoad;

		UpdateTypeBiasWeightTable();
		SetUpWeightByRarityTable();
	}

	private void UpdateTypeBiasWeightTable()
	{
		if (_lastDrawnReward == null)
		{
			_weightBiasByPartType.Clear();
			_weightBiasByPartType.Add(typeof(Grip), 16);
			_weightBiasByPartType.Add(typeof(Barrel), 20);
			_weightBiasByPartType.Add(typeof(Magazine), 16);
			_weightBiasByPartType.Add(typeof(Ammunition), 16);
			_weightBiasByPartType.Add(typeof(TriggerMechanism), 16);
			_weightBiasByPartType.Add(typeof(Sight), 16);
		}
		else
		{
			for (int i = 0; i < _weightBiasByPartType.Count; i++)
			{
				var partType = _weightBiasByPartType.ElementAt(i).Key;

				if (partType == _lastDrawnReward.GetType())
					_weightBiasByPartType[partType] -= 5;
				else
					_weightBiasByPartType[partType]++;
			}
		}
	}

	private void SetUpWeightByRarityTable()
	{
		_weightByRarity.Add("uncommon", 50);
		_weightByRarity.Add("rare", 28);
		_weightByRarity.Add("special", 17);
		_weightByRarity.Add("unique", 5);
	}

	public Color GetColorFromRarity(string rarity)
	{
		if (rarity == "uncommon")
			return new Color32(0x6A, 0xA8, 0x4F, 0xFF);
		else if (rarity == "rare")
			return new Color32(0x00, 0xcc, 0xff, 0xFF);
		else if(rarity == "special")
			return new Color32(0x9f, 0x00, 0xff, 0xFF);
		else if (rarity == "unique")
			return new Color32(0xe1, 0xbe, 0x18, 0xFF);
		else return Color.white;
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

	private WeaponPart GetBiasedPart()
	{
		List<WeaponPart> typeBiasedParts = GetTypeBiasedList();
		List<WeaponPart> typeAndRarityBiasedParts = GetRarityBiasedList(typeBiasedParts);
		WeaponPart drawnReward = GetFinalBiasedPart(typeAndRarityBiasedParts);

		_lastDrawnReward = drawnReward;
		UpdateTypeBiasWeightTable();

		return drawnReward;
	}

	private List<WeaponPart> GetTypeBiasedList()
	{
		KeyValuePair<Type, int> chosenType = new KeyValuePair<Type, int>(null, 0);
		double accumulatedWeight = 0;

		foreach (KeyValuePair<Type, int> partType in _weightBiasByPartType)
		{
			if (chosenType.Key == null)
			{
				chosenType = partType;
				accumulatedWeight = partType.Value;
				continue;
			}
			accumulatedWeight += partType.Value;

			double probabilityToChooseNewPart = (double)partType.Value/(accumulatedWeight + partType.Value) * 100;
			if (probabilityToChooseNewPart >= UnityEngine.Random.Range(0, 100))
			{
				chosenType = partType;
			}
		}

		return _weaponPartRewards.FindAll(e => e.GetType() == chosenType.Key).ToList();
	}

	private List<WeaponPart> GetRarityBiasedList(List<WeaponPart> weaponParts)
	{
		foreach (var weaponPart in weaponParts)
		{
			weaponPart.weight = GetWeigthByRarity(weaponPart.rarity);
		}

		return weaponParts;
	}

	private float GetWeigthByRarity(string rarity)
	{
		return _weightByRarity[rarity];
	}

	private WeaponPart GetFinalBiasedPart(List<WeaponPart> weaponParts)
	{
		WeaponPart chosenPart = null;
		double accumulatedWeight = 0;

		foreach (var weaponpart in weaponParts)
		{
			if (chosenPart == null)
			{
				chosenPart = weaponpart;
				accumulatedWeight = weaponpart.weight;
				continue;
			}
			accumulatedWeight += weaponpart.weight;

			double probabilityToChooseNewPart = weaponpart.weight/(accumulatedWeight + weaponpart.weight) * 100;
			if (probabilityToChooseNewPart >= UnityEngine.Random.Range(0, 100))
			{
				chosenPart = weaponpart;
			}
		}
		return chosenPart;
	}

	public void ScaleWeaponPartToLevel(WeaponPart weaponPart)
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
			weaponPart.cost *= 1 + GameManager.Instance.GameManagerValues[0]._weaponPartMultiplierPerLevel * 3 * (weaponPart.levelObtained - 1);
			Mathf.RoundToInt(weaponPart.cost);
		}

		// check if over cap ?

	}

	public void ClearRewards()
	{
		drawnRewards.Clear();
	}
}
