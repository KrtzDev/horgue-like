using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ModularWeapon/Weapon")]
public class Weapon : ScriptableObject
{
    [SerializeField]
    private List<GameObject> weaponModulePrefabs;
    [SerializeField]
    private WeaponBehaviour weaponType;

    private void InitalizeWeapon()
    {

    }

    public void DoAttack()
    {

            weaponType.Attack();
        
    }
}
