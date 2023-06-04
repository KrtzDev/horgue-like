using UnityEngine;

[CreateAssetMenu(fileName = "new PoisonDamage", menuName = "ModularWeapon/Data/DamageType/Poison")]
public class PoisonDamage : DamageType
{
    public override void ApplyEffect(Enemy enemy)
    {
        Debug.Log("Poison");
    }
}
