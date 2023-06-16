public class Slow : StatusEffect
{
	private Enemy _enemy;
	private float _slowAmount;
	private float _freezeDuration;

	public Slow(Enemy enemy, float slowAmount, float freezeDuration)
	{
		_enemy=enemy;
		_slowAmount=slowAmount;
		_freezeDuration=freezeDuration;
	}

	public override void Tick()
	{
		base.Tick();
	}
}