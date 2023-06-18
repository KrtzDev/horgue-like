using System;
using System.Collections.Generic;
using UnityEngine;

public class StatusEffect
{
	public Action<StatusEffect> OnStatusEffectEnded;
	public Action<Projectile> OnHitEnemy;

	[Header("General")]
	[SerializeField] private float _triggerChance;
	[SerializeField] private float _effectDuration;
	[SerializeField] private FloatRange _tickRate;

	[Header("Effects")]
	[SerializeField] private bool _hasInitialExtraDamage;
	[SerializeField] private float _initialDamage;

	[Space]
	[SerializeField] private bool _hasDamageOverTime;
	[SerializeField] private float _dotDamage;

	[Space]
	[SerializeField] private bool _canSlow;
	[SerializeField] private float _slowAmount;

	[Space]
	[SerializeField] private float _maxPierceAmount;

	[Header("Propagation")]
	[SerializeField] private bool _canPropagate;
	[SerializeField] private StatusEffectSO _propagatedEffect;
	[SerializeField] private float _propagationChance;
	[SerializeField] private float _propagationRange;
	[SerializeField] private int _maxPropagateToCount;
	[SerializeField] private int _consecutivePropagationCount;

	private Enemy _enemy;

	private List<Effect> _effects = new List<Effect>();
	private float _currentEffectTimer;
	private float _thisTickRate;
	private float _currentTickTimer;
	float _delta = 0;

	private int _timesPropagated;


	public StatusEffect(StatusEffectSO statusEffectSO)
	{
		_triggerChance = statusEffectSO.triggerChance;
		_effectDuration = statusEffectSO.effectDuration;
		_tickRate = statusEffectSO.tickRate;

		_hasInitialExtraDamage = statusEffectSO.hasInitialExtraDamage;
		_initialDamage = statusEffectSO.initialDamage;

		_hasDamageOverTime = statusEffectSO.hasDamageOverTime;
		_dotDamage = statusEffectSO.dotDamage;

		_canSlow = statusEffectSO.canSlow;
		_slowAmount = statusEffectSO.slowAmount;

		_maxPierceAmount = statusEffectSO.maxPierceAmount;

		_canPropagate = statusEffectSO.canPropagate;
		_propagatedEffect = statusEffectSO.propagatedEffect;
		_propagationChance = statusEffectSO.propagationChance;
		_propagationRange = statusEffectSO.propagationRange;
		_maxPropagateToCount = statusEffectSO.maxPropagateToCount;
		_consecutivePropagationCount = statusEffectSO.consecutivePropagationCount;
	}

	public void ApplyStatusEffect(Enemy enemy)
	{
		OnHitEnemy += OnEnemyHit;

		if (_triggerChance < UnityEngine.Random.Range(1, 100))
			return;

		_enemy = enemy;

		if (_hasInitialExtraDamage)
			AddEffect(new DamageOnce(_enemy, _initialDamage));
		if (_hasDamageOverTime)
			AddEffect(new DamageOverTime(_enemy, _dotDamage, _effectDuration));
		if (_canSlow)
			AddEffect(new Slow(_enemy, _slowAmount, _effectDuration));

		_enemy.GetComponent<Status>().AddStatusEffect(this);

		_currentEffectTimer = _effectDuration;
		_thisTickRate = UnityEngine.Random.Range(_tickRate.min, _tickRate.max);
	}

	public void RemoveAllEffects()
	{
		for (int i = 0; i < _effects.Count; i++)
		{
			_effects[i].OnEffectEnded -= RemoveEffect;
		}
		_effects.Clear();
	}

	private void AddEffect(Effect effect)
	{
		_effects.Add(effect);
		effect.OnEffectEnded += RemoveEffect;
	}

	private void RemoveEffect(Effect effect)
	{
		effect.OnEffectEnded -= RemoveEffect;
		_effects.Remove(effect);
	}

	public void OnUpdate()
	{
		_delta += Time.deltaTime;
		_currentTickTimer -= Time.deltaTime;
		_currentEffectTimer -= Time.deltaTime;

		if (_currentEffectTimer < 0)
		{
			OnStatusEffectEnded.Invoke(this);
			return;
		}

		if (_currentTickTimer > 0)
			return;

		_currentTickTimer = _thisTickRate;

		for (int i = 0; i < _effects.Count; i++)
			_effects[i].Tick(_delta);

		_delta = 0;

		OnEffectsTicked();
	}

	private void OnEnemyHit(Projectile projectile)
	{
		projectile.PierceAmount++;

		if (_maxPierceAmount <= 0)
			projectile.OnHit.Invoke(projectile);
	}

	private void OnEffectsTicked()
	{
		if (!_canPropagate && _timesPropagated < _consecutivePropagationCount)
			return;

		Propagate();
	}

	private void Propagate()
	{
		if (_propagationChance < UnityEngine.Random.Range(1, 100))
			return;

		StatusEffect effectToPropagate = new StatusEffect(_propagatedEffect);

		int propagatedTo = 0;

		Collider[] enemies = Physics.OverlapSphere(_enemy.gameObject.transform.position, _propagationRange, LayerMask.NameToLayer("Enemy"));

		if (enemies.Length < 0)
			return;

		for (int i = 0; i < enemies.Length; i++)
		{
			if (enemies[i].GetComponent<Enemy>() == _enemy)
				continue;

			if (propagatedTo < _maxPropagateToCount)
			{
				if(enemies[i].TryGetComponent(out Status status))
				{
					status.AddStatusEffect(effectToPropagate);
					propagatedTo++;
				}
			}
		}

		effectToPropagate._timesPropagated++;
	}
}
