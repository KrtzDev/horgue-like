using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "new Weapon", menuName = "ModularWeapon/Weapon")]
public class Weapon : ScriptableObject
{
    [Header("Visuals")]
    [SerializeField]
    private WeaponSkeleton weaponPrefab;

    [Header("WeaponParts")]
    [SerializeField]
    private Grip _grip;
    [SerializeField]
    private Barrel _barrel;
    [SerializeField]
    private Magazine _magazine;
    [SerializeField]
    private Ammunition _ammunition;
    [SerializeField]
    private TriggerMechanism _triggerMechanism;
    [SerializeField]
    private Sight _sight;

    private int _capacity;

    private Projectile _possibleProjectile;

    float _shotDelay;

    private bool _isReloading;
    private WeaponSkeleton _currentWeaponPrefab;
    private Transform _playerTransform;
	private Transform _weaponTransform;
    [SerializeField]
    private LayerMask _enemyLayer;
    [SerializeField]
    private LayerMask _groundLayer;

    public void Initialize(Transform owningTransform)
    {
        _playerTransform = owningTransform;
        _currentWeaponPrefab = Instantiate(weaponPrefab, owningTransform);
		_weaponTransform = _currentWeaponPrefab.transform;

        _possibleProjectile = _ammunition.projectilePrefab;
        _possibleProjectile.finalBaseDamage = CalculateDamage();
        _possibleProjectile.finalAttackSpeed = CalculateAttackSpeed();
        _possibleProjectile.finalCooldown = CalculateCooldown();
        _possibleProjectile.finalProjectileSize = CalculateProjectileSize();
        _possibleProjectile.finalCritChance = CalculateCritChance();
        _possibleProjectile.finalRange = CalculatefinalRange();

        _capacity = _magazine.capacity;

        _possibleProjectile.attackPattern = _barrel.attackPattern;
        _possibleProjectile.spawnPosition = _currentWeaponPrefab.ProjectileSpawnPosition;

        if (_possibleProjectile.finalAttackSpeed != 0)
        {
            _shotDelay = 1 / _possibleProjectile.finalAttackSpeed;
        }
    }

    private float CalculateDamage()
    {
        float totalDamage = 0;
		int partCount = 0;

        if (_triggerMechanism)
        {
            totalDamage += _triggerMechanism.baseDamage;
			partCount++;
        }
        if (_barrel)
        {
            totalDamage += _barrel.baseDamage;
			partCount++;
        }
        if (_ammunition)
        {
            totalDamage += _ammunition.baseDamage;
			partCount++;
        }

		return partCount > 0 ? totalDamage / partCount : -1;
	}

    private float CalculateAttackSpeed()
    {
        float totalAttackSpeed = 0;
		int partCount = 0;

        if (_grip)
        {
            totalAttackSpeed += _grip.attackSpeed;
			partCount++;
        }
        if (_barrel)
        {
            totalAttackSpeed += _barrel.attackSpeed;
			partCount++;
        }
        if (_magazine)
        {
            totalAttackSpeed += _magazine.attackSpeed;
			partCount++;
        }
        if (_ammunition)
        {
            totalAttackSpeed += _ammunition.attackSpeed;
			partCount++;
        }
        if (_triggerMechanism)
        {
            totalAttackSpeed += _triggerMechanism.attackSpeed;
			partCount++;
        }
        if (_sight)
        {
            totalAttackSpeed += _sight.attackSpeed;
			partCount++;
        }
		return partCount > 0 ? totalAttackSpeed / partCount : -1;
	}

    private float CalculateCooldown()
    {
        float totalCooldown = 0;
		int partCount = 0;

        if (_grip)
        {
            totalCooldown += _grip.cooldown;
			partCount++;
        }
        if (_barrel)
        {
            totalCooldown += _barrel.cooldown;
			partCount++;
        }
        if (_magazine)
        {
            totalCooldown += _magazine.cooldown;
			partCount++;
        }
        if (_ammunition)
        {
            totalCooldown += _ammunition.cooldown;
			partCount++;
        }
        if (_triggerMechanism)
        {
            totalCooldown += _triggerMechanism.cooldown;
			partCount++;
        }

		return partCount > 0 ? totalCooldown / partCount : -1;
    }

