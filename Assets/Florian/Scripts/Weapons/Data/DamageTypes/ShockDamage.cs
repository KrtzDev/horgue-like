using UnityEngine;

public class ShockDamage : DamageType
{
	[SerializeField] private float _additionalDamage;
	[SerializeField] private float _shockChance;
	[SerializeField] private float _slowDuration;
	[SerializeField] private float _slowAmount;
	[SerializeField] private float _propagationChance;
	[SerializeField] private float _propagationRange;

	[SerializeField] private float _statusDuration;

	public override void ApplyEffect(Enemy enemy)
	{
		if (_shockChance < Random.Range(1, 100))
			return;

		enemy.GetComponent<Status>().AddEffect(new DamageOnce(enemy, _additionalDamage, _propagationChance, _propagationRange));
		enemy.GetComponent<Status>().AddEffect(new Slow(enemy, _slowAmount, _slowDuration, _statusDuration, new FloatRange(), _propagationChance, _propagationRange));
	}
}
