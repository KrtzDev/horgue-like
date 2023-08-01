using System;
using UnityEngine;
using UnityEngine.UI;

public class HolsterTabs : MonoBehaviour
{
	public event Action<Weapon> OnSelectWeapon;

	[SerializeField]
	private Image _left;
	[SerializeField]
	private Image _right;

	[SerializeField]
	private RectTransform _tabContainer;
	[SerializeField]
	private UIButton _weaponTab;

	private void Start()
	{
		RemovePlaceholderTabs();
		CreateWeaponTabs();
	}

	private void RemovePlaceholderTabs()
	{
		for (int i = 0; i < _tabContainer.childCount; i++)
		{
			Destroy(_tabContainer.GetChild(i).gameObject);
		}
	}

	private void CreateWeaponTabs()
	{
		bool selected = false;

		foreach (Weapon weapon in GameManager.Instance.player.GetComponent<WeaponHolster>().weapons)
		{
			UIButton uiButton = Instantiate(_weaponTab, _tabContainer);
			uiButton.ButtonText.text = weapon.name;
			uiButton.OnButtonSelect += () => SelectWeapon(weapon);

			if (!selected)
			{
				uiButton.Select();
				selected = true;
			}
		}
	}

	private void SelectWeapon(Weapon weapon) => OnSelectWeapon.Invoke(weapon);


}
