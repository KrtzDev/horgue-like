using DG.Tweening;
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
	private WalletUI _walletUI;

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
			ToolTipUI tooltipUI = Instantiate(_toolTipUI_prefab, _shopItemContainer);
			WeaponPart weaponPart = RewardManager.Instance.GetRandomReward();
			RewardManager.Instance.ClearRewards();

			tooltipUI.OnBuy += OnItemBought;
			tooltipUI.OnBuyFailed += OnItemBuyFalied;
			tooltipUI.OnSelected += OnToolTipSelected;
			tooltipUI.OnDeselected += OnToolTipDeselected;
			tooltipUI.buyButton = _buyButton;

			tooltipUI.Initialize(weaponPart);
			shopItems.Add(tooltipUI);
		}

		SetUpNavigation();
	}

	private void OnItemBought(ToolTipUI tooltipUi)
	{
		AudioManager.Instance.PlaySound("ShopConfirmation");

		OnBought.Invoke(tooltipUi.weaponPart);

		shopItems.Remove(tooltipUi);
		tooltipUi.CleanUp();

		SetUpNavigation();

		if (shopItems.Count > 0)
			StartCoroutine(SelectFirstItemAfterFrame());
	}

	private IEnumerator SelectFirstItemAfterFrame()
	{
		yield return new WaitForEndOfFrame();
		shopItems[0].Select();
	}

	private void OnItemBuyFalied()
	{
		AudioManager.Instance.PlaySound("ShopDecline");
		DoBlinkEffect(Color.red, Color.white, .1f, 2);
	}

	private void DoBlinkEffect(Color blinkColor, Color normalColor, float duration, int amount)
	{
		Tween colorToRed = _walletUI.BgImage.DOColor(blinkColor, duration).SetEase(Ease.OutBounce);
		Tween colorToNormal = _walletUI.BgImage.DOColor(normalColor, duration).SetEase(Ease.OutBounce);

		Sequence sequence = DOTween.Sequence();
		sequence.Append(colorToRed)
			.Append(colorToNormal);

		sequence.SetLoops(amount);
		sequence.Play();
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


	private void OnToolTipDeselected()
	{
		UpdateWeaponUI();
	}

	private void UpdateWeaponUI()
	{
		_weaponUI.ShowWeaponStats(_weaponUI.weapon.CalculateWeaponStats(_weaponUI.weapon));
	}

	private void OnToolTipSelected(ToolTipUI toolTipUI)
	{
		UpdateEquippedComparisonUI(toolTipUI.weaponPart);
		SetSelectedToolTip(toolTipUI);
	}

	private void SetSelectedToolTip(ToolTipUI toolTipUI)
	{
		for (int i = 0; i < shopItems.Count; i++)
		{
			shopItems[i].SetSelected(false);
		}
		toolTipUI.SetSelected(true);
	}

	private void UpdateEquippedComparisonUI(WeaponPart weaponPart)
	{
		UpdateWeaponUI();
		_weaponUI.ShowPotentialUpdatedWeaponStats(weaponPart);

		_comparisonUI.ClearComparisonContainer();

		ToolTipUI selectedTooltip = Instantiate(_toolTipUI_prefab, _comparisonContainerSelected);
		selectedTooltip.Initialize(weaponPart);

		_comparisonUI.UpdateEquippedComparisonUI(_weaponUI, weaponPart);
	}
}
