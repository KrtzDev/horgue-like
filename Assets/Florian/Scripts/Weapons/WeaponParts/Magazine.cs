using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ModularWeapon/WeaponParts/Magazine")]
public class Magazine : ScriptableObject
{
    public uint maxAmmoCount;
    public uint ammoCount;
    public DamageType damageType;

    public Projectile projectile;

    public bool isReloading;

    private void OnEnable()
    {
        isReloading = false;      
    }
}
