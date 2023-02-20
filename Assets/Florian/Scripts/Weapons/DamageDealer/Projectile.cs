using UnityEngine;

public class Projectile : DamageDealer
{
	public float finalBaseDamage;
	public float finalAttackSpeed;
	public float finalCooldown;
	public float finalProjectileSize;
	public float finalCritChance;
	public float finalRange;

	public AttackPattern attackPattern;
	public Transform spawnPosition;

	[SerializeField]
	private LayerMask _hitLayerMask;

	private void OnTriggerEnter(Collider other)
	{
		if ((_hitLayerMask.value & (1 << other.gameObject.layer)) > 0)
		{
			if (other.TryGetComponent(out HealthComponent enemyHealth))
			{
				enemyHealth.TakeDamage((int)finalBaseDamage);
			}
			Destroy(gameObject);
		}
	}
}
