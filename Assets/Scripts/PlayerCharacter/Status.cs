using System;
using System.Collections.Generic;
using UnityEngine;

public class Status : MonoBehaviour
{
	[SerializeField]
	private List<StatusEffect> _statusEffects = new List<StatusEffect>();

	public void AddStatusEffect(StatusEffect statusEffect)
	{
		_statusEffects.Add(statusEffect);
		statusEffect.OnStatusEffectEnded += RemoveEffect;
		GetComponent<HealthComponent>().OnDeath += RemoveAllEffects;
	}

	public void Update()
	{
		for (int i = 0; i < _statusEffects.Count; i++)
			_statusEffects[i].OnUpdate();
	}

	private void RemoveEffect(StatusEffect statusEffect)
	{
		statusEffect.OnStatusEffectEnded -= RemoveEffect;
		_statusEffects.Remove(statusEffect);
	}

	public void RemoveAllEffects()
	{
		for (int i = 0; i < _statusEffects.Count; i++)
		{
			_statusEffects[i].RemoveAllEffects();
		}
		_statusEffects.Clear();
	}

	public bool HasStatusEffect(StatusEffectSO statusEffectSO)
	{
		for (int i = 0; i < _statusEffects.Count; i++)
		{
			if (_statusEffects[i].StatusEffectSO == statusEffectSO)
				return true;
		}
		return false;
	}
}
