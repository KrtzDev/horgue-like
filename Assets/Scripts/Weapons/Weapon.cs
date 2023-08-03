using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "new Weapon", menuName = "ModularWeapon/Weapon")]
public class Weapon : ScriptableObject
{
	[SerializeField]
	private WeaponStats _weaponStats;

	[Header("Visuals")]
	[SerializeField]
	private WeaponSkeleton _weaponSkeleton;
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

	private WeaponSkeleton _currentWeaponSkeleton;
	private Pattern _currentPatternPrefab;
	private Transform _weaponTransform;

	private ObjectPool<Projectile> _projectilePool;
	private ObjectPool<HorgueVFX> _vfxPool;

	private float _shotDelay;
	private int _capacity;
	private bool _isReloading;
	private float _reloadTime;

	private Camera _camera;

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
		Debug.Log("Inizialized " + _weaponSkeleton);
		OwningTransform = owningTransform;
		_currentWeaponSkeleton = Instantiate(_weaponSkeleton, owningTransform);
		_currentPatternPrefab = Instantiate(barrel.attackPattern.GetPattern(), _currentWeaponSkeleton.ProjectileSpawnPosition);
		_weaponTransform = _currentWeaponSkeleton.transform;

		_reloadTime = CalculateWeaponStats(this).cooldown;
		_projectile = ammunition.projectilePrefab;
		_capacity = CalculateWeaponStats(this).capacity;

		if (SceneManager.GetActiveScene().name != "SCENE_Weapon_Crafting")
			_projectilePool = ObjectPool<Projectile>.CreatePool(_projectile, 100, null);

		if (barrel.motionPattern.explosionVfx != null)
			_vfxPool = ObjectPool<HorgueVFX>.CreatePool(barrel.motionPattern.explosionVfx, 25, null);

