using UnityEngine;

public class WeaponHolsterUI : MonoBehaviour
{
	[SerializeField]
	private HolsterTabs _holsterTabs;

	[SerializeField]
	private WeaponUI _weaponUI;

	private void OnEnable()
	{
		_holsterTabs.OnSelectWeapon += ShowWeaponUI;
	}

	private void OnDisable()
	{
		_holsterTabs.OnSelectWeapon -= ShowWeaponUI;
	}

	private void ShowWeaponUI(Weapon weapon)
	{
		_weaponUI.Initialize(weapon);
	}
}
