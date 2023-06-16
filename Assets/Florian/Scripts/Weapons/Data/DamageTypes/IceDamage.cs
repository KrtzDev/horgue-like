using UnityEngine;

[CreateAssetMenu(fileName = "new IceDamage", menuName = "ModularWeapon/Data/DamageType/Ice")]
public class IceDamage : DamageType
{
	[SerializeField] private float _freezeChance;
	[SerializeField] private float _freezeDamage;
	[SerializeField] private float _freezeDuration;
	[SerializeField] private float _slowAmount;

	public override void ApplyEffect(Enemy enemy)
	{
		if (_freezeChance < Random.Range(1, 100))
			return;

		HealthComponent health = enemy.GetComponent<HealthComponent>();
		enemy.GetComponent<Status>().AddEffect(new DamageOverTime(health,_freezeDamage,_freezeDuration,0f));
		enemy.GetComponent<Status>().AddEffect(new Slow(enemy, _slowAmount, _freezeDuration));
	}
}