		_camera = Camera.main;
	}

	public void UpdateAimDirection()
	{
		Vector2 input = InputManager.Instance.CharacterInputActions.Character.Aim.ReadValue<Vector2>();
		AimWeapon(input);
	}

	private void AimWeapon(Vector2 input)
	{
		if (GameManager.Instance.weaponControll == WeaponControllKind.AllAuto)
			return;

		Vector3 direction = new Vector3(input.x, 0, input.y);

		Vector3 cameraForward = _camera.transform.forward;
		Vector3 cameraRight = _camera.transform.right;

		cameraForward.y = 0;
		cameraRight.y = 0;

		cameraForward = cameraForward.normalized;
		cameraRight = cameraRight.normalized;

		Vector3 relativeMoveDirection = direction.z * cameraForward + direction.x * cameraRight;
		_weaponTransform.transform.rotation = Quaternion.LookRotation(relativeMoveDirection);
	}

	public WeaponStats CalculateWeaponStats(Weapon weapon)
	{
		WeaponStats weaponStats = new WeaponStats();

		weaponStats.damage = CalculateDamage(weapon, GameManager.Instance.damageCalcKind);
		weaponStats.attackspeed = CalculateAttackSpeed(weapon, GameManager.Instance.damageCalcKind);
		weaponStats.cooldown = CalculateCooldown(weapon, GameManager.Instance.damageCalcKind);
		weaponStats.projectileSize = CalculateProjectileSize(weapon, GameManager.Instance.damageCalcKind);
		weaponStats.critChance = CalculateCritChance(weapon, GameManager.Instance.damageCalcKind);
		weaponStats.range = CalculatefinalRange(weapon, GameManager.Instance.damageCalcKind);

		weaponStats.capacity = weapon.magazine.capacity;
		weaponStats.attackPattern = weapon.barrel.attackPattern;
		weaponStats.motionPattern = weapon.barrel.motionPattern;
		weaponStats.statusEffect = weapon.ammunition.statusEffect;

		_weaponStats = weaponStats;

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

		_shotDelay = 1 / projectile.finalAttackSpeed;

		projectile.attackPattern = weaponStats.attackPattern;
		projectile.motionPattern = weaponStats.motionPattern;
		projectile.statusEffect = weaponStats.statusEffect;
	}

	private float CalculateDamage(Weapon weapon, DamageCalcKind damageCalcKind)
	{
		float totalDamage = 0;
		int partCount = 0;
		if (weapon.grip)
		{
			totalDamage += weapon.grip.baseDamage;
			partCount++;
		}
		if (weapon.barrel)
		{
			totalDamage += weapon.barrel.baseDamage;
			partCount++;
		}
		if (weapon.magazine)
		{
			totalDamage += weapon.magazine.baseDamage;
			partCount++;
		}
		if (weapon.ammunition)
		{
			totalDamage += weapon.ammunition.baseDamage;
			partCount++;
		}
		if (weapon.triggerMechanism)
		{
			totalDamage += weapon.triggerMechanism.baseDamage;
			partCount++;
		}
		if (weapon.sight)
		{
			totalDamage += weapon.sight.baseDamage;
			partCount++;
		}

		if (partCount <= 0 || totalDamage <= 0)
			return _currentWeaponSkeleton.skeletonBaseStats.baseDamage;

		float partsDamage = damageCalcKind == DamageCalcKind.Mean ? totalDamage / partCount : totalDamage;
		return _currentWeaponSkeleton.skeletonBaseStats.baseDamage + (_currentWeaponSkeleton.skeletonBaseStats.baseDamage * partsDamage * .1f);
	}

	private float CalculateAttackSpeed(Weapon weapon, DamageCalcKind damageCalcKind)
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

		if (partCount <= 0 || totalAttackSpeed <= 0)
			return _currentWeaponSkeleton.skeletonBaseStats.attackSpeed;

		float partsAttackSpeed = damageCalcKind == DamageCalcKind.Mean ? totalAttackSpeed / partCount : totalAttackSpeed;
		return _currentWeaponSkeleton.skeletonBaseStats.attackSpeed + (_currentWeaponSkeleton.skeletonBaseStats.attackSpeed * partsAttackSpeed);
	}

	private float CalculateCooldown(Weapon weapon, DamageCalcKind damageCalcKind)
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
		if (weapon.sight)
		{
			totalCooldown += weapon.sight.cooldown;
			partCount++;
		}

		if (partCount <= 0 || totalCooldown <= 0)
			return _currentWeaponSkeleton.skeletonBaseStats.cooldown;

		float partsCooldown = damageCalcKind == DamageCalcKind.Mean ? totalCooldown / partCount : totalCooldown;
		return _currentWeaponSkeleton.skeletonBaseStats.cooldown + (_currentWeaponSkeleton.skeletonBaseStats.cooldown * partsCooldown);
	}

	private float CalculateProjectileSize(Weapon weapon, DamageCalcKind damageCalcKind)
	{
		float totalProjectileSize = 0;
		int partCount = 0;

		if (weapon.grip)
		{
			totalProjectileSize += weapon.grip.projectileSize;
			partCount++;
		}
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
		if (weapon.triggerMechanism)
		{
			totalProjectileSize += weapon.triggerMechanism.projectileSize;
			partCount++;
		}
		if (weapon.sight)
		{
			totalProjectileSize += weapon.sight.projectileSize;
			partCount++;
		}

		if (partCount <= 0 || totalProjectileSize <= 0)
			return _currentWeaponSkeleton.skeletonBaseStats.projectileSize;

		float partsProjectileSize = damageCalcKind == DamageCalcKind.Mean ? totalProjectileSize / partCount : totalProjectileSize;
		return _currentWeaponSkeleton.skeletonBaseStats.projectileSize +(_currentWeaponSkeleton.skeletonBaseStats.projectileSize * partsProjectileSize);
	}

	private float CalculateCritChance(Weapon weapon, DamageCalcKind damageCalcKind)
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
		if (weapon.magazine)
		{
			totalCritChance += weapon.magazine.critChance;
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

		if (partCount <= 0 || totalCritChance <= 0)
			return _currentWeaponSkeleton.skeletonBaseStats.critChance;

		float partsCritChance = damageCalcKind == DamageCalcKind.Mean ? totalCritChance / partCount : totalCritChance;
		return _currentWeaponSkeleton.skeletonBaseStats.critChance +(_currentWeaponSkeleton.skeletonBaseStats.critChance * partsCritChance);
	}

	private float CalculatefinalRange(Weapon weapon, DamageCalcKind damageCalcKind)
	{
		float totalRange = 0;
		int partCount = 0;

		if (weapon.grip)
		{
			totalRange += weapon.grip.range;
			partCount++;
		}
		if (weapon.barrel)
		{
			totalRange += weapon.barrel.range;
			partCount++;
		}
		if (weapon.magazine)
		{
			totalRange += weapon.magazine.range;
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

		if (partCount <= 0 || totalRange <= 0)
			return _currentWeaponSkeleton.skeletonBaseStats.range;

		float partsRange = damageCalcKind == DamageCalcKind.Mean ? totalRange / partCount : totalRange;
		return _currentWeaponSkeleton.skeletonBaseStats.range + (_currentWeaponSkeleton.skeletonBaseStats.range * partsRange * .1f);
	}

	public void TryShoot()
	{
		_shotDelay -= Time.deltaTime;

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

		if (GameManager.Instance?.weaponControll == WeaponControllKind.AllAuto)
		{
			if (!RotateTowardsEnemy(weaponStats.range))
				return;
		}

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

			_currentWeaponSkeleton.MuzzleFlash.Play();
		}
	}

	private void CleanUpProjectile(Projectile projectile)
	{
		if (projectile.motionPattern.shouldExplodeOnDeath)
			projectile.DoExplosion();

		_projectilePool.ReturnObjectToPool(projectile);
	}

	private AI_Agent_Enemy TargetedEnemy { get; set; }

	private bool RotateTowardsEnemy(float range)
	{
		float currentclosestdistance = Mathf.Infinity;
		AI_Agent_Enemy closestEnemy = null;

		Collider[] enemies = Physics.OverlapSphere(_weaponTransform.position, range, _enemyLayer);
		foreach (Collider enemy in enemies)
		{
			float distanceToEnemy;

			if (!enemy.GetComponent<AI_Agent_Enemy>()._useHeightControl)
            {
				distanceToEnemy = Vector3.Distance(_weaponTransform.position, enemy.transform.position);
            }
			else
            {
				distanceToEnemy = Vector3.Distance(_weaponTransform.position, enemy.transform.position + enemy.GetComponent<AI_Agent_Enemy>()._heightGO.transform.localPosition);
			}

			if (distanceToEnemy > currentclosestdistance)
				continue;

			Vector3 directionToEnemy;

			if (!enemy.GetComponent<AI_Agent_Enemy>()._useHeightControl)
			{
				directionToEnemy = enemy.transform.position + Vector3.up - _weaponTransform.position;
			}
			else
			{
				directionToEnemy = enemy.transform.position + enemy.GetComponent<AI_Agent_Enemy>()._heightGO.transform.localPosition + Vector3.up - _weaponTransform.position;
			}

			if (!Physics.Raycast(_weaponTransform.position, directionToEnemy, distanceToEnemy, _groundLayer))
			{
				closestEnemy = enemy.GetComponent<AI_Agent_Enemy>();
				currentclosestdistance = distanceToEnemy;
			}
		}

		if (closestEnemy != null)
		{
			if (!closestEnemy._useHeightControl)
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
			else
			{
				TargetedEnemy = closestEnemy;
				Vector3 direction = (closestEnemy.transform.position + closestEnemy._heightGO.transform.localPosition + Vector3.up) - _weaponTransform.position;
				Vector3 rotateTowardsDirection = Vector3.RotateTowards(_weaponTransform.forward, direction, 20 * Time.deltaTime, .0f);
				_weaponTransform.transform.rotation = Quaternion.LookRotation(rotateTowardsDirection);
				if ((_weaponTransform.forward - direction.normalized).magnitude <= .1f)
				{
					return true;
				}
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
