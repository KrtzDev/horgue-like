﻿using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WeaponUI : MonoBehaviour
{
	public Weapon weapon;
	public RectTransform equipNewPartTransform;

	[Header("Header")]
	[SerializeField]
	private TMP_Text _weaponName;

	[Header("Weapon Image")]
	[SerializeField]
	private Image _weaponImage;
	[SerializeField]
	private Image _effectImage;

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
	private WeaponPartUI _rewardUI_prefab;

	[Header("Weapon Stats")]
	[SerializeField]
	private RectTransform _weaponStatsParent;
	[SerializeField]
	private StatUI _statUI_Prefab;
	[SerializeField]
	private StatUI _statUI_Projectile_Trajectory_Prefab;
	[SerializeField]
	private StatUI _statUI_DPS_Prefab;


	public void Initialize(Weapon weapon)
	{
		this.weapon = weapon;

		ShowWeaponName();
		ShowWeaponBackground();
		InitializeWeaponSlots();
		ShowWeaponParts();

		ShowWeaponStats(this.weapon.CalculateWeaponStats(this.weapon));
	}

	public bool SetNewWeaponPart(WeaponPartUI newWeaponPartUI)
	{
		if (!newWeaponPartUI.isSlotted)
		{
			if (newWeaponPartUI.weaponPart is Grip)
			{
				weapon.grip = newWeaponPartUI.weaponPart as Grip;
			}
			else if (newWeaponPartUI.weaponPart is Barrel)
			{
				weapon.barrel = newWeaponPartUI.weaponPart as Barrel;
			}
			else if (newWeaponPartUI.weaponPart is Magazine)
			{
				weapon.magazine = newWeaponPartUI.weaponPart as Magazine;
			}
			else if (newWeaponPartUI.weaponPart is Ammunition)
			{
				weapon.ammunition = newWeaponPartUI.weaponPart as Ammunition;
			}
			else if (newWeaponPartUI.weaponPart is TriggerMechanism)
			{
				weapon.triggerMechanism = newWeaponPartUI.weaponPart as TriggerMechanism;
			}
			else if (newWeaponPartUI.weaponPart is Sight)
			{
				weapon.sight = newWeaponPartUI.weaponPart as Sight;
			}

			newWeaponPartUI.isSlotted = true;
			weapon.Initialize(weapon.OwningTransform);

			Initialize(weapon);

			DoCraftingEffect();

			return true;
		}
		return false;
	}

	private void DoCraftingEffect()
	{
		Tween colorToWhite = _effectImage.DOColor(Color.white, .1f).SetEase(Ease.OutBounce);
		Tween ColorToNormal = _effectImage.DOColor(new Color(1, 1, 1, 0), .1f).SetEase(Ease.OutBounce);

		Sequence sequence = DOTween.Sequence();
		sequence.Append(colorToWhite)
			.Append(ColorToNormal);

		sequence.Play();
	}

	private void ShowWeaponName()
	{
		_weaponName.text = weapon.name;
	}

	private void ShowWeaponBackground()
	{
		_weaponImage.sprite = weapon.weaponSprite;
	}

	private void ShowWeaponParts()
	{
		WeaponPartUI rewardUI;

		rewardUI = Instantiate(_rewardUI_prefab, _ammunitionParent.transform);
		rewardUI.Initialize(weapon.ammunition);
		rewardUI.isSlotted = true;
		rewardUI.weaponUI = this;
		rewardUI = Instantiate(_rewardUI_prefab, _barrelParent.transform);
		rewardUI.Initialize(weapon.barrel);
		rewardUI.isSlotted = true;
		rewardUI.weaponUI = this;
		rewardUI = Instantiate(_rewardUI_prefab, _gripParent.transform);
		rewardUI.Initialize(weapon.grip);
		rewardUI.isSlotted = true;
		rewardUI.weaponUI = this;
		rewardUI = Instantiate(_rewardUI_prefab, _magazineParent.transform);
		rewardUI.Initialize(weapon.magazine);
		rewardUI.isSlotted = true;
		rewardUI.weaponUI = this;
		rewardUI = Instantiate(_rewardUI_prefab, _sightParent.transform);
		rewardUI.Initialize(weapon.sight);
		rewardUI.isSlotted = true;
		rewardUI.weaponUI = this;
		rewardUI = Instantiate(_rewardUI_prefab, _triggerParent.transform);
		rewardUI.Initialize(weapon.triggerMechanism);
		rewardUI.isSlotted = true;
		rewardUI.weaponUI = this;
	}

	private void InitializeWeaponSlots()
	{
		_ammunitionParent.Initialize(this, weapon.ammunition);
		_barrelParent.Initialize(this, weapon.barrel);
		_gripParent.Initialize(this, weapon.grip);
		_magazineParent.Initialize(this, weapon.magazine);
		_sightParent.Initialize(this, weapon.sight);
		_triggerParent.Initialize(this, weapon.triggerMechanism);
	}

	private void ClearWeaponStats()
	{
		for (int i = 0; i < _weaponStatsParent.childCount; i++)
			Destroy(_weaponStatsParent.GetChild(i).gameObject);
	}



	WeaponStats _previousWeaponStats;

	public void ShowWeaponStats(WeaponStats weaponStats, bool colorStats = false)
	{
		ClearWeaponStats();

		if (!colorStats)
		{
			StatUI currenStat = Instantiate(_statUI_DPS_Prefab, _weaponStatsParent);
			currenStat.Initialize("DPS: ", weaponStats.dps.ToString("0.00"));
			currenStat = Instantiate(_statUI_Prefab, _weaponStatsParent);
			currenStat.Initialize("Damage: ", weaponStats.damage.ToString("0.00"));
			currenStat = Instantiate(_statUI_Prefab, _weaponStatsParent);
			currenStat.Initialize("Attack Speed: ", weaponStats.attackspeed.ToString("0.00"));
			currenStat = Instantiate(_statUI_Prefab, _weaponStatsParent);
			currenStat.Initialize("Reload Time: ", weaponStats.cooldown.ToString("0.00"));
			currenStat = Instantiate(_statUI_Prefab, _weaponStatsParent);
			currenStat.Initialize("Projectile Size: ", weaponStats.projectileSize.ToString("0.00"));
			currenStat = Instantiate(_statUI_Prefab, _weaponStatsParent);
			currenStat.Initialize("Crit Chance: ", weaponStats.critChance.ToString("0.00"));
			currenStat = Instantiate(_statUI_Prefab, _weaponStatsParent);
			currenStat.Initialize("Crit Damage: ", weaponStats.critDamage.ToString("0.00"));
			currenStat = Instantiate(_statUI_Prefab, _weaponStatsParent);
			currenStat.Initialize("Range: ", weaponStats.range.ToString("0.00"));
			currenStat = Instantiate(_statUI_Prefab, _weaponStatsParent);
			currenStat.Initialize("Capacity: ", weaponStats.capacity.ToString("0.00"));
			currenStat = Instantiate(_statUI_Projectile_Trajectory_Prefab, _weaponStatsParent);
			currenStat.Initialize("Projectile Trajectory: ", weaponStats.attackPattern.PatternName());
			currenStat = Instantiate(_statUI_Prefab, _weaponStatsParent);
			if (weaponStats.statusEffect != null)
				currenStat.Initialize("Effect: ", weaponStats.statusEffect.StatusName());
			else
				currenStat.Initialize("Effect: ", "None");
		}
		else if (_previousWeaponStats != null && colorStats)
		{
			StatUI currenStat = Instantiate(_statUI_DPS_Prefab, _weaponStatsParent);
			currenStat.Initialize("DPS: ", weaponStats.dps.ToString("0.00"));
			if (weaponStats.dps > _previousWeaponStats.dps)
				currenStat.statBackground.color = currenStat.positiveColor;
			else if (weaponStats.dps < _previousWeaponStats.dps)
				currenStat.statBackground.color = currenStat.negativeColor;

			currenStat = Instantiate(_statUI_Prefab, _weaponStatsParent);
			currenStat.Initialize("Damage: ", weaponStats.damage.ToString("0.00"));
			if (weaponStats.damage > _previousWeaponStats.damage)
				currenStat.statBackground.color = currenStat.positiveColor;
			else if (weaponStats.damage < _previousWeaponStats.damage)
				currenStat.statBackground.color = currenStat.negativeColor;

			currenStat = Instantiate(_statUI_Prefab, _weaponStatsParent);
			currenStat.Initialize("Attack Speed: ", weaponStats.attackspeed.ToString("0.00"));
			if (weaponStats.attackspeed > _previousWeaponStats.attackspeed)
				currenStat.statBackground.color = currenStat.positiveColor;
			else if (weaponStats.attackspeed < _previousWeaponStats.attackspeed)
				currenStat.statBackground.color = currenStat.negativeColor;

			currenStat = Instantiate(_statUI_Prefab, _weaponStatsParent);
			currenStat.Initialize("Reload Time: ", weaponStats.cooldown.ToString("0.00"));
			if (weaponStats.cooldown < _previousWeaponStats.cooldown)
				currenStat.statBackground.color = currenStat.positiveColor;
			else if (weaponStats.cooldown > _previousWeaponStats.cooldown)
				currenStat.statBackground.color = currenStat.negativeColor;

			currenStat = Instantiate(_statUI_Prefab, _weaponStatsParent);
			currenStat.Initialize("Projectile Size: ", weaponStats.projectileSize.ToString("0.00"));
			if (weaponStats.projectileSize > _previousWeaponStats.projectileSize)
				currenStat.statBackground.color = currenStat.positiveColor;
			else if (weaponStats.projectileSize < _previousWeaponStats.projectileSize)
				currenStat.statBackground.color = currenStat.negativeColor;

			currenStat = Instantiate(_statUI_Prefab, _weaponStatsParent);
			currenStat.Initialize("Crit Chance: ", weaponStats.critChance.ToString("0.00"));
			if (weaponStats.critChance > _previousWeaponStats.critChance)
				currenStat.statBackground.color = currenStat.positiveColor;
			else if (weaponStats.critChance < _previousWeaponStats.critChance)
				currenStat.statBackground.color = currenStat.negativeColor;

			currenStat = Instantiate(_statUI_Prefab, _weaponStatsParent);
			currenStat.Initialize("Crit Damage: ", weaponStats.critDamage.ToString("0.00"));
			if (weaponStats.critDamage > _previousWeaponStats.critDamage)
				currenStat.statBackground.color = currenStat.positiveColor;
			else if (weaponStats.critDamage < _previousWeaponStats.critDamage)
				currenStat.statBackground.color = currenStat.negativeColor;

			currenStat = Instantiate(_statUI_Prefab, _weaponStatsParent);
			currenStat.Initialize("Range: ", weaponStats.range.ToString("0.00"));
			if (weaponStats.range > _previousWeaponStats.range)
				currenStat.statBackground.color = currenStat.positiveColor;
			else if (weaponStats.range < _previousWeaponStats.range)
				currenStat.statBackground.color = currenStat.negativeColor;

			currenStat = Instantiate(_statUI_Prefab, _weaponStatsParent);
			currenStat.Initialize("Capacity: ", weaponStats.capacity.ToString("0.00"));
			if (weaponStats.capacity > _previousWeaponStats.capacity)
				currenStat.statBackground.color = currenStat.positiveColor;
			else if (weaponStats.capacity < _previousWeaponStats.capacity)
				currenStat.statBackground.color = currenStat.negativeColor;

			currenStat = Instantiate(_statUI_Projectile_Trajectory_Prefab, _weaponStatsParent);
			currenStat.Initialize("Projectile Trajectory: ", weaponStats.attackPattern.PatternName());
			if (weaponStats.attackPattern != _previousWeaponStats.attackPattern)
				currenStat.statBackground.color = currenStat.highlightColor;
			else
				currenStat.statBackground.color = currenStat.standartBackgroundColor;

			currenStat = Instantiate(_statUI_Prefab, _weaponStatsParent);
			if (weaponStats.statusEffect != null)
            {
				currenStat.Initialize("Effect: ", weaponStats.statusEffect.StatusName());
				if (weaponStats.statusEffect != _previousWeaponStats.statusEffect)
				{
					currenStat.statBackground.color = currenStat.highlightColor;
				}
				else
				{
					currenStat.statBackground.color = currenStat.standartBackgroundColor;
				}
			}
			else
            {
				currenStat.Initialize("Effect: ", "None");
				if (weaponStats.statusEffect != _previousWeaponStats.statusEffect)
				{
					currenStat.statBackground.color = currenStat.highlightColor;
				}
				else
				{
					currenStat.statBackground.color = currenStat.standartBackgroundColor;
				}
			}
		}
		else
		{
			StatUI currenStat = Instantiate(_statUI_DPS_Prefab, _weaponStatsParent);
			currenStat.Initialize("DPS: ", weaponStats.dps.ToString("0.00"));
			currenStat = Instantiate(_statUI_Prefab, _weaponStatsParent);
			currenStat.Initialize("Damage: ", weaponStats.damage.ToString("0.00"));
			currenStat = Instantiate(_statUI_Prefab, _weaponStatsParent);
			currenStat.Initialize("Attack Speed: ", weaponStats.attackspeed.ToString("0.00"));
			currenStat = Instantiate(_statUI_Prefab, _weaponStatsParent);
			currenStat.Initialize("Reload Time: ", weaponStats.cooldown.ToString("0.00"));
			currenStat = Instantiate(_statUI_Prefab, _weaponStatsParent);
			currenStat.Initialize("Projectile Size: ", weaponStats.projectileSize.ToString("0.00"));
			currenStat = Instantiate(_statUI_Prefab, _weaponStatsParent);
			currenStat.Initialize("Crit Chance: ", weaponStats.critChance.ToString("0.00"));
			currenStat = Instantiate(_statUI_Prefab, _weaponStatsParent);
			currenStat.Initialize("Crit Damage: ", weaponStats.critDamage.ToString("0.00"));
			currenStat = Instantiate(_statUI_Prefab, _weaponStatsParent);
			currenStat.Initialize("Range: ", weaponStats.range.ToString("0.00"));
			currenStat = Instantiate(_statUI_Prefab, _weaponStatsParent);
			currenStat.Initialize("Capacity: ", weaponStats.capacity.ToString("0.00"));
			currenStat = Instantiate(_statUI_Projectile_Trajectory_Prefab, _weaponStatsParent);
			currenStat.Initialize("Projectile Trajectory: ", weaponStats.attackPattern.PatternName());
			currenStat = Instantiate(_statUI_Prefab, _weaponStatsParent);
			if (weaponStats.statusEffect != null)
				currenStat.Initialize("Effect: ", weaponStats.statusEffect.StatusName().ToString());
			else
				currenStat.Initialize("Effect: ", "None");
		}

		_previousWeaponStats = weaponStats;
	}

	public void ShowPotentialUpdatedWeaponStats(WeaponPart weaponPart)
	{
		ShowWeaponStats(weapon.CalculatePotentialStats(weaponPart), true);
	}
}