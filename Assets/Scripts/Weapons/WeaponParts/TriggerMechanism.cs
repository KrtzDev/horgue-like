using UnityEngine;

[CreateAssetMenu(fileName = "new TriggerMechanism",menuName = "ModularWeapon/WeaponParts/TriggerMechanism")]
public class TriggerMechanism : WeaponPart
{
    [Header("Stats")]
    public float baseDamage;
    public float attackSpeed;
    public float cooldown;
	public float projectileSize;
    public float critChance;
    public float range;
}
