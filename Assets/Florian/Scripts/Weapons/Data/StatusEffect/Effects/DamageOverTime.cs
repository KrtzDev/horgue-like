public class DamageOverTime : Effect
{
	private float _dotDamage;

	private HealthComponent _enemyhealth;

	public DamageOverTime(Enemy enemy, float dotDamage, float statusDuration)
	{
		_enemy = enemy;
		_dotDamage = dotDamage;
		_statusDuration = statusDuration;

		_enemyhealth = _enemy.GetComponent<HealthComponent>();
	}

	public override void Tick(float delta)
	{
		_statusDuration -= delta;

		if (_statusDuration <= 0)
		{
			OnEffectEnded?.Invoke(this);
			return;
		}

		_enemyhealth.TakeDamage((int)_dotDamage);
		OnEffectTicked.Invoke(this);
	}
}