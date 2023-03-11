using System.Collections.Generic;
using UnityEngine;

public class CraftingMenu : MonoBehaviour
{
	[HideInInspector]
	public List<WeaponUI> weaponUIs = new List<WeaponUI>();

	[Header("Weapon")]
	[SerializeField]
	private WeaponUI _weaponUI_prefab;
	[SerializeField]
	private RectTransform _weaponUIParent;

	[Header("Inventory")]
	[SerializeField]
	private RewardUI _rewardUI_prefab;
	[SerializeField]
	private RectTransform _rewardedWeaponPartsParent;

	public void PopulateWeaponUI()
	{
		foreach (Weapon weapon in RewardManager.Instance.equippedWeapons)
		{
			WeaponUI weaponUI = Instantiate(_weaponUI_prefab, _weaponUIParent);
			weaponUI.Initialize(weapon);
			weaponUIs.Add(weaponUI);
		}
	}

	public void PopulateRewardUI(List<Reward> rewards)
	{
		foreach (Reward reward in rewards)
		{
			RewardUI rewardUI = Instantiate(_rewardUI_prefab,_rewardedWeaponPartsParent);
			rewardUI.Initialize(reward);
		}
	}
}