    private float CalculateProjectileSize()
    {
        float totalProjectileSize = 0;
		int partCount = 0;

        if (_barrel)
        {
            totalProjectileSize += _barrel.projectileSize;
			partCount++;
        }
        if (_magazine)
        {
            totalProjectileSize += _magazine.projectileSize;
			partCount++;
        }
        if (_ammunition)
        {
            totalProjectileSize += _ammunition.projectileSize;
			partCount++;
        }
		return partCount > 0 ? totalProjectileSize / partCount : -1;
	}

    private float CalculateCritChance()
    {
        float totalCritChance = 0;
		int partCount = 0;

        if (_grip)
        {
            totalCritChance += _grip.critChance;
			partCount++;
        }
        if (_barrel)
        {
            totalCritChance += _barrel.critChance;
			partCount++;
        }
        if (_ammunition)
        {
            totalCritChance += _ammunition.critChance;
			partCount++;
        }
        if (_triggerMechanism)
        {
            totalCritChance += _triggerMechanism.critChance;
			partCount++;
        }
        if (_sight)
        {
            totalCritChance += _sight.critChance;
			partCount++;
        }
		return partCount > 0 ? totalCritChance / partCount : -1;
	}

    private float CalculatefinalRange()
    {
        float totalRange = 0;
		int partCount = 0;

        if (_barrel)
        {
            totalRange += _barrel.range;
			partCount++;
        }
        if (_ammunition)
        {
            totalRange += _ammunition.range;
			partCount++;
        }
        if (_triggerMechanism)
        {
            totalRange += _triggerMechanism.range;
			partCount++;
        }
        if (_sight)
        {
            totalRange += _sight.range;
			partCount++;
        }
		return partCount > 0 ? totalRange / partCount : -1;
	}

	public void TryShoot()
	{
		if (CanShoot())
			Shoot();
	}

    private bool CanShoot()
    {
        if (_capacity > 0)
        {
            return true;
        }
        if (!_isReloading)
        {
            Reload();
            _isReloading = true;
        }
        return false;
    }

    private void Shoot()
    {
        if (_possibleProjectile.finalAttackSpeed == 0) return;
        _shotDelay -= Time.deltaTime;
        if(!RotateTowardsEnemy()) return;
        if (_shotDelay <= 0)
        {
			DamageDealer spawnedDamageDealer;

            _capacity--;
            _shotDelay = 1 / _possibleProjectile.finalAttackSpeed;
            spawnedDamageDealer = _possibleProjectile.attackPattern.AttackInPattern(_possibleProjectile, _possibleProjectile.spawnPosition);
			spawnedDamageDealer.gameObject.transform.localScale = Vector3.one * _possibleProjectile.finalProjectileSize;
		}
    }

    private bool RotateTowardsEnemy()
    {
        float currentclosestdistance = Mathf.Infinity;
        Enemy closestEnemy = null;

        Collider[] enemies = Physics.OverlapSphere(_weaponTransform.position, _possibleProjectile.finalRange, _enemyLayer);
        foreach (var enemy in enemies)
        {
            Debug.Log(enemy.name);
            float distanceToEnemy = Vector3.Distance(_weaponTransform.position, enemy.transform.position);
            if (distanceToEnemy < currentclosestdistance)
            {
                if (!Physics.Raycast(_weaponTransform.position, ((enemy.transform.position + Vector3.up) - _weaponTransform.position), distanceToEnemy, _groundLayer))
                {
                    closestEnemy = enemy.GetComponent<Enemy>();
                    currentclosestdistance = distanceToEnemy;
                }
            }
        }
        if (closestEnemy)
        {
            Vector3 direction = (closestEnemy.transform.position + Vector3.up) - _weaponTransform.position;
			Vector3 rotateTowardsDirection = Vector3.RotateTowards(_weaponTransform.forward, direction, 20 * Time.deltaTime, .0f);
			_weaponTransform.transform.rotation = Quaternion.LookRotation(rotateTowardsDirection);
			if ((_weaponTransform.forward - direction.normalized).magnitude <= .1f)
			{
				return true;
			}
        }
		return false;
    }

    private async void Reload()
    {
        await Task.Delay((int)(_possibleProjectile.finalCooldown * 1000));

        _capacity = _magazine.capacity;
        _isReloading = false;
    }
}
