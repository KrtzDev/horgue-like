using UnityEngine;

[CreateAssetMenu(fileName = "new Barrel", menuName = "ModularWeapon/WeaponParts/Barrel")]
public class Barrel : WeaponPart
{
    [Header("Stats")]
    public float baseDamage;
    public float attackSpeed;
    public float cooldown;
    public float projectileSize;
    public float critChance;
    public float range;

    public AttackPattern attackPattern;
	public MotionPattern motionPattern;
}
