using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class HolsterTabs : MonoBehaviour
{
	public event Action<Weapon> OnSelectWeapon;

	public List<WeaponTab> WeaponTabs { get; private set; } = new List<WeaponTab>();

	[SerializeField]
	private Image _left;
	[SerializeField]
	private Image _right;

	[SerializeField]
	private RectTransform _tabContainer;
	[SerializeField]
	private WeaponTab _weaponTabPrefab;

	private int _currentSelectedTab;

	private void Awake()
	{
		InputManager.Instance.CharacterInputActions.UI.ShoulderButtons.started += NavigateTabs;
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
		foreach (Weapon weapon in GameManager.Instance.player.GetComponent<WeaponHolster>().weapons)
		{
			WeaponTab weaponTab = Instantiate(_weaponTabPrefab, _tabContainer);
			weaponTab.AssociatedWeapon = weapon;
			weaponTab.ButtonText.text = weapon.name;

			weaponTab.Addlistener(() => SelectWeapon(weapon));
			weaponTab.OnWeaponTabSelect += SelectWeapon;

			WeaponTabs.Add(weaponTab);
		}

		_currentSelectedTab = 0;
		WeaponTabs[_currentSelectedTab].Select();
	}

	private void NavigateTabs(InputAction.CallbackContext ctx)
	{
		int navInput = Mathf.CeilToInt(ctx.ReadValue<float>());

		_currentSelectedTab += navInput;
		_currentSelectedTab = Mathf.Clamp(_currentSelectedTab, 0, WeaponTabs.Count -1);
		WeaponTabs[_currentSelectedTab].Select();
	}

	private void SelectWeapon(Weapon weapon) => OnSelectWeapon.Invoke(weapon);
}
