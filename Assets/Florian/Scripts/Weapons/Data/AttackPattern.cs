using UnityEngine;

public abstract class AttackPattern : ScriptableObject, IAttackInPattern
{
    public abstract void AttackInPattern(Projectile projectile, Transform spawnPosition);
}
