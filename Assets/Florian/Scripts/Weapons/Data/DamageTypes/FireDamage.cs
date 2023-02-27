using UnityEngine;

[CreateAssetMenu(fileName = "new FireDamage", menuName = "ModularWeapon/Data/DamageType/Fire")]
public class FireDamage : DamageType
{
    public override void DoDamage()
    {
        Debug.Log("FireDamage");
    }
}
