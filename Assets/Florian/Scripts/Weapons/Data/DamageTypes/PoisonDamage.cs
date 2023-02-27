using UnityEngine;

[CreateAssetMenu(fileName = "new PoisonDamage", menuName = "ModularWeapon/Data/DamageType/Poison")]
public class PoisonDamage : DamageType
{
    public override void DoDamage()
    {
        Debug.Log("Poison");
    }
}
