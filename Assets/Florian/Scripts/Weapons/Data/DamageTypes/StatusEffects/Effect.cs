using System;

public abstract class Effect
{
	public Action OnEffectTicked;
	public Action<Effect> OnEffectEnded;

	protected Enemy _enemy;

	protected float _statusDuration;



	public abstract void Tick(float delta);
}