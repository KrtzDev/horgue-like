using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHolster : MonoBehaviour
{
    [SerializeField]
    private List<Weapon> weapons = new List<Weapon>();

    [SerializeField]
    private float _attackDelay;

    public bool canShoot = true;

    private float _currentAttackDelay;

    private void Awake()
    {
        foreach (var weapon in weapons)
        {
            weapon.weaponHolster = this;
        }
    }

    private void Update()
    {
        _currentAttackDelay -= Time.deltaTime;

        if (_currentAttackDelay <= 0 && canShoot)
        {
            foreach (var weapon in weapons)
            {
                weapon.DoAttack();
            }
            _currentAttackDelay = _attackDelay;
        }
    }

    public void Reload(Weapon weapon, Magazine mag)
    {
        StartCoroutine(ReloadWeapon(weapon,mag));
        mag.isReloading = true;
    }

    IEnumerator ReloadWeapon(Weapon weapon, Magazine mag)
    {
        yield return new WaitForSeconds(weapon.grip.cooldown);
        mag.ammoCount = mag.maxAmmoCount;
        mag.isReloading = false;
    }
}
