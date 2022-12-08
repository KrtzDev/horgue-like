using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ModularWeapon/Weapon")]
public class Weapon : ScriptableObject
{
    [Header("WeaponParts")]
    [SerializeField]
    public Grip grip;
    [SerializeField]
    private List<Magazine> _magazines = new List<Magazine>();


    [SerializeField]
    private LayerMask _enemyLayer;
    [SerializeField]
    private float _range;
    [SerializeField]
    private float _attackForce;

    public WeaponHolster weaponHolster;

    public void DoAttack()
    {
        foreach (var mag in _magazines)
        {
            if (mag.ammoCount > 0)
            {
                mag.projectile.damageType = mag.damageType;

                float currentclosestdistance = Mathf.Infinity;
                Enemy closestEnemy = null;

                Collider[] enemies = Physics.OverlapSphere(weaponHolster.transform.position, _range, _enemyLayer);
                foreach (var enemy in enemies)
                {
                    float distanceToEnemy = Vector3.Distance(weaponHolster.transform.position, enemy.transform.position);
                    if (distanceToEnemy < currentclosestdistance)
                    {
                        closestEnemy = enemy.GetComponent<Enemy>();
                        currentclosestdistance = distanceToEnemy;
                    }
                }

                if (closestEnemy)
                {
                    ShootAt(closestEnemy, mag);
                }
            }
            if(mag.ammoCount == 0 && !mag.isReloading)
            {
                weaponHolster.Reload(this,mag);
            }


        }
    }

    private void ShootAt(Enemy enemy, Magazine mag)
    {
        mag.ammoCount -= 1;

        Vector3 direction = enemy.transform.position - weaponHolster.transform.position;
        Projectile newProjectile = Instantiate(mag.projectile, weaponHolster.transform.position + Vector3.up + new Vector3(direction.x, 0, direction.z).normalized, Quaternion.identity);
        newProjectile.GetComponent<Rigidbody>().velocity = new Vector3(direction.x, direction.y - 1, direction.z).normalized * _attackForce;
    }
}
