using UnityEngine;

[CreateAssetMenu(fileName = "new Barrel", menuName = "ModularWeapon/WeaponParts/Ammunition")]
public class Ammunition : WeaponPart
{
    public Projectile projectilePrefab;

    [Header("Stats")]
    public float baseDamage;
    public float attackSpeed;
    public float cooldown;
    public float projectileSize;
    public float critChance;
    public float range;

    public StatusEffectSO statusEffect;
}
