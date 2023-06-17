using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "new StatusEffect", menuName = "ModularWeapon/Data/StatusEffect")]
public class StatusEffect : ScriptableObject
{
	public Action<StatusEffect> OnStatusEffectEnded;

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

	[Header("Propagation")]
	[SerializeField] private bool _canPropagate;
	[SerializeField] private StatusEffect _propagatedEffect;
	[SerializeField] private float _propagationChance;
	[SerializeField] private float _propagationRange;
	[SerializeField] private int _maxPropagateToCount;
	[SerializeField] private int _consecutivePropagationCount;

	private Enemy _enemy;

	private List<Effect> _statusEffects = new List<Effect>();
	private float _currentEffectTimer;
	private float _thisTickRate;
	private float _currentTickTimer;
	float _delta = 0;

	public int TimesPropagated { get; set; }


	public void ApplyStatusEffect(Enemy enemy)
	{
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

	private void AddEffect(Effect effect)
	{
		_statusEffects.Add(effect);
		effect.OnEffectEnded += RemoveEffect;
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

		for (int i = 0; i < _statusEffects.Count; i++)
			_statusEffects[i].Tick(_delta);

		_delta = 0;

		OnEffectsTicked();
	}

	private void RemoveEffect(Effect effect)
	{
		effect.OnEffectEnded -= RemoveEffect;
		_statusEffects.Remove(effect);
	}

	private void OnEffectsTicked()
	{
		if (!_canPropagate && TimesPropagated < _consecutivePropagationCount)
			return;

		Propagate();
	}

	private void Propagate()
	{
		if (_propagationChance < UnityEngine.Random.Range(1, 100))
			return;

		StatusEffect effectToPropagate = _propagatedEffect == null ? this : _propagatedEffect;

		int propagationCount = 0;

		Collider[] enemies = Physics.OverlapSphere(_enemy.gameObject.transform.position, _propagationRange, LayerMask.NameToLayer("Enemy"));

		if (enemies.Length < 0)
			return;

		for (int i = 0; i < enemies.Length; i++)
		{
			if (enemies[i].GetComponent<Enemy>() == _enemy)
				continue;
			if (propagationCount < _maxPropagateToCount)
			{
				if(enemies[i].TryGetComponent(out Status status))
					status.AddStatusEffect(effectToPropagate);
			}
		}

		effectToPropagate.TimesPropagated++;
	}
}
