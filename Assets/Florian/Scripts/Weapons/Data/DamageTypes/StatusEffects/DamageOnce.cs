internal class DamageOnce : StatusEffect
{
	private float _additionalDamage;

	private HealthComponent _enemyHealth;

	public DamageOnce(Enemy enemy, float additionalDamage, float propagationChance = 0, float propagationRange = 0)
	{
		_enemy = enemy;
		_additionalDamage = additionalDamage;

		_propagationChance = propagationChance;
		_propagationRange = propagationRange;

		_enemyHealth = enemy.GetComponent<HealthComponent>();
	}

	public override void Tick()
	{
		_enemyHealth.TakeDamage((int)_additionalDamage);
		CheckPropagation(this);

		OnEffectEnded?.Invoke(this);
	}
}