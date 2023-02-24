using System;
using UnityEngine;

public class WeaponPartSlot : MonoBehaviour
{
	public WeaponUI _owningWeaponUI;
	private WeaponPart _slottedWeaponPart;

	public WeaponPart GetCurrentSlottedWeaponPart()
	{
		return _slottedWeaponPart;
	}

	public void Initialize(WeaponUI owningWeaponUI, WeaponPart weaponPart) 
	{
		_owningWeaponUI = owningWeaponUI;
		_slottedWeaponPart = weaponPart;
		weaponPart.isSlotted = true;
	}

	public void SetNewWeaponPart(WeaponPart weaponPart)
	{
		_slottedWeaponPart = weaponPart;
	}
}