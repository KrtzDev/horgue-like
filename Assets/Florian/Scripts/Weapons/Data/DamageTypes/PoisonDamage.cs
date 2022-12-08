using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ModularWeapon/Data/DamageType/Poison")]
public class PoisonDamage : DamageType
{
    public override void DoDamage()
    {
        Debug.Log("Poison");
    }
}
