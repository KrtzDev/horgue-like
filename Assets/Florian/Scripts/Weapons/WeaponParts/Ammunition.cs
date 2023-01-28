using UnityEngine;

[CreateAssetMenu(fileName = "new Barrel", menuName = "ModularWeapon/WeaponParts/Ammunition")]
public class Ammunition : ScriptableObject
{
    public Projectile projectilePrefab;

    [Header("Stats")]
    public float baseDamage;
    public float attackSpeed;
    public float cooldown;
    public float projectileSize;
    public float critchance;
    public float range;

    public DamageType damageType;
}
