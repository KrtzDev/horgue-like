using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
	[field: SerializeField]
	public Wallet Wallet {  get; private set; }

	[SerializeField]
	private List<WeaponPart> _weaponParts;

	public void AddToInventory(WeaponPart weaponPartToAdd)
	{
		_weaponParts.Add(weaponPartToAdd);
	}

	public WeaponPart GetFromInventory(int index)
	{
		WeaponPart weaponPart = _weaponParts[index];
		_weaponParts.RemoveAt(index);
		return weaponPart;
	}

	public List<WeaponPart> GetAll()
	{
		return _weaponParts;
	}
}
