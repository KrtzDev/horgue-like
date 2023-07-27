using UnityEngine;

[CreateAssetMenu(fileName = "new SkeletonBaseStats", menuName = "ModularWeapon/SkeletonBaseStats")]
public class SkeletonBaseStats : ScriptableObject
{
	public float baseDamage;
	public float attackSpeed;
	public float cooldown;
	public float projectileSize;
	public float critChance;
	public float range;
}