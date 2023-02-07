using System.Collections.Generic;
using UnityEngine;

public class WeaponHolster : MonoBehaviour
{
    [SerializeField]
    private List<Weapon> weapons = new List<Weapon>();

    private void Start()
    {
        foreach (Weapon weapon in weapons)
        {
            weapon.Initialize(transform);
        }
    }

    private void Update()
    {
        TryShootAllWeapons();
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
