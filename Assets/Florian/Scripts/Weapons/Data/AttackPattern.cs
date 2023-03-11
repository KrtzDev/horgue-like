using UnityEngine;

public abstract class AttackPattern : ScriptableObject, IAttackInPattern
{
	public abstract string PatternName();
    public abstract DamageDealer AttackInPattern(Projectile projectile, Transform spawnPosition);
}
