internal class DamageOnce : Effect
{
	private float _additionalDamage;

	private HealthComponent _enemyHealth;

	public DamageOnce(Enemy enemy, float additionalDamage)
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