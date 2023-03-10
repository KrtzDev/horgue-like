using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "new Weapon", menuName = "ModularWeapon/Weapon")]
public class Weapon : ScriptableObject
{
    [Header("Visuals")]
    [SerializeField]
    private WeaponSkeleton _weaponPrefab;
	[SerializeField]
	public Sprite _weaponSprite;

	[Header("DefaultWeaponParts")]
	[SerializeField]
	public Grip _defaultGrip;
	[SerializeField]
	public Barrel _defaultBarrel;
	[SerializeField]
	public Magazine _defaultMagazine;
	[SerializeField]
	public Ammunition _defaultAmmunition;
	[SerializeField]
	public TriggerMechanism _defaultTriggerMechanism;
	[SerializeField]
	public Sight _defaultSight;

	[Header("WeaponParts")]
    [SerializeField]
    public Grip _grip;
    [SerializeField]
    public Barrel _barrel;
    [SerializeField]
    public Magazine _magazine;
    [SerializeField]
    public Ammunition _ammunition;
    [SerializeField]
    public TriggerMechanism _triggerMechanism;
    [SerializeField]
    public Sight _sight;

    public Projectile PossibleProjectile { get; private set; }
	public Transform OwningTransform { get; private set; }

    private WeaponSkeleton _currentWeaponPrefab;
	private Transform _weaponTransform;

    [SerializeField]
    private LayerMask _enemyLayer;
    [SerializeField]
    private LayerMask _groundLayer;

    private float _shotDelay;
    private int _capacity;
    private bool _isReloading;

	public void ResetWeaponParts()
	{
		_grip = _defaultGrip; 
		_barrel = _defaultBarrel;
		_magazine = _defaultMagazine;
		_ammunition = _defaultAmmunition;
		_triggerMechanism = _defaultTriggerMechanism;
		_sight = _defaultSight;
	}

    public void Initialize(Transform owningTransform)
    {
		OwningTransform = owningTransform;
        _currentWeaponPrefab = Instantiate(_weaponPrefab, owningTransform);
		_weaponTransform = _currentWeaponPrefab.transform;

        PossibleProjectile = _ammunition.projectilePrefab;
        PossibleProjectile.finalBaseDamage = CalculateDamage();
        PossibleProjectile.finalAttackSpeed = CalculateAttackSpeed();
        PossibleProjectile.finalCooldown = CalculateCooldown();
        PossibleProjectile.finalProjectileSize = CalculateProjectileSize();
        PossibleProjectile.finalCritChance = CalculateCritChance();
        PossibleProjectile.finalRange = CalculatefinalRange();

        _capacity = _magazine.capacity;

        PossibleProjectile.attackPattern = _barrel.attackPattern;
        PossibleProjectile.spawnPosition = _currentWeaponPrefab.ProjectileSpawnPosition;

        if (PossibleProjectile.finalAttackSpeed != 0)
        {
            _shotDelay = 1 / PossibleProjectile.finalAttackSpeed;
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
        if (PossibleProjectile.finalAttackSpeed == 0) return;
        _shotDelay -= Time.deltaTime;
        if(!RotateTowardsEnemy()) return;
        if (_shotDelay <= 0)
        {
			DamageDealer spawnedDamageDealer;

            _capacity--;
            _shotDelay = 1 / PossibleProjectile.finalAttackSpeed;

            _currentWeaponPrefab.MuzzleFlash.Play();

            spawnedDamageDealer = PossibleProjectile.attackPattern.AttackInPattern(PossibleProjectile, PossibleProjectile.spawnPosition);
			spawnedDamageDealer.gameObject.transform.localScale = Vector3.one * PossibleProjectile.finalProjectileSize;
		}
    }

    private bool RotateTowardsEnemy()
    {
        float currentclosestdistance = Mathf.Infinity;
        Enemy closestEnemy = null;

        Collider[] enemies = Physics.OverlapSphere(_weaponTransform.position, PossibleProjectile.finalRange, _enemyLayer);
        foreach (var enemy in enemies)
        {
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
        await Task.Delay((int)(PossibleProjectile.finalCooldown * 1000));

        _capacity = _magazine.capacity;
        _isReloading = false;
    }
}
