using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ToolTipUI : Selectable
{
	public Action OnSubmit;

	[SerializeField]
	private Image _selectionIndicator;

	[SerializeField]
	private Image _partImage;
	[SerializeField]
	private RectTransform _statParent;
	[SerializeField]
	private StatUI _statUI_prefab;

	protected override void Awake()
	{
		base.Awake();
		OnSubmit += StartBuy;
	}

	public void Initialize(WeaponPart weaponPartData)
	{
		_partImage.sprite = weaponPartData.WeaponPartUISprite;

		InitializeStats(weaponPartData);
	}

	private void InitializeStats(WeaponPart weaponPartData)
	{
		StatUI currentStat = Instantiate(_statUI_prefab, _statParent);
		currentStat.Initialize("Damage: ", weaponPartData.baseDamage.ToString());
		currentStat = Instantiate(_statUI_prefab, _statParent);
		currentStat.Initialize("Attack Speed: ", weaponPartData.attackSpeed.ToString());
		currentStat = Instantiate(_statUI_prefab, _statParent);
		currentStat.Initialize("Reload Time: ", weaponPartData.cooldown.ToString());
		currentStat = Instantiate(_statUI_prefab, _statParent);
		currentStat.Initialize("Projectile Size: ", weaponPartData.projectileSize.ToString());
		currentStat = Instantiate(_statUI_prefab, _statParent);
		currentStat.Initialize("Crit Chance: ", weaponPartData.critChance.ToString());
		currentStat = Instantiate(_statUI_prefab, _statParent);
		currentStat.Initialize("Range: ", weaponPartData.range.ToString());

		if (weaponPartData is Barrel)
		{
			Barrel barrel = weaponPartData as Barrel;

			currentStat = Instantiate(_statUI_prefab, _statParent);
			currentStat.Initialize("Projectile trajectory: ", barrel.attackPattern.PatternName());
		}
		else if (weaponPartData is Magazine)
		{
			Magazine mag = weaponPartData as Magazine;

			currentStat = Instantiate(_statUI_prefab, _statParent);
			currentStat.Initialize("Capacity: ", mag.capacity.ToString());
		}
		else if (weaponPartData is Ammunition)
		{
			Ammunition ammunition = weaponPartData as Ammunition;

			if (ammunition.statusEffect != null)
			{
				currentStat = Instantiate(_statUI_prefab, _statParent);
				currentStat.Initialize("Impact Effect", ammunition.statusEffect.StatusName());
			}
		}
	}

	private void StartBuy()
	{
		Debug.Log("Buy");
	}

	public override void OnSelect(BaseEventData eventData)
	{
		base.OnSelect(eventData);
		_selectionIndicator.enabled = true;
	}

	public override void OnDeselect(BaseEventData eventData)
	{
		base.OnDeselect(eventData);
		_selectionIndicator.enabled = false;
	}
}