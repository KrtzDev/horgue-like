using UnityEngine;

[CreateAssetMenu(fileName = "new FireDamage", menuName = "ModularWeapon/Data/DamageType/Fire")]
public class FireDamage : DamageType
{
	[SerializeField] private float _additionalDamage;
	[SerializeField] private float _burnChance;
	[SerializeField] private float _burnDamage;
	[SerializeField] private float _propagationChance;
	[SerializeField] private float _propagationRange;

	[SerializeField] private float _statusDuration;
	[SerializeField] private FloatRange _tickRate;

	public override void ApplyEffect(Enemy enemy)
	{
		if (_burnChance < Random.Range(1, 100))
			return;

		enemy.GetComponent<Status>().AddEffect(new DamageOnce(enemy, _additionalDamage));
		enemy.GetComponent<Status>().AddEffect(new DamageOverTime(enemy, _burnDamage, _statusDuration, _tickRate, _propagationChance, _propagationRange));
	}
}
