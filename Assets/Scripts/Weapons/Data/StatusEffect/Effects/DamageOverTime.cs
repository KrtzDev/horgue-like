public class DamageOverTime : Effect
{
	private float _dotDamage;
	private float _startDuration;

	private HealthComponent _enemyhealth;

	public DamageOverTime(AI_Agent_Enemy enemy, float dotDamage, float statusDuration)
	{
		_enemy = enemy;
		_dotDamage = dotDamage;
		_statusDuration = statusDuration;

		_startDuration = statusDuration;

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

	public override void ResetDuration()
	{
		_statusDuration = _startDuration;
	}
}