using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEngine;

[CreateAssetMenu(fileName = "new SkeletonBaseStats", menuName = "ModularWeapon/SkeletonBaseStats")]
public class SkeletonBaseStats : ScriptableObject
{
	[Header("Stats")]
	public float baseDamage;
	public float attackSpeed;
	public float cooldown;
	public float projectileSize;
	public float critChance;
	public float critDamage;
	public float range;

	[Header("Max Stats")]
	public float maxAttackSpeed;
	public float maxCooldown;
	public float maxProjectileSize;
	public float maxCritChance;

	[Header("Min Stats")]
	public float minBaseDamage;
	public float minAttackSpeed;
	public float minCooldown;
	public float minProjectileSize;
	public float minCritChance;
	public float minCritDamage;
	public float minRange;
}