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
	[SerializeField]
	private LayerMask _enemyLayerMask;

	public Enemy TargetedEnemy { get; set; }
	public int PierceAmount { get; set; }
	public float LifeTime { get; set; }

	public void Update()
	{
		motionPattern.UpdateMotion(this);
	}

	public void LateUpdate()
	{
		motionPattern.LateUpdateMotion(this);
	}

	public void DoExplosion()
	{
		if (motionPattern.explosionRange < 0)
			return;

		HorgueVFX spawnedVfx = motionPattern.explosionVfxPool.GetObject();
		spawnedVfx.transform.position = transform.position;
		spawnedVfx.Play();
		spawnedVfx.ReturnToPoolOnFinished(motionPattern.explosionVfxPool);

		Collider[] _hitEnemies = Physics.OverlapSphere(transform.position, motionPattern.explosionRange, _enemyLayerMask);
		for (int i = 0; i < _hitEnemies.Length; i++)
		{
			if (_hitEnemies[i] != null)
				DoDamage(_hitEnemies[i]);
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
