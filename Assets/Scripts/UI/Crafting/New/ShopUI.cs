using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopUI : UIMenu
{
	public event Action<WeaponPart> OnBought;

	[SerializeField]
	private Transform _shopItemContainer;
	[SerializeField]
	private HoldButton _buyButton;
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

		if(_shopItems.Count > 0)
			StartCoroutine(SelectFirstItemAfterFrame());
	}

	private IEnumerator SelectFirstItemAfterFrame()
	{
		yield return null;
		_shopItems.First().Select();
	}

	private void SetUpNavigation()
	{
		for (int i = 0; i < _shopItems.Count; i++)
		{
			Navigation navigation = new Navigation();
			navigation.mode = Navigation.Mode.Explicit;
			if (i - 1 >= 0)
				navigation.selectOnLeft = _shopItems[i - 1];
			if (i - 3 >= 0)
				navigation.selectOnUp = _shopItems[i - 3];
			if (i + 1 < _shopItems.Count)
				navigation.selectOnRight = _shopItems[i + 1];
			if (i + 3 < _shopItems.Count)
				navigation.selectOnDown = _shopItems[i + 3];

			_shopItems[i].navigation = navigation;
		}
	}

	public override void SetFocusedMenu()
	{
		_buyButton.Enable();
		_equipButton.Disable();
		StartCoroutine(FocusafterFrame());
	}

	private IEnumerator FocusafterFrame()
	{
		yield return new WaitForEndOfFrame();
		_shopItems[0].Select();
	}
}
