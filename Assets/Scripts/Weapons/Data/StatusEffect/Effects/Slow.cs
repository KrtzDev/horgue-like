using UnityEngine;
using UnityEngine.AI;

public class Slow : Effect
{
	private float _slowAmount;
	private float _startDuration;

	private NavMeshAgent _agent;
	private float _originalSpeed;

	private bool _shouldSlow = true;

	public Slow(AI_Agent_Enemy enemy, float slowAmount, float statusDuration)
	{
		_enemy = enemy;
		_slowAmount = slowAmount;
		_statusDuration = statusDuration;

		_startDuration = statusDuration;

		_agent = enemy.GetComponent<NavMeshAgent>();
		_originalSpeed = _agent.speed;
	}

	public override void Tick(float delta)
	{
		_statusDuration -= delta;
		if(_enemy is AI_Agent_Rikayon)
			Debug.Log(_statusDuration);

		if (_statusDuration <= 0)
		{
			Debug.Log("end slow");
			_agent.speed = _originalSpeed;
			OnEffectEnded?.Invoke(this);
			return;
		}

		if (_shouldSlow)
		{
			_shouldSlow = false;
			_agent.speed = _originalSpeed * ((100 - _slowAmount) / 100);
			OnEffectTicked?.Invoke(this);
		}
	}

	public override void ResetDuration()
	{
		_statusDuration = _startDuration;
	}

	public override void EndEffect()
	{
		_agent.speed = _originalSpeed;
	}
}