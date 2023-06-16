using UnityEngine;

public class DamageOverTime : StatusEffect
{
	private float _dotDamage;

	private HealthComponent _enemyhealth;

	public DamageOverTime(Enemy enemy, 
		float dotDamage, 
		float statusDuration, 
		FloatRange tickRate = default, 
		float propagationChance = 0f, 
		float propagationRange = 0f)
	{
		_enemy = enemy;
		_dotDamage = dotDamage;
		_statusDuration = statusDuration;

		_propagationChance = propagationChance;
		_propagationRange = propagationRange;
		_tickRate = tickRate;
		_randomTickTimer = Random.Range(tickRate.min,tickRate.max);

		_enemyhealth = _enemy.GetComponent<HealthComponent>();
	}

	public override void Tick()
	{
		_statusDuration -= Time.deltaTime;

		if (_randomTickTimer > 0)
			_randomTickTimer -= Time.deltaTime;

		if (_statusDuration <= 0)
		{
			OnEffectEnded?.Invoke(this);
			return;
		}

		if (_randomTickTimer <= 0f)
		{
			_randomTickTimer = Random.Range(_tickRate.min, _tickRate.max);
			_enemyhealth.TakeDamage((int)_dotDamage);

			CheckPropagation(this);
		}
	}
}