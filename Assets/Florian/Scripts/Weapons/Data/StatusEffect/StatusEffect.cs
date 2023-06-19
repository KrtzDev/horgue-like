using System;
using System.Collections.Generic;
using UnityEngine;


public enum KnockBackType
{
	Push,
	Pull
}
[Serializable]
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
	[SerializeField] private HorgueVFX _initialDamageVFX;

	[Space]
	[SerializeField] private bool _hasDamageOverTime;
	[SerializeField] private float _dotDamage;
	[SerializeField] private HorgueVFX _dotDamageVFX;

	[Space]
	[SerializeField] private bool _hasSlow;
	[SerializeField] private float _slowAmount;
	[SerializeField] private HorgueVFX _slowVFX;

	[Space]
	[SerializeField] private bool _hasKnockBack;
	[SerializeField] private KnockBackType _knockBackType;
	[SerializeField] private float _knockBackStrength;
	[SerializeField] private HorgueVFX _knockBackVFX;

	[Space]
	[SerializeField] private bool _hasPierce;
	[SerializeField] private float _maxPierceAmount;

	[Header("Propagation")]
	[SerializeField] private bool _canPropagate;
	[SerializeField] private StatusEffectSO _propagatedEffect;
	[SerializeField] private LayerMask _layersToPropagateTo;
	[SerializeField] private float _propagationChance;
	[SerializeField] private float _propagationRange;
	[SerializeField] private int _maxPropagateToCount;
	[SerializeField] private int _consecutivePropagationCount;
	[SerializeField] private HorgueVFX _propagationVFX;

	private Enemy _enemy;
	private Projectile _projectile;

	private List<Effect> _effects = new List<Effect>();
	private float _currentEffectTimer;
	private float _thisTickRate;
	private float _currentTickTimer;
	float _delta = 0;

	public List<Enemy> propagatedToEnemies = new List<Enemy>();
	private int _timesPropagated = 0;

	public StatusEffect(StatusEffectSO statusEffectSO)
	{
		_triggerChance = statusEffectSO.triggerChance;
		_effectDuration = statusEffectSO.effectDuration;
		_tickRate = statusEffectSO.tickRate;

		_hasInitialExtraDamage = statusEffectSO.hasInitialExtraDamage;
		_initialDamage = statusEffectSO.initialDamage;
		_initialDamageVFX = statusEffectSO.initialDamageVFX;

		_hasDamageOverTime = statusEffectSO.hasDamageOverTime;
		_dotDamage = statusEffectSO.dotDamage;
		_dotDamageVFX = statusEffectSO.dotDamageVFX;

		_hasSlow = statusEffectSO.hasSlow;
		_slowAmount = statusEffectSO.slowAmount;
		_slowVFX = statusEffectSO.slowVFX;

		_hasKnockBack = statusEffectSO.hasKnockBack;
		_knockBackType = statusEffectSO.knockBackType;
		_knockBackStrength = statusEffectSO.knockBackStrength;
		_knockBackVFX = statusEffectSO.knockBackVFX;

		_hasPierce = statusEffectSO.hasPierce;
		_maxPierceAmount = statusEffectSO.maxPierceAmount;

		_canPropagate = statusEffectSO.canPropagate;
		_propagatedEffect = statusEffectSO.propagatedEffect;
		_layersToPropagateTo = statusEffectSO.layersToPropagateTo;
		_propagationChance = statusEffectSO.propagationChance;
		_propagationRange = statusEffectSO.propagationRange;
		_maxPropagateToCount = statusEffectSO.maxPropagateToCount;
		_consecutivePropagationCount = statusEffectSO.consecutivePropagationCount;
		_propagationVFX = statusEffectSO.propagationVFX;
	}

	public void ApplyStatusEffect(Enemy enemy, Projectile projectile)
	{
		OnHitEnemy += OnEnemyHit;

		if (_triggerChance < UnityEngine.Random.Range(1, 100))
			return;

		_enemy = enemy;
		_projectile = projectile;

		if (_hasInitialExtraDamage)
			AddEffect(new DamageOnce(_enemy, _initialDamage));
		if (_hasDamageOverTime)
			AddEffect(new DamageOverTime(_enemy, _dotDamage, _effectDuration));
		if (_hasSlow)
			AddEffect(new Slow(_enemy, _slowAmount, _effectDuration));
		if (_hasKnockBack)
		{
			Vector3 knockBackDirection = Vector3.zero;
			switch (_knockBackType)
			{
				case KnockBackType.Push:
					knockBackDirection = _enemy.transform.position - _projectile.transform.position;
					break;
				case KnockBackType.Pull:
					knockBackDirection = _projectile.transform.position - _enemy.transform.position;
					break;
			}
			AddEffect(new KnockBack(_enemy, new Vector3(knockBackDirection.x, 0, knockBackDirection.z).normalized * _knockBackStrength));
		}

		for (int i = 0; i < _effects.Count; i++)
		{
			_effects[i].OnEffectTicked += TriggerVisualEffect;
		}

		_enemy.GetComponent<Status>().AddStatusEffect(this);

		_currentEffectTimer = _effectDuration;
		_thisTickRate = UnityEngine.Random.Range(_tickRate.min, _tickRate.max);
	}

	private void TriggerVisualEffect(Effect effect)
	{
		effect.TriggerVFX();
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
		if (_effects == null)
			_effects = new List<Effect>();

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
		if (!_hasPierce)
		{
			projectile.OnHit.Invoke(projectile);
			return;
		}

		projectile.PierceAmount++;

		if (projectile.PierceAmount >= _maxPierceAmount)
			projectile.OnHit.Invoke(projectile);
	}

	private void OnEffectsTicked()
	{
		Debug.Log(_timesPropagated);
		if (!_canPropagate || _timesPropagated >= _consecutivePropagationCount)
			return;

		Propagate();
	}

	private void Propagate()
	{
		if (_propagationChance < UnityEngine.Random.Range(1, 100))
			return;

		_propagationVFX?.Play();
		_timesPropagated++;

		Collider[] enemies = Physics.OverlapSphere(_enemy.gameObject.transform.position, _propagationRange, _layersToPropagateTo);
		if (enemies.Length < 0)
			return;

		int propagatedTo = 0;


		for (int i = 0; i < enemies.Length; i++)
		{
			Enemy enemy = enemies[i].GetComponent<Enemy>();
			if (enemy == _enemy || propagatedToEnemies.Contains(enemy))
				continue;

			if (propagatedTo < _maxPropagateToCount)
			{
				if (enemies[i].TryGetComponent(out Status status))
				{
					StatusEffect effectToPropagate = new StatusEffect(_propagatedEffect);
					effectToPropagate._timesPropagated++;
					propagatedToEnemies.Add(enemy);
					effectToPropagate.propagatedToEnemies = propagatedToEnemies;
					effectToPropagate.ApplyStatusEffect(enemy, _projectile);
					propagatedTo++;
				}
			}
		}

	}
}
