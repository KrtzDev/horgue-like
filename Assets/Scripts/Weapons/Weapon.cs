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
	public Sprite weaponSprite;

	[Header("DefaultWeaponParts")]
	[SerializeField]
	public Grip defaultGrip;
	[SerializeField]
	public Barrel defaultBarrel;
	[SerializeField]
	public Magazine defaultMagazine;
	[SerializeField]
	public Ammunition defaultAmmunition;
	[SerializeField]
	public TriggerMechanism defaultTriggerMechanism;
	[SerializeField]
	public Sight defaultSight;

	[Header("WeaponParts")]
	[SerializeField]
	public Grip grip;
	[SerializeField]
	public Barrel barrel;
	[SerializeField]
	public Magazine magazine;
	[SerializeField]
	public Ammunition ammunition;
	[SerializeField]
	public TriggerMechanism triggerMechanism;
	[SerializeField]
	public Sight sight;



	[SerializeField]
	private LayerMask _enemyLayer;
	[SerializeField]
	private LayerMask _groundLayer;

	private Projectile _projectile;
	public Transform OwningTransform { get; private set; }

	private WeaponSkeleton _currentWeaponPrefab;
	private Pattern _currentPatternPrefab;
	private Transform _weaponTransform;

	private ObjectPool<Projectile> _projectilePool;
	private ObjectPool<HorgueVFX> _vfxPool;

	private float _shotDelay;
	private int _capacity;
	private bool _isReloading;
	private float _reloadTime;



	public void ResetWeaponParts()
	{
		grip = defaultGrip;
		barrel = defaultBarrel;
		magazine = defaultMagazine;
		ammunition = defaultAmmunition;
		triggerMechanism = defaultTriggerMechanism;
		sight = defaultSight;
	}

	public void Initialize(Transform owningTransform)
	{
		Debug.Log("Inizialized " + _weaponPrefab);
		OwningTransform = owningTransform;
		_currentWeaponPrefab = Instantiate(_weaponPrefab, owningTransform);
		_currentPatternPrefab = Instantiate(barrel.attackPattern.GetPattern(), _currentWeaponPrefab.ProjectileSpawnPosition);
		_weaponTransform = _currentWeaponPrefab.transform;

		_reloadTime = CalculateWeaponStats(this).cooldown;
		_projectile = ammunition.projectilePrefab;
		_capacity = CalculateWeaponStats(this).capacity;

		_projectilePool = ObjectPool<Projectile>.CreatePool(_projectile, 100, null);

		if(barrel.motionPattern != null && barrel.motionPattern.explosionVfx != null)
		_vfxPool = ObjectPool<HorgueVFX>.CreatePool(barrel.motionPattern.explosionVfx, 25, null);
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

		weaponStats.capacity = weapon.magazine.capacity;
		weaponStats.attackPattern = weapon.barrel.attackPattern;
		weaponStats.motionPattern = weapon.barrel.motionPattern;
		weaponStats.statusEffect = weapon.ammunition.statusEffect;

		return weaponStats;
	}

	public WeaponStats CalculatePotentialStats(WeaponPart hoveredPart)
	{
		Grip potentialGrip = (hoveredPart is Grip) ? (Grip)hoveredPart : grip;
		Barrel potentialBarrel = (hoveredPart is Barrel) ? (Barrel)hoveredPart : barrel;
		Magazine potentialMagazine = (hoveredPart is Magazine) ? (Magazine)hoveredPart : magazine;
		Ammunition potentialAmmunition = (hoveredPart is Ammunition) ? (Ammunition)hoveredPart : ammunition;
		TriggerMechanism potentialTriggerMechanism = (hoveredPart is TriggerMechanism) ? (TriggerMechanism)hoveredPart : triggerMechanism;
		Sight potentialSight = (hoveredPart is Sight) ? (Sight)hoveredPart : sight;

		Weapon weapon = CreateInstance<Weapon>();

		weapon.grip = potentialGrip;
		weapon.barrel = potentialBarrel;
		weapon.magazine = potentialMagazine;
		weapon.ammunition = potentialAmmunition;
		weapon.triggerMechanism = potentialTriggerMechanism;
		weapon.sight = potentialSight;

		return CalculateWeaponStats(weapon);
	}

	private void ApplyStats(WeaponStats weaponStats, Projectile projectile)
	{
		projectile.finalBaseDamage = weaponStats.damage;
		projectile.finalAttackSpeed = weaponStats.attackspeed;
		projectile.finalCooldown = weaponStats.cooldown;
		projectile.finalProjectileSize = weaponStats.projectileSize;
		projectile.finalCritChance = weaponStats.critChance;
		projectile.finalRange = weaponStats.range;

		_shotDelay = projectile.finalAttackSpeed > 0 ? 1 / projectile.finalAttackSpeed : SLOWEST_POSSIBLE_ATTACKSPEED;

		projectile.attackPattern = weaponStats.attackPattern;
		projectile.motionPattern = weaponStats.motionPattern;
		projectile.statusEffect = weaponStats.statusEffect;
	}

	private float CalculateDamage(Weapon weapon)
	{
		float totalDamage = 0;
		int partCount = 0;

		if (weapon.triggerMechanism)
		{
			totalDamage += weapon.triggerMechanism.baseDamage;
			partCount++;
		}
		if (weapon.barrel)
		{
			totalDamage += weapon.barrel.baseDamage;
			partCount++;
		}
		if (weapon.ammunition)
		{
			totalDamage += weapon.ammunition.baseDamage;
			partCount++;
		}

		return partCount > 0 ? totalDamage / partCount : -1;
	}

	private float CalculateAttackSpeed(Weapon weapon)
	{
		float totalAttackSpeed = 0;
		int partCount = 0;

		if (weapon.grip)
		{
			totalAttackSpeed += weapon.grip.attackSpeed;
			partCount++;
		}
		if (weapon.barrel)
		{
			totalAttackSpeed += weapon.barrel.attackSpeed;
			partCount++;
		}
		if (weapon.magazine)
		{
			totalAttackSpeed += weapon.magazine.attackSpeed;
			partCount++;
		}
		if (weapon.ammunition)
		{
			totalAttackSpeed += weapon.ammunition.attackSpeed;
			partCount++;
		}
		if (weapon.triggerMechanism)
		{
			totalAttackSpeed += weapon.triggerMechanism.attackSpeed;
			partCount++;
		}
		if (weapon.sight)
		{
			totalAttackSpeed += weapon.sight.attackSpeed;
			partCount++;
		}
		return partCount > 0 ? totalAttackSpeed / partCount : -1;
	}

	private float CalculateCooldown(Weapon weapon)
	{
		float totalCooldown = 0;
		int partCount = 0;

		if (weapon.grip)
		{
			totalCooldown += weapon.grip.cooldown;
			partCount++;
		}
		if (weapon.barrel)
		{
			totalCooldown += weapon.barrel.cooldown;
			partCount++;
		}
		if (weapon.magazine)
		{
			totalCooldown += weapon.magazine.cooldown;
			partCount++;
		}
		if (weapon.ammunition)
		{
			totalCooldown += weapon.ammunition.cooldown;
			partCount++;
		}
		if (weapon.triggerMechanism)
		{
			totalCooldown += weapon.triggerMechanism.cooldown;
			partCount++;
		}

		return partCount > 0 ? totalCooldown / partCount : -1;
	}

	private float CalculateProjectileSize(Weapon weapon)
	{
		float totalProjectileSize = 0;
		int partCount = 0;

		if (weapon.barrel)
		{
			totalProjectileSize += weapon.barrel.projectileSize;
			partCount++;
		}
		if (weapon.magazine)
		{
			totalProjectileSize += weapon.magazine.projectileSize;
			partCount++;
		}
		if (weapon.ammunition)
		{
			totalProjectileSize += weapon.ammunition.projectileSize;
			partCount++;
		}
		return partCount > 0 ? totalProjectileSize / partCount : -1;
	}

	private float CalculateCritChance(Weapon weapon)
	{
		float totalCritChance = 0;
		int partCount = 0;

		if (weapon.grip)
		{
			totalCritChance += weapon.grip.critChance;
			partCount++;
		}
		if (weapon.barrel)
		{
			totalCritChance += weapon.barrel.critChance;
			partCount++;
		}
		if (weapon.ammunition)
		{
			totalCritChance += weapon.ammunition.critChance;
			partCount++;
		}
		if (weapon.triggerMechanism)
		{
			totalCritChance += weapon.triggerMechanism.critChance;
			partCount++;
		}
		if (weapon.sight)
		{
			totalCritChance += weapon.sight.critChance;
			partCount++;
		}
		return partCount > 0 ? totalCritChance / partCount : -1;
	}

	private float CalculatefinalRange(Weapon weapon)
	{
		float totalRange = 0;
		int partCount = 0;

		if (weapon.barrel)
		{
			totalRange += weapon.barrel.range;
			partCount++;
		}
		if (weapon.ammunition)
		{
			totalRange += weapon.ammunition.range;
			partCount++;
		}
		if (weapon.triggerMechanism)
		{
			totalRange += weapon.triggerMechanism.range;
			partCount++;
		}
		if (weapon.sight)
		{
			totalRange += weapon.sight.range;
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
		WeaponStats weaponStats = CalculateWeaponStats(this);

		if (weaponStats.attackspeed == 0)
			return;

		if (!RotateTowardsEnemy(weaponStats.range))
			return;

		_shotDelay -= Time.deltaTime;

		if (_shotDelay <= 0)
		{
			Projectile[] projectiles = weaponStats.attackPattern.SpawnProjectiles(_capacity, _projectilePool, _currentPatternPrefab);
			for (int i = 0; i < projectiles.Length; i++)
			{
				Projectile projectile = projectiles[i];
				ApplyStats(weaponStats, projectile);

				projectile.gameObject.transform.localScale = Vector3.one * projectile.finalProjectileSize;

				projectile.TargetedEnemy = TargetedEnemy;

				projectile.motionPattern = weaponStats.motionPattern;
				projectile.motionPattern.explosionVfxPool = _vfxPool;
				projectile.motionPattern.BeginMotion(projectile);

				projectile.OnHit += CleanUpProjectile;
				projectile.OnLifeTimeEnd += CleanUpProjectile;

				_capacity--;
			}

			_shotDelay = 1 / weaponStats.attackspeed;

			_currentWeaponPrefab.MuzzleFlash.Play();
		}
	}

	private void CleanUpProjectile(Projectile projectile)
	{
		if (projectile.motionPattern.shouldExplodeOnDeath)
			projectile.DoExplosion();

		_projectilePool.ReturnObjectToPool(projectile);
	}

	private Enemy TargetedEnemy { get; set; }

	private bool RotateTowardsEnemy(float range)
	{
		float currentclosestdistance = Mathf.Infinity;
		AI_Agent closestEnemy = null;

		Collider[] enemies = Physics.OverlapSphere(_weaponTransform.position, range, _enemyLayer);
		foreach (Collider enemy in enemies)
		{
			float distanceToEnemy = Vector3.Distance(_weaponTransform.position, enemy.transform.position);
			if (distanceToEnemy > currentclosestdistance)
				continue;

			Vector3 directionToEnemy = enemy.transform.position + Vector3.up - _weaponTransform.position;
			if (!Physics.Raycast(_weaponTransform.position, directionToEnemy, distanceToEnemy, _groundLayer))
			{
				closestEnemy = enemy.GetComponent<AI_Agent>();
				currentclosestdistance = distanceToEnemy;
			}
		}

		if (closestEnemy != null)
		{
			TargetedEnemy = closestEnemy;
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
		await Task.Delay((int)(_reloadTime * 1000));

		_capacity = magazine.capacity;
		_isReloading = false;
	}
}
