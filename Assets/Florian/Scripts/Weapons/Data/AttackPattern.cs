using UnityEngine;

public abstract class AttackPattern : ScriptableObject, IAttackInPattern
{
    public abstract DamageDealer AttackInPattern(Projectile projectile, Transform spawnPosition);
}
