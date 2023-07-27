using UnityEngine;

[CreateAssetMenu(fileName = "new Grip", menuName = "ModularWeapon/WeaponParts/Grip")]
public class Grip : WeaponPart
{
	[Header("Stats")]
	public float baseDamage;
    public float attackSpeed;
    public float cooldown;
	public float projectileSize;
    public float critChance;
	public float range;

    [Header("Attachements")]
    public uint maxBarrels;
    public uint maxMagazines;
}
