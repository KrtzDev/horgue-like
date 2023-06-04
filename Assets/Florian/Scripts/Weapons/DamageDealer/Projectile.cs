using System;
using UnityEngine;

public class Projectile : DamageDealer
{
	public event Action<Projectile> OnHit;
	public event Action<Projectile> OnLifeTimeEnd;

	public float finalBaseDamage;
	public float finalAttackSpeed;
	public float finalCooldown;
	public float finalProjectileSize;
	public float finalCritChance;
	public float finalRange;

	public AttackPattern attackPattern;
	public DamageType damageType;
	public Transform spawnTransform;

	[SerializeField]
	private LayerMask _hitLayerMask;

	private void OnEnable()
	{
		float lifeTime = 10;
		lifeTime -= Time.deltaTime;
		if (lifeTime <= 0)
			OnLifeTimeEnd?.Invoke(this);
	}

	private void OnTriggerEnter(Collider other)
	{
		if ((_hitLayerMask.value & (1 << other.gameObject.layer)) > 0)
		{
			if (other.TryGetComponent(out HealthComponent health))
			{
				if (finalCritChance > UnityEngine.Random.Range(0,100))
					finalBaseDamage *= 2;

				health.TakeDamage((int)finalBaseDamage);
				if(health.TryGetComponent(out Enemy enemy))
					damageType.ApplyEffect(enemy);
			}
			OnHit?.Invoke(this);
		}
	}
}
