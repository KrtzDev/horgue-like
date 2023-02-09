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
		_weaponTransform = _playerTransform.GetComponent<PlayerCharacter>().WeaponSpawnTransform;
        _currentWeaponPrefab = Instantiate(weaponPrefab, _weaponTransform);

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
        float damage = 0;
        if (_triggerMechanism)
        {
            damage += _triggerMechanism.baseDamage;
        }
        if (_barrel)
        {
            damage += _barrel.baseDamage;
        }
        if (_ammunition)
        {
            damage += _ammunition.baseDamage;
        }
        return damage;
    }

    private float CalculateAttackSpeed()
    {
        float attackSpeed = 0;
        if (_grip)
        {
            attackSpeed += _grip.attackSpeed;
        }
        if (_barrel)
        {
            attackSpeed += _barrel.attackSpeed;
        }
        if (_magazine)
        {
            attackSpeed += _magazine.attackSpeed;
        }
        if (_ammunition)
        {
            attackSpeed += _ammunition.attackSpeed;
        }
        if (_triggerMechanism)
        {
            attackSpeed += _triggerMechanism.attackSpeed;
        }
        if (_sight)
        {
            attackSpeed += _sight.attackSpeed;
        }
        return attackSpeed;
    }

    private float CalculateCooldown()
    {
        float cooldown = 0;
        if (_grip)
        {
            cooldown += _grip.cooldown;
        }
        if (_barrel)
        {
            cooldown += _barrel.cooldown;
        }
        if (_magazine)
        {
            cooldown += _magazine.cooldown;
        }
        if (_ammunition)
        {
            cooldown += _ammunition.cooldown;
        }
        if (_triggerMechanism)
        {
            cooldown += _triggerMechanism.cooldown;
        }
        return cooldown;
    }

    private float CalculateProjectileSize()
    {
        float projectileSize = 0;
        if (_barrel)
        {
            projectileSize += _barrel.projectileSize;
        }
        if (_magazine)
        {
            projectileSize += _magazine.projectileSize;
        }
        if (_ammunition)
        {
            projectileSize += _ammunition.projectileSize;
        }
        return projectileSize;
    }

    private float CalculateCritChance()
    {
        float critChance = 0;
        if (_grip)
        {
            critChance += _grip.critChance;
        }
        if (_barrel)
        {
            critChance += _barrel.critChance;
        }
        if (_ammunition)
        {
            critChance += _ammunition.critChance;
        }
        if (_triggerMechanism)
        {
            critChance += _triggerMechanism.critChance;
        }
        if (_sight)
        {
            critChance += _sight.critChance;
        }
        return critChance;
    }

    private float CalculatefinalRange()
    {
        float range = 0;
        if (_barrel)
        {
            range += _barrel.range;
        }
        if (_ammunition)
        {
            range += _ammunition.range;
        }
        if (_triggerMechanism)
        {
            range += _triggerMechanism.range;
        }
        if (_sight)
        {
            range += _sight.range;
        }
        return range;
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
		Debug.Log(this.name);
        if (_possibleProjectile.finalAttackSpeed == 0) return;
        _shotDelay -= Time.deltaTime;
        if (_shotDelay <= 0)
        {
            if(!RotateTowardsEnemy()) return;
            _capacity--;
            _possibleProjectile.attackPattern.AttackInPattern(_possibleProjectile, _possibleProjectile.spawnPosition);
            _shotDelay = 1 / _possibleProjectile.finalAttackSpeed;
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
                if (!Physics.Raycast(_weaponTransform.position, (enemy.transform.position - _weaponTransform.position), distanceToEnemy, _groundLayer))
                {
                    closestEnemy = enemy.GetComponent<Enemy>();
                    currentclosestdistance = distanceToEnemy;
                }
            }
        }
        if (closestEnemy)
        {
            Vector3 direction = closestEnemy.transform.position - _weaponTransform.position;
			_weaponTransform.transform.rotation = Quaternion.LookRotation(direction);
			return true;
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
