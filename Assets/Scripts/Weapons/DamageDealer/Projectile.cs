using System;
using UnityEngine;

public class Projectile : DamageDealer
{
	public Action<Projectile> OnHit;
	public Action<Projectile> OnLifeTimeEnd;

	public float finalBaseDamage;
	public float finalAttackSpeed;
	public float finalCooldown;
	public float finalProjectileSize;
	public float finalCritChance;
	public float finalRange;

	public AttackPattern attackPattern;
	public MotionPattern motionPattern;
	public StatusEffectSO statusEffect;
	public Transform spawnTransform;

	[SerializeField]
	private LayerMask _hitLayerMask;

	public int PierceAmount { get; set; }

	public float LifeTime { get; set; }

	public void Update()
	{		
		motionPattern.UpdateMotion(this);
	}

	public void DoExplosion()
	{
		if (statusEffect.propagationRange < 0)
			return;

		HorgueVFX spawnedVFX = Instantiate(statusEffect.initialDamageVFX, transform.position, Quaternion.identity);
		spawnedVFX.Play();

		Collider[] _hitEnemies = new Collider[statusEffect.maxPropagateToCount];
		if( Physics.OverlapSphereNonAlloc(transform.position, statusEffect.propagationRange, _hitEnemies, LayerMask.NameToLayer("Enemy")) > 0)
		{
			for (int i = 0; i < _hitEnemies.Length; i++)
			{
				if (_hitEnemies[i] != null)
					DoDamage(_hitEnemies[i]);
			}
		}
	}

	private bool DoDamage(Collider collider)
	{
		if (collider.TryGetComponent(out HealthComponent health))
		{
			if (finalCritChance > UnityEngine.Random.Range(0, 100))
				finalBaseDamage *= 2;

			health.TakeDamage((int)finalBaseDamage);

			if (statusEffect != null)
			{
				if (health.TryGetComponent(out Enemy enemy))
				{
					StatusEffect thisStatusEffect = new StatusEffect(statusEffect);
					thisStatusEffect.ApplyStatusEffect(enemy, this);
					thisStatusEffect.OnHitEnemy.Invoke(this);
				}
			}
			return true;
		}
		return false;
	}

	private void OnTriggerEnter(Collider other)
	{
		if ((_hitLayerMask.value & (1 << other.gameObject.layer)) > 0)
		{
			if (!DoDamage(other))
			{
				OnHit?.Invoke(this);
			}
		}
	}
}
