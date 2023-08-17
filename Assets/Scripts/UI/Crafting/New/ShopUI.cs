using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopUI : UIMenu
{
	public event Action<WeaponPart> OnBought;

	[SerializeField]
	private WeaponUI _weaponUI;
	[SerializeField]
	private ComparisonUI _comparisonUI;

	[SerializeField]
	private Transform _shopItemContainer;
	[SerializeField]
	private Transform _comparisonContainerShopItem;
	[SerializeField]
	private Transform _comparisonContainerEquipped;
	[SerializeField]
	private Transform _comparisonContainerSelected;
	[SerializeField]
	private HoldButton _buyButton;
	[SerializeField]
	private HoldButton _sellButton;
	[SerializeField]
	private HoldButton _equipButton;

	[SerializeField]
	private ToolTipUI _toolTipUI_prefab;

	public List<ToolTipUI> shopItems;

	private void Start()
	{
		shopItems = new List<ToolTipUI>();
		for (int i = 0; i < 3; i++)
		{
			ToolTipUI tooltipUI = Instantiate(_toolTipUI_prefab,_shopItemContainer);
			WeaponPart weaponPart = RewardManager.Instance.GetRandomReward();
			RewardManager.Instance.ClearRewards();

			tooltipUI.OnBuy += OnItemBought;
			tooltipUI.OnSelected += UpdateEquippedComparisonUI;
			tooltipUI.OnDeselected += UpdateWeaponUI;
			tooltipUI.buyButton = _buyButton;

			tooltipUI.Initialize(weaponPart);
			shopItems.Add(tooltipUI);
		}

		SetUpNavigation();
	}

	private void OnItemBought(ToolTipUI TooltipUi)
	{
		 OnBought.Invoke(TooltipUi.weaponPart);
		shopItems.Remove(TooltipUi);

		Debug.Log("Ping");

		SetUpNavigation();

		if (shopItems.Count > 0)
			StartCoroutine(SelectFirstItemAfterFrame());
	}

	private IEnumerator SelectFirstItemAfterFrame()
	{
		yield return new WaitForEndOfFrame();
		shopItems[0].Select();
	}

	private void SetUpNavigation()
	{
		for (int i = 0; i < shopItems.Count; i++)
		{
			Navigation navigation = new Navigation();
			navigation.mode = Navigation.Mode.Explicit;
			if (i - 1 >= 0)
				navigation.selectOnLeft = shopItems[i - 1];
			if (i - shopItems.Count >= 0)
				navigation.selectOnUp = shopItems[i - shopItems.Count];
			if (i + 1 < shopItems.Count)
				navigation.selectOnRight = shopItems[i + 1];
			if (i + shopItems.Count < shopItems.Count)
				navigation.selectOnDown = shopItems[i + shopItems.Count];

			shopItems[i].navigation = navigation;
		}
	}

	public override void SetFocusedMenu()
	{
		_buyButton.Enable();
		_equipButton.Disable();
		_sellButton.Disable();
		StartCoroutine(SelectFirstItemAfterFrame());
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

		ToolTipUI selectedTooltip = Instantiate(_toolTipUI_prefab, _comparisonContainerSelected);
		selectedTooltip.Initialize(weaponPart);

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

		StartCoroutine(CompareAfterFrame());
	}

	private IEnumerator CompareAfterFrame()
	{
		yield return new WaitForEndOfFrame();
		_comparisonUI.Compare();
	}

	private void ClearComparisonContainer()
	{
		for (int i = 0; i < _comparisonContainerShopItem.childCount; i++)
			Destroy(_comparisonContainerShopItem.GetChild(i).gameObject);

		for (int i = 0; i < _comparisonContainerEquipped.childCount; i++)
			Destroy(_comparisonContainerEquipped.GetChild(i).gameObject);

		for (int i = 0; i < _comparisonContainerSelected.childCount; i++)
			Destroy(_comparisonContainerSelected.GetChild(i).gameObject);
	}
}
