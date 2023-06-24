using System.Collections.Generic;
using UnityEngine;

public class WeaponHolster : MonoBehaviour
{
	[SerializeField]
	public List<Weapon> weapons = new List<Weapon>();

	private List<WeaponSpawnSlot> _weaponSpawnSlots = new List<WeaponSpawnSlot>();

	private void Start()
	{
		PlayerCharacter playerCharacter = GetComponent<PlayerCharacter>();
		for (int i = 0; i < playerCharacter.WeaponSpawnTransform.childCount; ++i)
		{
			_weaponSpawnSlots.Add(new WeaponSpawnSlot(playerCharacter.WeaponSpawnTransform.GetChild(i), false));
		}
		foreach (Weapon weapon in weapons)
		{
			WeaponSpawnSlot freeWeaponSpawnSlot;
			if (GetFreeWeaponSlot(out freeWeaponSpawnSlot))
			{
				weapon.Initialize(freeWeaponSpawnSlot.spawnTransform);
				freeWeaponSpawnSlot.isOccupied = true;
			}
			else
			{
				Debug.LogError("No Free WeaponSpawnSlot found");
			}
		}
	}

	private void Update()
	{
		TryShootAllWeapons();
	}

	private bool GetFreeWeaponSlot(out WeaponSpawnSlot freeweaponSpawnSlot)
	{
		foreach (var slot in _weaponSpawnSlots)
		{
			if (!slot.isOccupied)
			{
				freeweaponSpawnSlot = slot;
				return true;
			}
		}
		freeweaponSpawnSlot = null;
		return false;
	}

	[ContextMenu("TryShootAllWeapons")]
	private void TryShootAllWeapons()
	{
		foreach (Weapon weapon in weapons)
		{
			weapon.TryShoot();
		}
	}
}
