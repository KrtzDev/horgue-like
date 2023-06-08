using UnityEngine;

public class DamageOverTime : StatusEffect
{
	private Enemy _enemy;
	private float _burnDamage;
	private float _burnDuration;
	private float _propagationChance;

	private float _randomTickTimer;

	public DamageOverTime(Enemy enemy, float burnDamage, float burnDuration, float propagationChance)
	{
		_enemy = enemy;
		_burnDamage = burnDamage;
		_burnDuration = burnDuration;
		_propagationChance = propagationChance;

		_randomTickTimer = Random.Range(1f,3f);
	}

	public override void Tick()
	{
		_burnDuration -= Time.deltaTime;
		_randomTickTimer -= Time.deltaTime;

		if (_burnDuration <= 0)
		{
			OnEffectEnded?.Invoke(this);
			return;
		}

		if (_randomTickTimer <= 0f)
		{
			_randomTickTimer = Random.Range(1f, 3f);
			Debug.Log("Tick DoT " + _burnDamage);
		}


	}
}