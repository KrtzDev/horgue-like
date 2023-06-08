using UnityEngine;

public class DamageOverTime : StatusEffect
{
	private HealthComponent _enemyhealth;

	private float _dotDamage;
	private float _dotDuration;
	private float _propagationChance;

	private float _randomTickTimer;

	public DamageOverTime(HealthComponent enemyHealth, float burnDamage, float burnDuration, float propagationChance)
	{
		_enemyhealth = enemyHealth;
		_dotDamage = burnDamage;
		_dotDuration = burnDuration;
		_propagationChance = propagationChance;

		_randomTickTimer = Random.Range(1f,3f);
	}

	public override void Tick()
	{
		_dotDuration -= Time.deltaTime;
		_randomTickTimer -= Time.deltaTime;

		if (_dotDuration <= 0)
		{
			OnEffectEnded?.Invoke(this);
			return;
		}

		if (_randomTickTimer <= 0f)
		{
			_randomTickTimer = Random.Range(1f, 3f);
			_enemyhealth.TakeDamage((int)_dotDamage);
			CheckPropagation();
			Debug.Log("Tick DoT " + _dotDamage);
		}
	}

	//ToDo: OnCollisionEnter event from Enemy 
	private void CheckPropagation()
	{
		if (_propagationChance > Random.Range(1, 100))
			Propagate(/*collision*/);
	}

	private void Propagate()
	{
		//ToDo: Add this StatusEffect to colliding Enemy
	}
}