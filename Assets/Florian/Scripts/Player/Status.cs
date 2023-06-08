using System;
using System.Collections.Generic;
using UnityEngine;

public class Status : MonoBehaviour
{
	[SerializeField]
	private List<StatusEffect> _statusEffects = new List<StatusEffect>();

	public void AddEffect(StatusEffect statusEffect)
	{
		_statusEffects.Add(statusEffect);
		statusEffect.OnEffectEnded += RemoveEffect;
	}

	public void Update()
	{
		foreach (var effect in _statusEffects)
		{
			effect.Tick();
		}
	}

	private void RemoveEffect(StatusEffect statusEffect)
	{
		_statusEffects.Remove(statusEffect);
	}

}
