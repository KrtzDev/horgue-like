using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ModularWeapon/Data/DamageType/Fire")]
public class FireDamage : DamageType
{
    public override void DoDamage()
    {
        Debug.Log("FireDamage");
    }
}
