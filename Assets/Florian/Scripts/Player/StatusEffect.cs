using UnityEngine;
using System;

public abstract class StatusEffect
{
	public Action<StatusEffect> OnEffectEnded;

	public virtual void Tick()
	{
		Debug.Log("Tick effect");
	}
}