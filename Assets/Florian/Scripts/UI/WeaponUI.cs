using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WeaponUI : MonoBehaviour
{
	[SerializeField]
	private Image _weaponImage;
	[SerializeField]
	private TMP_Text _weaponName;

	[SerializeField]
	private RectTransform _ammunitionParent;
	[SerializeField]
	private RectTransform _barrelParent;
	[SerializeField]
	private RectTransform _gripParent;
	[SerializeField]
	private RectTransform _magazineParent;
	[SerializeField]
	private RectTransform _sightParent;
	[SerializeField]
	private RectTransform _triggerParent;

	[SerializeField]
	private RewardUI _rewardUI_prefab;

	private Weapon _weapon;

	public void Initialize(Weapon weapon)
	{
		_weapon = weapon;
		ShowWeaponName();
		//ShowWeaponBackground();
		ShowWeaponParts();
	}

	private void ShowWeaponName()
	{
		_weaponName.text = _weapon.name;
	}

	private void ShowWeaponBackground()
	{
		_weaponImage.sprite = _weapon._weaponSprite;	
	}

	private void ShowWeaponParts()
	{
		RewardUI rewardUI;

		rewardUI = Instantiate(_rewardUI_prefab,_ammunitionParent);
		Reward ammo = new Reward(_weapon._ammunition);
		rewardUI.Initialize(ammo);
		rewardUI = Instantiate(_rewardUI_prefab, _barrelParent);
		Reward barrel = new Reward(_weapon._barrel);
		rewardUI.Initialize(barrel);
		rewardUI = Instantiate(_rewardUI_prefab, _gripParent);
		Reward grip = new Reward(_weapon._grip);
		rewardUI.Initialize(grip);
		rewardUI = Instantiate(_rewardUI_prefab, _magazineParent);
		Reward mag = new Reward(_weapon._magazine);
		rewardUI.Initialize(mag);
		rewardUI = Instantiate(_rewardUI_prefab, _sightParent);
		Reward sight = new Reward(_weapon._sight);
		rewardUI.Initialize(sight);
		rewardUI = Instantiate(_rewardUI_prefab, _triggerParent);
		Reward trigger = new Reward(_weapon._triggerMechanism);
		rewardUI.Initialize(trigger);
	}
}