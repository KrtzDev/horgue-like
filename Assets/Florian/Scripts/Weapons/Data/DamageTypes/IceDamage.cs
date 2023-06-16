using UnityEngine;

[CreateAssetMenu(fileName = "new IceDamage", menuName = "ModularWeapon/Data/DamageType/Ice")]
public class IceDamage : DamageType
{
	[SerializeField] private float _freezeChance;
	[SerializeField] private float _freezeDamage;
	[SerializeField] private float _freezeDuration;
	[SerializeField] private float _slowAmount;

	[SerializeField] private float _statusDuration;
	[SerializeField] private FloatRange _tickRate;

	public override void ApplyEffect(Enemy enemy)
	{
		if (_freezeChance < Random.Range(1, 100))
			return;

		enemy.GetComponent<Status>().AddEffect(new DamageOverTime(enemy, _freezeDamage, _statusDuration));
		enemy.GetComponent<Status>().AddEffect(new Slow(enemy, _slowAmount, _freezeDuration, _statusDuration));
	}
}
