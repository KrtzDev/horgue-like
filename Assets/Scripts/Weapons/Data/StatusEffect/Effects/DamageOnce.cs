
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
		_enemyHealth.TakeDamage((int)_additionalDamage, false);

		OnEffectTicked?.Invoke(this);
		OnEffectEnded?.Invoke(this);
	}
}