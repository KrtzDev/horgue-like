using UnityEngine;

public class DamageOnce : Effect
{
	private float _additionalDamage;

	private HealthComponent _enemyHealth;

	public DamageOnce(AI_Agent_Enemy enemy, float additionalDamage)
	{
		_enemy = enemy;
		_additionalDamage = additionalDamage;

		_enemyHealth = enemy.GetComponent<HealthComponent>();
	}

	public override void Tick(float delta)
	{
		Debug.Log("Damage: " + _additionalDamage + "  at " + _enemyHealth.currentHealth + " results at");
		_enemyHealth.TakeDamage((int)_additionalDamage, false);
		Debug.Log(_enemyHealth.currentHealth);

		OnEffectTicked?.Invoke(this);
		OnEffectEnded?.Invoke(this);
	}
}