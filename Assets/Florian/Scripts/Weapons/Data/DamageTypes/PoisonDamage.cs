using UnityEngine;

[CreateAssetMenu(fileName = "new PoisonDamage", menuName = "ModularWeapon/Data/DamageType/Poison")]
public class PoisonDamage : DamageType
{
	[SerializeField] private float _poisonChance;
	[SerializeField] private float _poisonDamage;

	[SerializeField] private float _statusDuration;
	[SerializeField] private FloatRange _tickRate;

    public override void ApplyEffect(Enemy enemy)
    {
		if (_poisonChance < Random.Range(1, 100))
			return;

		enemy.GetComponent<Status>().AddEffect(new DamageOverTime(enemy, _poisonDamage, _statusDuration));
    }
}
