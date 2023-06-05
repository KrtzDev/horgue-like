using UnityEngine;

public abstract class AttackPattern : ScriptableObject, IAttackInPattern
{
	public abstract string PatternName();
    public abstract void AttackInPattern(Projectile projectile);
}
