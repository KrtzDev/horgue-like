using UnityEngine;

[CreateAssetMenu(fileName = "new FireDamage", menuName = "ModularWeapon/Data/DamageType/Fire")]
public class FireDamage : DamageType
{
	[SerializeField] private float _additionalDamage;
	[SerializeField] private float _burnChance;
	[SerializeField] private float _burnDuration;
	[SerializeField] private float _burnDamage;
	[SerializeField] private float _propagationChance;

    public override void ApplyEffect(Enemy enemy)
    {
		enemy.GetComponent<HealthComponent>().TakeDamage((int)_additionalDamage);
		enemy.GetComponent<Status>().AddEffect(new DamageOverTime(enemy, _burnDamage, _burnDuration, _propagationChance));
    }
}
