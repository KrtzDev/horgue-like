using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHolster : MonoBehaviour
{
    [SerializeField]
    private List<Weapon> weapons = new List<Weapon>();

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            foreach (var weapon in weapons)
            {
                weapon.DoAttack();
            }
        }
    }
}
