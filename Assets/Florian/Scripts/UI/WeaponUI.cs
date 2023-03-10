﻿using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WeaponUI : MonoBehaviour
{
	[Header("Header")]
	[SerializeField]
	private TMP_Text _weaponName;

	[Header("Weapon Image")]
	[SerializeField]
	private Image _weaponImage;

	[Header("Weapon Parts")]
	[SerializeField]
	private WeaponPartSlot _ammunitionParent;
	[SerializeField]
	private WeaponPartSlot _barrelParent;
	[SerializeField]
	private WeaponPartSlot _gripParent;
	[SerializeField]
	private WeaponPartSlot _magazineParent;
	[SerializeField]
	private WeaponPartSlot _sightParent;
	[SerializeField]
	private WeaponPartSlot _triggerParent;

	[SerializeField]
	private RewardUI _rewardUI_prefab;

	[Header("Weapon Stats")]
	[SerializeField]
	private RectTransform _weaponStatsParent;
	[SerializeField]
	private StatUI _statUI_prefab;

	private Weapon _weapon;

	public void Initialize(Weapon weapon)
	{
		_weapon = weapon;

		ShowWeaponName();
		ShowWeaponBackground();
		ShowWeaponParts();
		InitializeWeaponSlots();

		ClearWeaponStats();
		ShowWeaponStats();
	}

	public bool SetNewWeaponPart(WeaponPart newWeaponPart, WeaponPartSlot weaponPartSlot)
	{
		if (newWeaponPart.GetType() == weaponPartSlot.GetCurrentSlottedWeaponPart().GetType() && !newWeaponPart.isSlotted)
		{
			if (newWeaponPart is Grip)
			{
				_weapon._grip = newWeaponPart as Grip;
			}
			else if (newWeaponPart is Barrel)
			{
				_weapon._barrel = newWeaponPart as Barrel;
			}
			else if (newWeaponPart is Magazine)
			{
				_weapon._magazine = newWeaponPart as Magazine;
			}
			else if (newWeaponPart is Ammunition)
			{
				_weapon._ammunition = newWeaponPart as Ammunition;
			}
			else if (newWeaponPart is TriggerMechanism)
			{
				_weapon._triggerMechanism = newWeaponPart as TriggerMechanism;
			}
			else if (newWeaponPart is Sight)
			{
				_weapon._sight = newWeaponPart as Sight;
			}

			_weapon.Initialize(_weapon.OwningTransform);

			Initialize(_weapon);

			return true;
		}
		return false;
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

		rewardUI = Instantiate(_rewardUI_prefab, _ammunitionParent.transform);
		Reward ammo = new Reward(_weapon._ammunition);
		rewardUI.Initialize(ammo);
		rewardUI = Instantiate(_rewardUI_prefab, _barrelParent.transform);
		Reward barrel = new Reward(_weapon._barrel);
		rewardUI.Initialize(barrel);
		rewardUI = Instantiate(_rewardUI_prefab, _gripParent.transform);
		Reward grip = new Reward(_weapon._grip);
		rewardUI.Initialize(grip);
		rewardUI = Instantiate(_rewardUI_prefab, _magazineParent.transform);
		Reward mag = new Reward(_weapon._magazine);
		rewardUI.Initialize(mag);
		rewardUI = Instantiate(_rewardUI_prefab, _sightParent.transform);
		Reward sight = new Reward(_weapon._sight);
		rewardUI.Initialize(sight);
		rewardUI = Instantiate(_rewardUI_prefab, _triggerParent.transform);
		Reward trigger = new Reward(_weapon._triggerMechanism);
		rewardUI.Initialize(trigger);
	}

	private void InitializeWeaponSlots()
	{
		_ammunitionParent.Initialize(this, _weapon._ammunition);
		_barrelParent.Initialize(this, _weapon._barrel);
		_gripParent.Initialize(this, _weapon._grip);
		_magazineParent.Initialize(this, _weapon._magazine);
		_sightParent.Initialize(this, _weapon._sight);
		_triggerParent.Initialize(this, _weapon._triggerMechanism);
	}

	private void ClearWeaponStats()
	{
		for (int i = 0; i < _weaponStatsParent.childCount; i++)
		{
			Destroy(_weaponStatsParent.GetChild(i).gameObject);
		}
	}

	private void ShowWeaponStats()
	{
		StatUI currenStat = Instantiate(_statUI_prefab, _weaponStatsParent);
		currenStat.Initialize("Damage: ", _weapon.PossibleProjectile.finalBaseDamage.ToString());
		currenStat = Instantiate(_statUI_prefab, _weaponStatsParent);
		currenStat.Initialize("Attack Speed: ", _weapon.PossibleProjectile.finalAttackSpeed.ToString());
		currenStat = Instantiate(_statUI_prefab, _weaponStatsParent);
		currenStat.Initialize("Reload Time: ", _weapon.PossibleProjectile.finalCooldown.ToString());
		currenStat = Instantiate(_statUI_prefab, _weaponStatsParent);
		currenStat.Initialize("Projectile Size: ", _weapon.PossibleProjectile.finalProjectileSize.ToString());
		currenStat = Instantiate(_statUI_prefab, _weaponStatsParent);
		currenStat.Initialize("Crit Chance: ", _weapon.PossibleProjectile.finalCritChance.ToString());
		currenStat = Instantiate(_statUI_prefab, _weaponStatsParent);
		currenStat.Initialize("Range: ", _weapon.PossibleProjectile.finalRange.ToString());
		currenStat = Instantiate(_statUI_prefab, _weaponStatsParent);
		currenStat.Initialize("Capacity: ", _weapon._magazine.capacity.ToString());
		currenStat = Instantiate(_statUI_prefab, _weaponStatsParent);
		currenStat.Initialize("Projectile trajectory: ", _weapon.PossibleProjectile.attackPattern.PatternName().ToString());
	}
}