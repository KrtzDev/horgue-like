using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "new Weapon", menuName = "ModularWeapon/Weapon")]
public class Weapon : ScriptableObject
{
	private const float SLOWEST_POSSIBLE_ATTACKSPEED = 10;

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

	[SerializeField]
	private LayerMask _enemyLayer;
	[SerializeField]
	private LayerMask _groundLayer;

	public Projectile Projectile { get; private set; }
	public Transform OwningTransform { get; private set; }

	private WeaponSkeleton _currentWeaponPrefab;
	private Transform _weaponTransform;

	private ObjectPool _projectilePool;

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
		Debug.Log("Inizialized " + _weaponPrefab);
		OwningTransform = owningTransform;
		_currentWeaponPrefab = Instantiate(_weaponPrefab, owningTransform);
		_weaponTransform = _currentWeaponPrefab.transform;

		ApplyStats(CalculateWeaponStats(this));

		_projectilePool = ObjectPool.CreatePool(Projectile, 100, null);
	}

	public WeaponStats CalculateWeaponStats(Weapon weapon)
	{
		WeaponStats weaponStats = new WeaponStats();

		weaponStats.damage = CalculateDamage(weapon);
		weaponStats.attackspeed = CalculateAttackSpeed(weapon);
		weaponStats.cooldown = CalculateCooldown(weapon);
		weaponStats.projectileSize = CalculateProjectileSize(weapon);
		weaponStats.critChance = CalculateCritChance(weapon);
		weaponStats.range = CalculatefinalRange(weapon);

		weaponStats.capacity = weapon._magazine.capacity;
		weaponStats.attackPattern = weapon._barrel.attackPattern;

		return weaponStats;
	}

	public WeaponStats CalculatePotentialStats(WeaponPart hoveredPart)
	{
		Grip grip = (hoveredPart is Grip) ? (Grip)hoveredPart : _grip;
		Barrel barrel = (hoveredPart is Barrel) ? (Barrel)hoveredPart : _barrel;
		Magazine magazine = (hoveredPart is Magazine) ? (Magazine)hoveredPart : _magazine;
		Ammunition ammunition = (hoveredPart is Ammunition) ? (Ammunition)hoveredPart : _ammunition;
		TriggerMechanism triggerMechanism = (hoveredPart is TriggerMechanism) ? (TriggerMechanism)hoveredPart : _triggerMechanism;
		Sight sight = (hoveredPart is Sight) ? (Sight)hoveredPart : _sight;

		Weapon weapon = CreateInstance<Weapon>();

		weapon._grip = grip;
		weapon._barrel = barrel;
		weapon._magazine = magazine;
		weapon._ammunition = ammunition;
		weapon._triggerMechanism = triggerMechanism;
		weapon._sight = sight;

		return CalculateWeaponStats(weapon);
	}

	private void ApplyStats(WeaponStats weaponStats)
	{
		Projectile = _ammunition.projectilePrefab;

		Projectile.finalBaseDamage = weaponStats.damage;
		Projectile.finalAttackSpeed = weaponStats.attackspeed;
		Projectile.finalCooldown = weaponStats.cooldown;
		Projectile.finalProjectileSize = weaponStats.projectileSize;
		Projectile.finalCritChance = weaponStats.critChance;
		Projectile.finalRange = weaponStats.range;

		_shotDelay = Projectile.finalAttackSpeed > 0 ? 1 / Projectile.finalAttackSpeed : SLOWEST_POSSIBLE_ATTACKSPEED;

		_capacity = weaponStats.capacity;
		Projectile.attackPattern = weaponStats.attackPattern;

		Projectile.spawnTransform = _currentWeaponPrefab.ProjectileSpawnPosition;
	}

	private float CalculateDamage(Weapon weapon)
	{
		float totalDamage = 0;
		int partCount = 0;

		if (weapon._triggerMechanism)
		{
			totalDamage += weapon._triggerMechanism.baseDamage;
			partCount++;
		}
		if (weapon._barrel)
		{
			totalDamage += weapon._barrel.baseDamage;
			partCount++;
		}
		if (weapon._ammunition)
		{
			totalDamage += weapon._ammunition.baseDamage;
			partCount++;
		}

		return partCount > 0 ? totalDamage / partCount : -1;
	}

	private float CalculateAttackSpeed(Weapon weapon)
	{
		float totalAttackSpeed = 0;
		int partCount = 0;

		if (weapon._grip)
		{
			totalAttackSpeed += weapon._grip.attackSpeed;
			partCount++;
		}
		if (weapon._barrel)
		{
			totalAttackSpeed += weapon._barrel.attackSpeed;
			partCount++;
		}
		if (weapon._magazine)
		{
			totalAttackSpeed += weapon._magazine.attackSpeed;
			partCount++;
		}
		if (weapon._ammunition)
		{
			totalAttackSpeed += weapon._ammunition.attackSpeed;
			partCount++;
		}
		if (weapon._triggerMechanism)
		{
			totalAttackSpeed += weapon._triggerMechanism.attackSpeed;
			partCount++;
		}
		if (weapon._sight)
		{
			totalAttackSpeed += weapon._sight.attackSpeed;
			partCount++;
		}
		return partCount > 0 ? totalAttackSpeed / partCount : -1;
	}

	private float CalculateCooldown(Weapon weapon)
	{
		float totalCooldown = 0;
		int partCount = 0;

		if (weapon._grip)
		{
			totalCooldown += weapon._grip.cooldown;
			partCount++;
		}
		if (weapon._barrel)
		{
			totalCooldown += weapon._barrel.cooldown;
			partCount++;
		}
		if (weapon._magazine)
		{
			totalCooldown += weapon._magazine.cooldown;
			partCount++;
		}
		if (weapon._ammunition)
		{
			totalCooldown += weapon._ammunition.cooldown;
			partCount++;
		}
		if (weapon._triggerMechanism)
		{
			totalCooldown += weapon._triggerMechanism.cooldown;
			partCount++;
		}

		return partCount > 0 ? totalCooldown / partCount : -1;
	}

	private float CalculateProjectileSize(Weapon weapon)
	{
		float totalProjectileSize = 0;
		int partCount = 0;

		if (weapon._barrel)
		{
			totalProjectileSize += weapon._barrel.projectileSize;
			partCount++;
		}
		if (weapon._magazine)
		{
			totalProjectileSize += weapon._magazine.projectileSize;
			partCount++;
		}
		if (weapon._ammunition)
		{
			totalProjectileSize += weapon._ammunition.projectileSize;
			partCount++;
		}
		return partCount > 0 ? totalProjectileSize / partCount : -1;
	}

	private float CalculateCritChance(Weapon weapon)
	{
		float totalCritChance = 0;
		int partCount = 0;

		if (weapon._grip)
		{
			totalCritChance += weapon._grip.critChance;
			partCount++;
		}
		if (weapon._barrel)
		{
			totalCritChance += weapon._barrel.critChance;
			partCount++;
		}
		if (weapon._ammunition)
		{
			totalCritChance += weapon._ammunition.critChance;
			partCount++;
		}
		if (weapon._triggerMechanism)
		{
			totalCritChance += weapon._triggerMechanism.critChance;
			partCount++;
		}
		if (weapon._sight)
		{
			totalCritChance += weapon._sight.critChance;
			partCount++;
		}
		return partCount > 0 ? totalCritChance / partCount : -1;
	}

	private float CalculatefinalRange(Weapon weapon)
	{
		float totalRange = 0;
		int partCount = 0;

		if (weapon._barrel)
		{
			totalRange += weapon._barrel.range;
			partCount++;
		}
		if (weapon._ammunition)
		{
			totalRange += weapon._ammunition.range;
			partCount++;
		}
		if (weapon._triggerMechanism)
		{
			totalRange += weapon._triggerMechanism.range;
			partCount++;
		}
		if (weapon._sight)
		{
			totalRange += weapon._sight.range;
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
			_isReloading = true;
			Reload();
		}
		return false;
	}

	private void Shoot()
	{
		if (Projectile.finalAttackSpeed == 0)
			return;

		_shotDelay -= Time.deltaTime;
		if (!RotateTowardsEnemy())
			return;

		if (_shotDelay <= 0)
		{
			_capacity--;
			_shotDelay = 1 / Projectile.finalAttackSpeed;

			_currentWeaponPrefab.MuzzleFlash.Play();

			Projectile spawnedProjectile = (Projectile)_projectilePool.GetObject();
			spawnedProjectile.transform.position = spawnedProjectile.spawnTransform.position;
			spawnedProjectile.transform.rotation = spawnedProjectile.spawnTransform.rotation;
			spawnedProjectile.attackPattern.AttackInPattern(spawnedProjectile);
			spawnedProjectile.gameObject.transform.localScale = Vector3.one * Projectile.finalProjectileSize;
			spawnedProjectile.OnHit += CleanUpProjectile;
			spawnedProjectile.OnLifeTimeEnd += CleanUpProjectile;
		}
	}

	private void CleanUpProjectile(Projectile projectile)
	{
		Debug.Log("Returned projectile");
		_projectilePool.ReturnObjectToPool(projectile);
	}

	private bool RotateTowardsEnemy()
	{
		float currentclosestdistance = Mathf.Infinity;
		Enemy closestEnemy = null;

		Collider[] enemies = Physics.OverlapSphere(_weaponTransform.position, Projectile.finalRange, _enemyLayer);
		foreach (Collider enemy in enemies)
		{
			float distanceToEnemy = Vector3.Distance(_weaponTransform.position, enemy.transform.position);
			if (distanceToEnemy > currentclosestdistance)
				continue;

			Vector3 directionToEnemy = enemy.transform.position + Vector3.up - _weaponTransform.position;
			if (!Physics.Raycast(_weaponTransform.position, directionToEnemy, distanceToEnemy, _groundLayer))
			{
				closestEnemy = enemy.GetComponent<Enemy>();
				currentclosestdistance = distanceToEnemy;
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
		await Task.Delay((int)(Projectile.finalCooldown * 1000));

		_capacity = _magazine.capacity;
		_isReloading = false;
	}
}
