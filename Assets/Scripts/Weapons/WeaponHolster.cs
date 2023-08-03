using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponHolster : MonoBehaviour
{
	[SerializeField]
	public List<Weapon> weapons = new List<Weapon>();

	private List<WeaponSpawnSlot> _weaponSpawnSlots = new List<WeaponSpawnSlot>();

	private void Awake()
	{
		if (GameManager.Instance == null)
			return;

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

		InputManager.Instance.CharacterInputActions.Character.SwitchMode.performed += SwitchWeaponControllMode;

		InputManager.Instance.CharacterInputActions.Character.Shoot.performed += ShootWeapons;
		InputManager.Instance.CharacterInputActions.Character.Shoot.canceled += ShootWeapons;
	}

	private void SwitchWeaponControllMode(InputAction.CallbackContext ctx)
	{
		GameManager.Instance.weaponControll = (WeaponControllKind)(((int)GameManager.Instance.weaponControll + 1) % 3);
		Debug.Log(GameManager.Instance.weaponControll);
	}

	private void OnDisable()
	{
		if (GameManager.Instance == null)
			return;

		InputManager.Instance.CharacterInputActions.Character.SwitchMode.performed -= SwitchWeaponControllMode;

		InputManager.Instance.CharacterInputActions.Character.Shoot.performed -= ShootWeapons;
		InputManager.Instance.CharacterInputActions.Character.Shoot.canceled -= ShootWeapons;
	}

	private void ShootWeapons(InputAction.CallbackContext ctx)
	{
		_shouldShoot = ctx.ReadValue<float>() > 0;
	}

	private bool _shouldShoot = false;

	private void Update()
	{
		if (GameManager.Instance == null)
			return;

		if (GameManager.Instance.weaponControll == WeaponControllKind.AllManual || GameManager.Instance.weaponControll == WeaponControllKind.AutoShootManualAim)
		{
			foreach (Weapon weapon in weapons)
				weapon.UpdateAimDirection();
		}
		if (GameManager.Instance.weaponControll == WeaponControllKind.AllManual)
		{
			if(_shouldShoot)
				TryShootAllWeapons();
			return;
		}

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
			weapon.TryShoot();
	}
}
