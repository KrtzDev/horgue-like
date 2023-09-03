﻿using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ToolTipUI : Selectable
{
	public event Action<ToolTipUI> OnBuy;
	public event Action OnBuyFailed;
	public event Action<ToolTipUI> OnSelected;
	public event Action OnDeselected;

	[SerializeField]
	public HoldButton buyButton;
	[SerializeField]
	public HoldButton newShopButton;

	[SerializeField]
	public Image _selectionIndicator;

	[SerializeField]
	private Image _partImage;
	[SerializeField]
	private LayoutGroup _layoutGroup;
	[SerializeField]
	private RectTransform _statParent;
	[SerializeField]
	private RectTransform _contentRect;
	[SerializeField]
	private StatUI _statUI_prefab;
	[SerializeField]
	private StatUI _statUI_Projectile_Trajectory_prefab;
	[SerializeField]
	private RarityBadgeUI _rarityBadgeUI;

	public WeaponPart weaponPart;
	private int _cost;

	private bool _selected;

	public StatUI damageStatUI;
	public StatUI attackSpeedStatUI;
	public StatUI cooldownStatUI;
	public StatUI projectileSizeStatUI;
	public StatUI critChanceStatUI;
	public StatUI critDamageStatUI;
	public StatUI rangeStatUI;

	public StatUI attackPatternStatUI;
	public StatUI capacityStatUI;
	public StatUI statusEffectStatUI;

	public void Initialize(WeaponPart weaponPartData)
	{
		_partImage.sprite = weaponPartData.WeaponPartUISprite;
		weaponPart = weaponPartData;
		_cost = (int)weaponPartData.cost;
		_rarityBadgeUI.SetColor(RewardManager.Instance.GetColorFromRarity(weaponPart.rarity));

		InitializeStats(weaponPartData);
		LayoutRebuilder.ForceRebuildLayoutImmediate(_layoutGroup.transform as RectTransform);
		StartCoroutine(UpdateHeightAfterFrame());
		
		if(buyButton)
			buyButton.OnButtonExecute += Buy;
	}

	private IEnumerator UpdateHeightAfterFrame()
	{
		yield return new WaitForEndOfFrame();
		GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical,_contentRect.rect.height);
	}

	private void InitializeStats(WeaponPart weaponPartData)
	{
		damageStatUI = Instantiate(_statUI_prefab, _statParent);
		damageStatUI.Initialize("Damage: ", weaponPartData.baseDamage.ToString("0.00"));
		attackSpeedStatUI = Instantiate(_statUI_prefab, _statParent);
		attackSpeedStatUI.Initialize("Attack Speed: ", weaponPartData.attackSpeed.ToString("0.00"));
		cooldownStatUI = Instantiate(_statUI_prefab, _statParent);
		cooldownStatUI.Initialize("Reload Time: ", weaponPartData.cooldown.ToString("0.00"));
		projectileSizeStatUI = Instantiate(_statUI_prefab, _statParent);
		projectileSizeStatUI.Initialize("Projectile Size: ", weaponPartData.projectileSize.ToString("0.00"));
		critChanceStatUI = Instantiate(_statUI_prefab, _statParent);
		critChanceStatUI.Initialize("Crit Chance: ", weaponPartData.critChance.ToString("0.00"));
		critDamageStatUI = Instantiate(_statUI_prefab, _statParent);
		critDamageStatUI.Initialize("Crit Damage: ", weaponPartData.critChance.ToString("0.00"));
		rangeStatUI = Instantiate(_statUI_prefab, _statParent);
		rangeStatUI.Initialize("Range: ", weaponPartData.range.ToString("0.00"));

		if (weaponPartData is Barrel)
		{
			Barrel barrel = weaponPartData as Barrel;

			attackPatternStatUI = Instantiate(_statUI_Projectile_Trajectory_prefab, _statParent);
			attackPatternStatUI.Initialize("Projectile Trajectory: ", barrel.attackPattern.PatternName());
		}
		else if (weaponPartData is Magazine)
		{
			Magazine mag = weaponPartData as Magazine;

			capacityStatUI = Instantiate(_statUI_prefab, _statParent);
			capacityStatUI.Initialize("Capacity: ", mag.capacity.ToString());
		}
		else if (weaponPartData is Ammunition)
		{
			Ammunition ammunition = weaponPartData as Ammunition;

			if (ammunition.statusEffect != null)
			{
				statusEffectStatUI = Instantiate(_statUI_prefab, _statParent);
				statusEffectStatUI.Initialize("Effect", ammunition.statusEffect.StatusName());
			}
		}
	}

	private void Buy()
	{
		if (!_selected)
			return;

		if (!GameManager.Instance.inventory.Wallet.TryPay(_cost))
		{
			OnBuyFailed.Invoke();
			return;
		}

		OnBuy.Invoke(this);
	}

	public void CleanUp()
	{
		buyButton.OnButtonExecute -= Buy;
		Destroy(gameObject);
	}

	public void SetSelected(bool selected)
	{
		_selected = selected;
		_selectionIndicator.enabled = selected;
	}

	public override void OnSelect(BaseEventData eventData)
	{
		base.OnSelect(eventData);
		OnSelected.Invoke(this);
	}

	public override void OnDeselect(BaseEventData eventData)
	{
		base.OnDeselect(eventData);
		OnDeselected.Invoke();
	}
}