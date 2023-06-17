internal class DamageOnce : Effect
{
	private float _additionalDamage;

	private HealthComponent _enemyHealth;

	public DamageOnce(Enemy enemy, float additionalDamage, float propagationChance = 0, float propagationRange = 0)
	{
		_enemy = enemy;
		_additionalDamage = additionalDamage;

		_enemyHealth = enemy.GetComponent<HealthComponent>();
	}

	public override void Tick(float delta)
	{
		_enemyHealth.TakeDamage((int)_additionalDamage);

		OnEffectEnded?.Invoke(this);
	}
}