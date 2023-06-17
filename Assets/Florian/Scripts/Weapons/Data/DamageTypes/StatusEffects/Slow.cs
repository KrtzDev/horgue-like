using UnityEngine.AI;

public class Slow : Effect
{
	private float _slowAmount;

	private NavMeshAgent _agent;
	private float _originalSpeed;

	private bool _shouldSlow = true;

	public Slow(Enemy enemy, 
		float slowAmount, 
		float statusDuration)
	{
		_enemy = enemy;
		_slowAmount = slowAmount;
		_statusDuration = statusDuration;

		_agent = enemy.GetComponent<NavMeshAgent>();
		_originalSpeed = _agent.speed;
	}

	public override void Tick(float delta)
	{
		_statusDuration -= delta;

		if (_statusDuration <= 0)
		{
			_agent.speed = _originalSpeed;
			OnEffectEnded.Invoke(this);
			return;
		}

		if (_shouldSlow)
		{
			_shouldSlow = false;
			_agent.speed = _originalSpeed * ((100 - _slowAmount) / 100);
		}
	}
}