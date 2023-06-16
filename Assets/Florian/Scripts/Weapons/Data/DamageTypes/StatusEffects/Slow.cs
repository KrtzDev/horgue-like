using UnityEngine;
using UnityEngine.AI;

public class Slow : StatusEffect
{
	private float _slowAmount;
	private float _slowDuration;

	private NavMeshAgent _agent;
	private float _originalSpeed;

	private bool _shouldSlow = true;
	private float _currentSlowDuration;

	public Slow(Enemy enemy, 
		float slowAmount, 
		float slowDuration, 
		float statusDuration, 
		FloatRange tickRate = default,
		float propagationChance = 0f,
		float propagationRange = 0f)
	{
		_enemy = enemy;
		_slowAmount = slowAmount;
		_slowDuration = slowDuration;
		_statusDuration = statusDuration;

		_propagationChance = propagationChance;
		_propagationRange = propagationRange;
		_randomTickTimer = Random.Range(tickRate.min,tickRate.max);

		_agent = enemy.GetComponent<NavMeshAgent>();
		_originalSpeed = _agent.speed;
	}

	public override void Tick()
	{
		_statusDuration -= Time.deltaTime;
		_currentSlowDuration -= Time.deltaTime;

		if (_randomTickTimer > 0)
			_randomTickTimer -= Time.deltaTime;

		if (_statusDuration <= 0)
		{
			_agent.speed = _originalSpeed;
			OnEffectEnded.Invoke(this);
			return;
		}

		if(_randomTickTimer <= 0)
		{
			_currentSlowDuration = _slowDuration;
			_shouldSlow = true;
		}

		if (_currentSlowDuration <= 0)
		{
			_agent.speed = _originalSpeed;
		}

		if (_shouldSlow)
		{
			_shouldSlow = false;
			_agent.speed = _originalSpeed * (_slowAmount / 100);

			CheckPropagation(this);
		}
	}
}