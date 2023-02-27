using UnityEngine;

public interface IAttackInPattern
{
    DamageDealer AttackInPattern(Projectile projectile, Transform spawnPosition);
}
