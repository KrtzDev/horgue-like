using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class HolsterTabs : MonoBehaviour
{
	public event Action<Weapon> OnSelectWeapon;

	private List<WeaponTab> _weaponTabs = new List<WeaponTab>();

	[SerializeField]
	private Image _left;
	[SerializeField]
	private Image _right;

	[SerializeField]
	private RectTransform _tabContainer;
	[SerializeField]
	private WeaponTab _weaponTabPrefab;

	private int _currentSelectedTab;

	private void OnEnable()
	{
		InputManager.Instance.CharacterInputActions.UI.ShoulderButtons.started += NavigateTabs;
		InputManager.Instance.CharacterInputActions.UI.Triggers.started += KeepTabSelected;
	}

	private void Start()
	{
		DestroyOldTabs();
		CreateWeaponTabs();
	}

	private void DestroyOldTabs()
	{
		for (int i = 0; i < _tabContainer.childCount; i++)
		{
			Destroy(_tabContainer.GetChild(i).gameObject);
		}
	}

	private void CreateWeaponTabs()
	{
		foreach (Weapon weapon in GameManager.Instance._player.GetComponent<WeaponHolster>().weapons)
		{
			WeaponTab weaponTab = Instantiate(_weaponTabPrefab, _tabContainer);
			weaponTab.AssociatedWeapon = weapon;
			weaponTab.ButtonText.text = weapon.name;

			weaponTab.Addlistener(() => SelectWeaponMouse(weapon));
			weaponTab.OnWeaponTabSelect += SelectWeapon;

			_weaponTabs.Add(weaponTab);
		}

		_currentSelectedTab = 0;
		_weaponTabs[_currentSelectedTab].Select();
		_weaponTabs[_currentSelectedTab].KeepFocus();
	}

	private void NavigateTabs(InputAction.CallbackContext ctx)
	{
		int navInput = Mathf.CeilToInt(ctx.ReadValue<float>());

		_weaponTabs[_currentSelectedTab].ResetColors();

		_currentSelectedTab += navInput;
		_currentSelectedTab = Mathf.Clamp(_currentSelectedTab, 0, _weaponTabs.Count -1);
		_weaponTabs[_currentSelectedTab].Select();
		_weaponTabs[_currentSelectedTab].KeepFocus();
	}

	private void KeepTabSelected(InputAction.CallbackContext obj)
	{
		_weaponTabs[_currentSelectedTab].KeepFocus();
	}

	private void SelectWeapon(Weapon weapon) => OnSelectWeapon.Invoke(weapon);

	private void SelectWeaponMouse(Weapon weapon)
	{
		_weaponTabs[_currentSelectedTab].ResetColors();
		_currentSelectedTab = _weaponTabs.FindIndex(e => e.AssociatedWeapon == weapon);
		_weaponTabs[_currentSelectedTab].KeepFocus();

		OnSelectWeapon.Invoke(weapon);
	}
}
