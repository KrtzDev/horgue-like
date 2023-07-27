using UnityEngine;

[CreateAssetMenu(fileName = "new Sight", menuName = "ModularWeapon/WeaponParts/Sight")]
public class Sight : WeaponPart
{
	[Header("Stats")]
	public float baseDamage;
    public float attackSpeed;
	public float cooldown;
	public float projectileSize;
    public float critChance;
    public float range;
}
