using UnityEngine;

[CreateAssetMenu(fileName = "new Magazine", menuName = "ModularWeapon/WeaponParts/Magazine")]
public class Magazine : WeaponPart
{
    [Header("Stats")]
    public float attackSpeed;
    public float cooldown;
    public float projectileSize;

    public int capacity;

    [Header("Attachements")]
    public int maxAmmoSlots;
}
