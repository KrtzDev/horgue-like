using UnityEngine;

[CreateAssetMenu(fileName = "new Magazine", menuName = "ModularWeapon/WeaponParts/Magazine")]
public class Magazine : WeaponPart
{
	[Header("Stats")]
	public float baseDamage;
    public float attackSpeed;
    public float cooldown;
    public float projectileSize;
	public float critChance;
    public float critDamage;
	public float range;

    public int capacity;

    [Header("Attachements")]
    public int maxAmmoSlots;
}
