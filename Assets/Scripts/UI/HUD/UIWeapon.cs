using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIWeapon : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _weapon1Bullets;
    [SerializeField] private TextMeshProUGUI _weapon2Bullets;

    private int _maxCapacityWeapon1;
    private int _maxCapacityWeapon2;

    private WeaponHolster _weaponHolster;

    private void Start() 
    {
        _weaponHolster = FindObjectOfType<WeaponHolster>();
        _maxCapacityWeapon1 = _weaponHolster.weapons[0]._capacity;
        _maxCapacityWeapon2 = _weaponHolster.weapons[1]._capacity;

        _weapon1Bullets.text = _weaponHolster.weapons[0]._capacity + " / " + _maxCapacityWeapon1;
        _weapon2Bullets.text = _weaponHolster.weapons[1]._capacity + " / " + _maxCapacityWeapon2;
    }

    private void Update() 
    {
        _weapon1Bullets.text = _weaponHolster.weapons[0]._capacity + " / " + _maxCapacityWeapon1;
        _weapon2Bullets.text = _weaponHolster.weapons[1]._capacity + " / " + _maxCapacityWeapon2;    
    }
}
