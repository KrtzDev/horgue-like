using UnityEngine;
using UnityEngine.AI;

public class KnockBack : Effect
{
	private Vector3 _knockBackDirection;

	private NavMeshAgent _agent;
	private Rigidbody _rigidbody;

	private bool _wasKnockBacked = false;
	private float _currentKnockBackTimer = 0;

	public KnockBack(Enemy enemy, Vector3 knockBackDirection)
	{
		_enemy = enemy;
		_knockBackDirection = knockBackDirection;

		_agent = _enemy.GetComponent<NavMeshAgent>();
		_rigidbody = _enemy.GetComponent<Rigidbody>();
	}

	public override void Tick(float delta)
	{
		if (!_wasKnockBacked)
		{
			OnEffectTicked?.Invoke(this);
			_wasKnockBacked = true;
		}

		_agent.enabled = false;
		_rigidbody.AddForce(_knockBackDirection, ForceMode.Impulse);

		_currentKnockBackTimer += Time.deltaTime;
		if(_currentKnockBackTimer > .2f)
		{
			_agent.enabled = true;
			_rigidbody.velocity = Vector3.zero;
			OnEffectEnded.Invoke(this);
		}
	}
}
