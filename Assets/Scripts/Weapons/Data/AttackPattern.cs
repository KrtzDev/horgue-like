using UnityEngine;

public abstract class AttackPattern : ScriptableObject
{
	public abstract Pattern GetPattern();
	public abstract string PatternName();
	public abstract Projectile[] SpawnProjectiles(int capacity, ObjectPool<Projectile> projectilePool, Pattern spawnedPattern);
}
