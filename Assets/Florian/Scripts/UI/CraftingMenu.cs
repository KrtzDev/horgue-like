using System.Collections.Generic;
using UnityEngine;

public class CraftingMenu : MonoBehaviour
{

	[SerializeField]
	private WeaponUI _weaponUI_prefab;
	[SerializeField]
	private List<RectTransform> _weaponUIParents;

	[SerializeField]
	private RewardUI _rewardUI_prefab;
	[SerializeField]
	private RectTransform _rewardedWeaponPartsParent;

	public void PopulateWeaponUI()
	{
		for (int i = 0; i < _weaponUIParents.Count; i++)
		{
			RectTransform parent = _weaponUIParents[i];
			WeaponUI weaponUI = Instantiate(_weaponUI_prefab, parent);
			weaponUI.Initialize(RewardManager.Instance.equippedWeapons[i]);
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
