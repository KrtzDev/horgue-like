using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
	[field: SerializeField]
	public Wallet Wallet { get; private set; }

	[SerializeField]
	private List<WeaponPart> _weaponParts;

	public void AddToInventory(WeaponPart weaponPartToAdd)
	{
		_weaponParts.Add(weaponPartToAdd);
	}

	public void RemoveFromInventory(int index)
	{
		_weaponParts.RemoveAt(index);
	}

	public void RemoveFromInventory(WeaponPart weaponPart)
	{
		_weaponParts.Remove(weaponPart);
	}
	public void ResetInventory()
    {
		_weaponParts.Clear();
    }

	public WeaponPart GetFromInventory(int index)
	{
		return _weaponParts[index];
	}

	public List<WeaponPart> GetAll()
	{
		return _weaponParts;
	}

}
