using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : Selectable
{
	[SerializeField, HideInInspector]
	private WeaponPartUI _rewardUI;

	[HideInInspector]
	public WeaponPart weaponPart;
	[HideInInspector]
	public int index;

	public void SetWeaponPart(WeaponPart weaponPart)
	{
		this.weaponPart = weaponPart;
	}
}