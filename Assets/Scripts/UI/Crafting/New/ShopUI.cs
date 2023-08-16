using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ShopUI : UIMenu
{
	public event Action<WeaponPart> OnBought;

	[SerializeField]
	private WeaponUI _weaponUI;

	[SerializeField]
	private Transform _shopItemContainer;
	[SerializeField]
	private Transform _comparisonContainerShopItem;
	[SerializeField]
	private Transform _comparisonContainerEquipped;
	[SerializeField]
	private HoldButton _buyButton;
	[SerializeField]
	private HoldButton _sellButton;
	[SerializeField]
	private HoldButton _equipButton;

	[SerializeField]
	private ToolTipUI _toolTipUI_prefab;

	private List<ToolTipUI> _shopItems;

	private void Awake()
	{
		_shopItems = new List<ToolTipUI>();
		for (int i = 0; i < 3; i++)
		{
			ToolTipUI tooltipUI = Instantiate(_toolTipUI_prefab,_shopItemContainer);

			_shopItems.Add(tooltipUI);
			tooltipUI.OnBuy += OnItemBought;
			tooltipUI.OnSelected += UpdateEquippedComparisonUI;
			tooltipUI.OnDeselected += UpdateWeaponUI;

			tooltipUI = _shopItemContainer.GetChild(i).GetComponent<ToolTipUI>();
			tooltipUI.buyButton = _buyButton;
			tooltipUI.Initialize(RewardManager.Instance.GetRandomReward());
			RewardManager.Instance.ClearRewards();
		}

		SetUpNavigation();
	}

	private void OnItemBought(ToolTipUI TooltipUi)
	{
		 OnBought.Invoke(TooltipUi.weaponPart);
		_shopItems.Remove(TooltipUi);

		SetUpNavigation();

		if (_shopItems.Count > 0)
			StartCoroutine(SelectFirstItemAfterFrame());
	}

	private IEnumerator SelectFirstItemAfterFrame()
	{
		yield return new WaitForEndOfFrame();
		_shopItems[0].Select();
	}

	private void SetUpNavigation()
	{
		for (int i = 0; i < _shopItems.Count; i++)
		{
			Navigation navigation = new Navigation();
			navigation.mode = Navigation.Mode.Explicit;
			if (i - 1 >= 0)
				navigation.selectOnLeft = _shopItems[i - 1];
			if (i - _shopItems.Count >= 0)
				navigation.selectOnUp = _shopItems[i - _shopItems.Count];
			if (i + 1 < _shopItems.Count)
				navigation.selectOnRight = _shopItems[i + 1];
			if (i + _shopItems.Count < _shopItems.Count)
				navigation.selectOnDown = _shopItems[i + _shopItems.Count];

			_shopItems[i].navigation = navigation;
		}
	}

	public override void SetFocusedMenu()
	{
		_buyButton.Enable();
		_equipButton.Disable();
		_sellButton.Disable();
		StartCoroutine(FocusafterFrame());
	}

	private IEnumerator FocusafterFrame()
	{
		yield return new WaitForEndOfFrame();
		_shopItems[0].Select();
	}

	private void UpdateWeaponUI()
	{
		_weaponUI.ShowWeaponStats(_weaponUI.weapon.CalculateWeaponStats(_weaponUI.weapon));
	}

	private void UpdateEquippedComparisonUI(WeaponPart weaponPart)
	{
		UpdateWeaponUI();
		_weaponUI.ShowPotentialUpdatedWeaponStats(weaponPart);

		ClearComparisonContainer();

		if(weaponPart.GetType() == typeof(Grip))
		{
			ToolTipUI toolTipUI = Instantiate(_toolTipUI_prefab, _comparisonContainerEquipped);
			toolTipUI.Initialize(_weaponUI.weapon.grip);
		}
		else if(weaponPart.GetType() == typeof(Barrel))
		{
			ToolTipUI toolTipUI = Instantiate(_toolTipUI_prefab, _comparisonContainerEquipped);
			toolTipUI.Initialize(_weaponUI.weapon.barrel);
		}
		else if(weaponPart.GetType() == typeof(Magazine))
		{
			ToolTipUI toolTipUI = Instantiate(_toolTipUI_prefab, _comparisonContainerEquipped);
			toolTipUI.Initialize(_weaponUI.weapon.magazine);
		}
		else if(weaponPart.GetType() == typeof(Ammunition))
		{
			ToolTipUI toolTipUI = Instantiate(_toolTipUI_prefab, _comparisonContainerEquipped);
			toolTipUI.Initialize(_weaponUI.weapon.ammunition);
		}
		else if(weaponPart.GetType() == typeof(TriggerMechanism))
		{
			ToolTipUI toolTipUI = Instantiate(_toolTipUI_prefab, _comparisonContainerEquipped);
			toolTipUI.Initialize(_weaponUI.weapon.triggerMechanism);
		}
		else if(weaponPart.GetType() == typeof(Sight))
		{
			ToolTipUI toolTipUI = Instantiate(_toolTipUI_prefab, _comparisonContainerEquipped);
			toolTipUI.Initialize(_weaponUI.weapon.sight);
		}
	}

	public void UpdateComparisonUI(WeaponPartUI weaponPartUI)
	{
		ClearComparisonContainer();

		foreach (ToolTipUI item in _shopItems)
		{
			if (weaponPartUI != null && item.weaponPart.GetType() == weaponPartUI.weaponPart.GetType())
			{
				ToolTipUI toolTipUI = Instantiate(_toolTipUI_prefab, _comparisonContainerShopItem);
				toolTipUI.Initialize(item.weaponPart);
			}
		}
	}

	private void ClearComparisonContainer()
	{
		for (int i = 0; i < _comparisonContainerShopItem.childCount; i++)
			Destroy(_comparisonContainerShopItem.GetChild(i).gameObject);

		for (int i = 0; i < _comparisonContainerEquipped.childCount; i++)
			Destroy(_comparisonContainerEquipped.GetChild(i).gameObject);
	}
}
