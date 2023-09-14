using System;

public abstract class Effect
{
	public Action<Effect> OnEffectTicked;
	public Action<Effect> OnEffectEnded;

	protected AI_Agent_Enemy _enemy;

	protected float _statusDuration;

	protected HorgueVFX _TickVFX;

	public abstract void Tick(float delta);

	public virtual void ResetDuration()
	{

	}

	public void TriggerVFX()
	{
		if(_TickVFX != null)
			_TickVFX.Play();
	}

	public virtual void EndEffect()
	{

	}
}