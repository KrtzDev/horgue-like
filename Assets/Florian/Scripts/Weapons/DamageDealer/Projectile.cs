using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float finalBaseDamage;
    public float finalAttackSpeed;
    public float finalCooldown;
    public float finalProjectileSize;
    public float finalCritChance;
    public float finalRange;

    public AttackPattern attackPattern;
    public Transform spawnPosition;
}
