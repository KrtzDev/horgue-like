using UnityEngine;
using System;

public abstract class StatusEffect
{
	public Action<StatusEffect> OnEffectEnded;

	protected Enemy _enemy;

	protected float _statusDuration;

	protected float _propagationChance;
	protected float _propagationRange;

	protected FloatRange _tickRate;
	protected float _randomTickTimer;

	public abstract void Tick();

	protected void CheckPropagation(StatusEffect statusEffect)
	{
		if (_propagationChance >= 0)
			return;

			Propagate(statusEffect);
	}

	private void Propagate(StatusEffect statusEffect)
	{
		Collider[] enemies = Physics.OverlapSphere(_enemy.transform.position, _propagationRange, LayerMask.NameToLayer("Enemy"));
		for (int i = 0; i < enemies.Length; i++)
		{
			if (_propagationChance > UnityEngine.Random.Range(1, 100))
				enemies[i].GetComponent<Status>().AddEffect(statusEffect);
		}
	}
}