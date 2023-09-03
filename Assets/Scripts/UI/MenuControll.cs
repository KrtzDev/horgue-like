using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class MenuControll : MonoBehaviour
{
	[SerializeField]
	private Image _left;
	[SerializeField]
	private Image _right;

	[SerializeField]
	private List<MenuTab> _menuTabs = new List<MenuTab>();

	private int _currentSelectedTab;



	private void OnEnable()
	{
		InputManager.Instance.CharacterInputActions.UI.Triggers.started += NavigateTabs;
		InputManager.Instance.CharacterInputActions.UI.ShoulderButtons.started += KeepTabSelected;
	}


	private void Start()
	{
		foreach (MenuTab tab in _menuTabs)
		{
			tab.Addlistener(() => SelectMenuMouse(tab.AssociatedMenu));
			tab.UnFocusMenu();
			tab.Deselect();
		}

		_currentSelectedTab = 1;
		_menuTabs[_currentSelectedTab].KeepFocus();
		_menuTabs[_currentSelectedTab].Select();
	}

	private void NavigateTabs(InputAction.CallbackContext ctx)
	{
		int navInput = (int)Mathf.Sign(ctx.ReadValue<float>());

		_menuTabs[_currentSelectedTab].ResetColors();
		_menuTabs[_currentSelectedTab].UnFocusMenu();

		_currentSelectedTab += navInput;
		_currentSelectedTab = Mathf.Clamp(_currentSelectedTab, 0, _menuTabs.Count -1);
		_menuTabs[_currentSelectedTab].KeepFocus();
		_menuTabs[_currentSelectedTab].Select();
	}

	private void KeepTabSelected(InputAction.CallbackContext ctx)
	{
		_menuTabs[_currentSelectedTab].KeepFocus();
	}

	private void SelectMenuMouse(UIMenu menu)
	{
		_menuTabs[_currentSelectedTab].ResetColors();
		_menuTabs[_currentSelectedTab].UnFocusMenu();

		_currentSelectedTab = _menuTabs.FindIndex(e => e.AssociatedMenu == menu);
		_menuTabs[_currentSelectedTab].KeepFocus();
		_menuTabs[_currentSelectedTab].Select();
	}
}
