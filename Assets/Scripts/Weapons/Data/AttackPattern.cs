using UnityEngine;

public abstract class AttackPattern : ScriptableObject
{
	public abstract Pattern GetPattern();
	public abstract string PatternName();
	public abstract Projectile[] SpawnProjectiles(WeaponStats weaponStats, ObjectPool<Projectile> projectilePool, Pattern spawnedPattern);
}
