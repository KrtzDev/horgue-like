using UnityEngine;

public class Inventory : MonoBehaviour
{
	[SerializeField]
	private WeaponPart[] _weaponParts;

	public void AddToInventory()
	{

	}

	public void GetFromInventory(int index)
	{

	}

	public WeaponPart[] GetAll()
	{
		return _weaponParts;
	}
}
