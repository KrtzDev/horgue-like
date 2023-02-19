using System.Collections.Generic;
using UnityEngine;

public class CraftingMenu : MonoBehaviour
{
	[SerializeField]
	private RewardUI rewardUI_prefab;
	[SerializeField]
	private RectTransform rewardedWeaponPartsParent;

	public void PopulateRewardUI(List<Reward> rewards)
	{
		foreach (Reward reward in rewards)
		{
			RewardUI rewardUI = Instantiate(rewardUI_prefab,rewardedWeaponPartsParent);
			rewardUI.RewardImage.sprite = reward.weaponPartReward.WeaponPartUISprite;
		}
	}
}
