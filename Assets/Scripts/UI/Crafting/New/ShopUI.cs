using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ShopUI : UIMenu
{
	[SerializeField]
	private Transform _shopItemContainer;

	[SerializeField]
	private ToolTipUI _toolTipUI_prefab;

	private ToolTipUI[] _shopItems;

	private void Awake()
	{
		_shopItems = new ToolTipUI[_shopItemContainer.childCount];
		for (int i = 0; i < _shopItems.Length; i++)
		{
			_shopItems[i] = _shopItemContainer.GetChild(i).GetComponent<ToolTipUI>();
		}

		InputManager.Instance.CharacterInputActions.UI.Submit.started += OnSubmit;

		SetUpNavigation();
	}


	private void OnSubmit(InputAction.CallbackContext obj)
	{
		if (EventSystem.current.currentSelectedGameObject.TryGetComponent<ToolTipUI>(out ToolTipUI shopItem))
		{
			shopItem.OnSubmit.Invoke();
		}
	}

	private void SetUpNavigation()
	{
		for (int i = 0; i < _shopItems.Length; i++)
		{
			Navigation navigation = new Navigation();
			navigation.mode = Navigation.Mode.Explicit;
			if (i - 1 >= 0)
				navigation.selectOnLeft = _shopItems[i - 1];
			if (i - 3 >= 0)
				navigation.selectOnUp = _shopItems[i - 3];
			if (i + 1 < _shopItems.Length)
				navigation.selectOnRight = _shopItems[i + 1];
			if (i + 3 < _shopItems.Length)
				navigation.selectOnDown = _shopItems[i + 3];

			_shopItems[i].navigation = navigation;
		}
	}

	public override void SetFocusedMenu()
	{
		StartCoroutine(FocusafterFrame());
	}

	private IEnumerator FocusafterFrame()
	{
		yield return new WaitForEndOfFrame();
		_shopItems[0].Select();
	}
}
