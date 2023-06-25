using TMPro;
using UnityEngine;
using UnityEngine.ProBuilder.MeshOperations;
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

	public Weapon _weapon;

	public void Initialize(Weapon weapon)
	{
		_weapon = weapon;

		ShowWeaponName();
		ShowWeaponBackground();
		ShowWeaponParts();
		InitializeWeaponSlots();

		ShowWeaponStats(_weapon.CalculateWeaponStats(_weapon));
	}

	public bool SetNewWeaponPart(WeaponPart newWeaponPart, WeaponPartSlot weaponPartSlot)
	{
		if (newWeaponPart.GetType() == weaponPartSlot.GetCurrentSlottedWeaponPart().GetType() && !newWeaponPart.isSlotted)
		{
			if (newWeaponPart is Grip)
			{
				_weapon.grip = newWeaponPart as Grip;
			}
			else if (newWeaponPart is Barrel)
			{
				_weapon.barrel = newWeaponPart as Barrel;
			}
			else if (newWeaponPart is Magazine)
			{
				_weapon.magazine = newWeaponPart as Magazine;
			}
			else if (newWeaponPart is Ammunition)
			{
				_weapon.ammunition = newWeaponPart as Ammunition;
			}
			else if (newWeaponPart is TriggerMechanism)
			{
				_weapon.triggerMechanism = newWeaponPart as TriggerMechanism;
			}
			else if (newWeaponPart is Sight)
			{
				_weapon.sight = newWeaponPart as Sight;
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
		_weaponImage.sprite = _weapon.weaponSprite;
	}

	private void ShowWeaponParts()
	{
		RewardUI rewardUI;

		rewardUI = Instantiate(_rewardUI_prefab, _ammunitionParent.transform);
		Reward ammo = new Reward(_weapon.ammunition);
		rewardUI.Initialize(ammo);
		rewardUI = Instantiate(_rewardUI_prefab, _barrelParent.transform);
		Reward barrel = new Reward(_weapon.barrel);
		rewardUI.Initialize(barrel);
		rewardUI = Instantiate(_rewardUI_prefab, _gripParent.transform);
		Reward grip = new Reward(_weapon.grip);
		rewardUI.Initialize(grip);
		rewardUI = Instantiate(_rewardUI_prefab, _magazineParent.transform);
		Reward mag = new Reward(_weapon.magazine);
		rewardUI.Initialize(mag);
		rewardUI = Instantiate(_rewardUI_prefab, _sightParent.transform);
		Reward sight = new Reward(_weapon.sight);
		rewardUI.Initialize(sight);
		rewardUI = Instantiate(_rewardUI_prefab, _triggerParent.transform);
		Reward trigger = new Reward(_weapon.triggerMechanism);
		rewardUI.Initialize(trigger);
	}

	private void InitializeWeaponSlots()
	{
		_ammunitionParent.Initialize(this, _weapon.ammunition);
		_barrelParent.Initialize(this, _weapon.barrel);
		_gripParent.Initialize(this, _weapon.grip);
		_magazineParent.Initialize(this, _weapon.magazine);
		_sightParent.Initialize(this, _weapon.sight);
		_triggerParent.Initialize(this, _weapon.triggerMechanism);
	}

	private void ClearWeaponStats()
	{
		for (int i = 0; i < _weaponStatsParent.childCount; i++)
		{
			Destroy(_weaponStatsParent.GetChild(i).gameObject);
		}
	}



	WeaponStats _previousWeaponStats;

	public void ShowWeaponStats(WeaponStats weaponStats, bool colorStats = false)
	{
		ClearWeaponStats();

		if (!colorStats) 
		{ 
		StatUI currenStat = Instantiate(_statUI_prefab, _weaponStatsParent);
		currenStat.Initialize("Damage: ", weaponStats.damage.ToString());
		currenStat = Instantiate(_statUI_prefab, _weaponStatsParent);
		currenStat.Initialize("Attack Speed: ", weaponStats.attackspeed.ToString());
		currenStat = Instantiate(_statUI_prefab, _weaponStatsParent);
		currenStat.Initialize("Reload Time: ", weaponStats.cooldown.ToString());
		currenStat = Instantiate(_statUI_prefab, _weaponStatsParent);
		currenStat.Initialize("Projectile Size: ", weaponStats.projectileSize.ToString());
		currenStat = Instantiate(_statUI_prefab, _weaponStatsParent);
		currenStat.Initialize("Crit Chance: ", weaponStats.critChance.ToString());
		currenStat = Instantiate(_statUI_prefab, _weaponStatsParent);
		currenStat.Initialize("Range: ", weaponStats.range.ToString());
		currenStat = Instantiate(_statUI_prefab, _weaponStatsParent);
		currenStat.Initialize("Capacity: ", weaponStats.capacity.ToString());
		currenStat = Instantiate(_statUI_prefab, _weaponStatsParent);
		currenStat.Initialize("Projectile trajectory: ", weaponStats.attackPattern.PatternName().ToString());
		}
		else if (_previousWeaponStats != null && colorStats)
		{
			StatUI currenStat = Instantiate(_statUI_prefab, _weaponStatsParent);
			currenStat.Initialize("Damage: ", weaponStats.damage.ToString());
			if (weaponStats.damage > _previousWeaponStats.damage)
				currenStat.statBackground.color = currenStat.positiveColor;
			else if (weaponStats.damage < _previousWeaponStats.damage)
				currenStat.statBackground.color = currenStat.negativeColor;

			currenStat = Instantiate(_statUI_prefab, _weaponStatsParent);
			currenStat.Initialize("Attack Speed: ", weaponStats.attackspeed.ToString());
			if (weaponStats.attackspeed > _previousWeaponStats.attackspeed)
				currenStat.statBackground.color = currenStat.positiveColor;
			else if (weaponStats.attackspeed < _previousWeaponStats.attackspeed)
				currenStat.statBackground.color = currenStat.negativeColor;

			currenStat = Instantiate(_statUI_prefab, _weaponStatsParent);
			currenStat.Initialize("Reload Time: ", weaponStats.cooldown.ToString());
			if (weaponStats.cooldown > _previousWeaponStats.cooldown)
				currenStat.statBackground.color = currenStat.positiveColor;
			else if (weaponStats.cooldown < _previousWeaponStats.cooldown)
				currenStat.statBackground.color = currenStat.negativeColor;

			currenStat = Instantiate(_statUI_prefab, _weaponStatsParent);
			currenStat.Initialize("Projectile Size: ", weaponStats.projectileSize.ToString());
			if (weaponStats.projectileSize > _previousWeaponStats.projectileSize)
				currenStat.statBackground.color = currenStat.positiveColor;
			else if (weaponStats.projectileSize < _previousWeaponStats.projectileSize)
				currenStat.statBackground.color = currenStat.negativeColor;

			currenStat = Instantiate(_statUI_prefab, _weaponStatsParent);
			currenStat.Initialize("Crit Chance: ", weaponStats.critChance.ToString());
			if (weaponStats.critChance > _previousWeaponStats.critChance)
				currenStat.statBackground.color = currenStat.positiveColor;
			else if (weaponStats.critChance < _previousWeaponStats.critChance)
				currenStat.statBackground.color = currenStat.negativeColor;

			currenStat = Instantiate(_statUI_prefab, _weaponStatsParent);
			currenStat.Initialize("Range: ", weaponStats.range.ToString());
			if (weaponStats.range > _previousWeaponStats.range)
				currenStat.statBackground.color = currenStat.positiveColor;
			else if (weaponStats.range < _previousWeaponStats.range)
				currenStat.statBackground.color = currenStat.negativeColor;

			currenStat = Instantiate(_statUI_prefab, _weaponStatsParent);
			currenStat.Initialize("Capacity: ", weaponStats.capacity.ToString());
			if (weaponStats.capacity > _previousWeaponStats.capacity)
				currenStat.statBackground.color = currenStat.positiveColor;
			else if (weaponStats.capacity < _previousWeaponStats.capacity)
				currenStat.statBackground.color = currenStat.negativeColor;

			currenStat = Instantiate(_statUI_prefab, _weaponStatsParent);
			currenStat.Initialize("Projectile trajectory: ", weaponStats.attackPattern.PatternName().ToString());
		}
		else
		{
			StatUI currenStat = Instantiate(_statUI_prefab, _weaponStatsParent);
			currenStat.Initialize("Damage: ", weaponStats.damage.ToString());
			currenStat = Instantiate(_statUI_prefab, _weaponStatsParent);
			currenStat.Initialize("Attack Speed: ", weaponStats.attackspeed.ToString());
			currenStat = Instantiate(_statUI_prefab, _weaponStatsParent);
			currenStat.Initialize("Reload Time: ", weaponStats.cooldown.ToString());
			currenStat = Instantiate(_statUI_prefab, _weaponStatsParent);
			currenStat.Initialize("Projectile Size: ", weaponStats.projectileSize.ToString());
			currenStat = Instantiate(_statUI_prefab, _weaponStatsParent);
			currenStat.Initialize("Crit Chance: ", weaponStats.critChance.ToString());
			currenStat = Instantiate(_statUI_prefab, _weaponStatsParent);
			currenStat.Initialize("Range: ", weaponStats.range.ToString());
			currenStat = Instantiate(_statUI_prefab, _weaponStatsParent);
			currenStat.Initialize("Capacity: ", weaponStats.capacity.ToString());
			currenStat = Instantiate(_statUI_prefab, _weaponStatsParent);
			currenStat.Initialize("Projectile trajectory: ", weaponStats.attackPattern.PatternName().ToString());
		}

		_previousWeaponStats = weaponStats;
	}

	public void ShowPotentilaUpdatedWeaponStats(WeaponPart weaponPart)
	{
		ShowWeaponStats(_weapon.CalculatePotentialStats(weaponPart), true);
	}
}