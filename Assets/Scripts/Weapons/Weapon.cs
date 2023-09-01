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
	private ObjectPool<HorgueVFX> _attackPatternExplosiveVFXPool;

	private ObjectPool<HorgueVFX> _statusEffectInitialDamageVFXPool;
	private ObjectPool<HorgueVFX> _statusEffectDamageOverTimeVFXPool;
	private ObjectPool<HorgueVFX> _statusEffectSlowVFXPool;
	private ObjectPool<HorgueVFX> _statusEffectKnockbackVFXPool;
	private ObjectPool<HorgueVFX> _statusEffectPropagationVFXPool;

	private float _shotDelay;
	public int _capacity;
	public bool _isReloading;
	public float _reloadTime;

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
			_attackPatternExplosiveVFXPool = ObjectPool<HorgueVFX>.CreatePool(barrel.motionPattern.explosionVfx, 25, null);
		if (ammunition.statusEffect?.initialDamageVFX != null)
			_statusEffectInitialDamageVFXPool = ObjectPool<HorgueVFX>.CreatePool(ammunition.statusEffect.initialDamageVFX, 100, null);
		if (ammunition.statusEffect?.dotDamageVFX != null)
			_statusEffectDamageOverTimeVFXPool = ObjectPool<HorgueVFX>.CreatePool(ammunition.statusEffect.dotDamageVFX, 100, null);
		if (ammunition.statusEffect?.slowVFX != null)
			_statusEffectSlowVFXPool = ObjectPool<HorgueVFX>.CreatePool(ammunition.statusEffect.slowVFX, 100, null);
		if (ammunition.statusEffect?.knockBackVFX != null)
			_statusEffectKnockbackVFXPool = ObjectPool<HorgueVFX>.CreatePool(ammunition.statusEffect.knockBackVFX, 100, null);
		if (ammunition.statusEffect?.propagationVFX != null)
			_statusEffectPropagationVFXPool = ObjectPool<HorgueVFX>.CreatePool(ammunition.statusEffect.propagationVFX, 100, null);

		_camera = Camera.main;
		_isReloading = false;
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
		WeaponStats weaponStats = new WeaponStats
		{
			damage = CalculateDamage(weapon, GameManager.Instance.damageCalcKind),
			attackspeed = CalculateAttackSpeed(weapon, GameManager.Instance.damageCalcKind),
			cooldown = CalculateCooldown(weapon, GameManager.Instance.damageCalcKind),
			projectileSize = CalculateProjectileSize(weapon, GameManager.Instance.damageCalcKind),
			critChance = CalculateCritChance(weapon, GameManager.Instance.damageCalcKind),
			critDamage = CalculateCritDamage(weapon, GameManager.Instance.damageCalcKind),
			range = CalculatefinalRange(weapon, GameManager.Instance.damageCalcKind),
			dps = CalculateDPS(CalculateDamage(weapon, GameManager.Instance.damageCalcKind), 
				CalculateAttackSpeed(weapon, GameManager.Instance.damageCalcKind), 
				CalculateCooldown(weapon, GameManager.Instance.damageCalcKind),
				CalculateCritChance(weapon, GameManager.Instance.damageCalcKind),
				CalculateCritDamage(weapon, GameManager.Instance.damageCalcKind),
				weapon.magazine.capacity),

			capacity = weapon.magazine.capacity,
            attackPattern = weapon.barrel.attackPattern,
            motionPattern = weapon.barrel.motionPattern,
            statusEffect = weapon.ammunition.statusEffect
        };

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
		projectile.finalCritDamage = weaponStats.critDamage;
		projectile.finalRange = weaponStats.range;

		_shotDelay = 1 / projectile.finalAttackSpeed;

		projectile.attackPattern = weaponStats.attackPattern;
		projectile.motionPattern = weaponStats.motionPattern;
		projectile.statusEffect = weaponStats.statusEffect;
		projectile.statusEffectInitialDamageVFXPool = _statusEffectInitialDamageVFXPool;
		projectile.statusEffectDamageOverTimeVFXPool = _statusEffectDamageOverTimeVFXPool;
		projectile.statusEffectSlowVFXPool = _statusEffectSlowVFXPool;
		projectile.statusEffectKnockbackVFXPool = _statusEffectKnockbackVFXPool;
		projectile.statusEffectPropagationVFXPool = _statusEffectPropagationVFXPool;
	}

	private float CalculateDPS(float damage, float attackSpeed, float cooldown, float critChance, float critDamage, float capacity)
    {
		float critDamagePerShot = damage * (critChance / 100) * critDamage;
		float normalDamagePerShot = damage * (1 - critChance / 100);
		float fullDamagePerShot = critDamagePerShot + normalDamagePerShot;
		float fullDamagePerMagazine = fullDamagePerShot *capacity;
		float cycleTime = capacity / attackSpeed + cooldown;

		float damagePerSecond = fullDamagePerMagazine / cycleTime;

		return damagePerSecond;
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

		if(partCount <= 0) // base
			return _currentWeaponSkeleton.skeletonBaseStats.baseDamage;

		float partsDamage = damageCalcKind == DamageCalcKind.Mean ? totalDamage / partCount : totalDamage;
		// float damage = _currentWeaponSkeleton.skeletonBaseStats.baseDamage + (_currentWeaponSkeleton.skeletonBaseStats.baseDamage * partsDamage * .1f);
		float damage = _currentWeaponSkeleton.skeletonBaseStats.baseDamage + partsDamage;

		if(damage < _currentWeaponSkeleton.skeletonBaseStats.minBaseDamage) // min
			return _currentWeaponSkeleton.skeletonBaseStats.minBaseDamage;

		// no max

		return damage;
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

		if(partCount <= 0) // base
			return _currentWeaponSkeleton.skeletonBaseStats.attackSpeed;

		float partsAttackSpeed = damageCalcKind == DamageCalcKind.Mean ? totalAttackSpeed / partCount : totalAttackSpeed;
		float attackSpeed = _currentWeaponSkeleton.skeletonBaseStats.attackSpeed + partsAttackSpeed;

		if(attackSpeed < _currentWeaponSkeleton.skeletonBaseStats.minAttackSpeed) // min
			return _currentWeaponSkeleton.skeletonBaseStats.minAttackSpeed;

		if(attackSpeed > _currentWeaponSkeleton.skeletonBaseStats.maxAttackSpeed) // max
			return _currentWeaponSkeleton.skeletonBaseStats.maxAttackSpeed;

		return attackSpeed;
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

		if(partCount <= 0) // base
			return _currentWeaponSkeleton.skeletonBaseStats.cooldown;

		float partsCooldown = damageCalcKind == DamageCalcKind.Mean ? totalCooldown / partCount : totalCooldown;
		float cooldown = _currentWeaponSkeleton.skeletonBaseStats.cooldown + partsCooldown;

		if(cooldown < _currentWeaponSkeleton.skeletonBaseStats.minCooldown) // min
			return _currentWeaponSkeleton.skeletonBaseStats.minCooldown;

		if(cooldown > _currentWeaponSkeleton.skeletonBaseStats.maxCooldown) // max
			return _currentWeaponSkeleton.skeletonBaseStats.maxCooldown;

		return cooldown;
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

		if(partCount <= 0) // base
			return _currentWeaponSkeleton.skeletonBaseStats.projectileSize;

		float partsProjectileSize = damageCalcKind == DamageCalcKind.Mean ? totalProjectileSize / partCount : totalProjectileSize;
		float projectileSize = _currentWeaponSkeleton.skeletonBaseStats.projectileSize + partsProjectileSize;

		if(projectileSize < _currentWeaponSkeleton.skeletonBaseStats.minProjectileSize) // min
			return _currentWeaponSkeleton.skeletonBaseStats.minProjectileSize;

		if(projectileSize > _currentWeaponSkeleton.skeletonBaseStats.maxProjectileSize) // max
			return _currentWeaponSkeleton.skeletonBaseStats.maxProjectileSize;		

		return projectileSize;
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

		if(partCount <= 0) // base
			return _currentWeaponSkeleton.skeletonBaseStats.critChance;

		float partsCritChance = damageCalcKind == DamageCalcKind.Mean ? totalCritChance / partCount : totalCritChance;
		float critChance = _currentWeaponSkeleton.skeletonBaseStats.critChance + partsCritChance;

		if(critChance  < _currentWeaponSkeleton.skeletonBaseStats.minCritChance) // min
			return _currentWeaponSkeleton.skeletonBaseStats.minCritChance;

		if(critChance  > _currentWeaponSkeleton.skeletonBaseStats.maxCritChance) // max
			return _currentWeaponSkeleton.skeletonBaseStats.maxCritChance;

		return critChance;
	}

	private float CalculateCritDamage(Weapon weapon, DamageCalcKind damageCalcKind)
	{
		float totalCritDamage = 0;
		int partCount = 0;

		if (weapon.grip)
		{
			totalCritDamage += weapon.grip.critDamage;
			partCount++;
		}
		if (weapon.barrel)
		{
			totalCritDamage += weapon.barrel.critDamage;
			partCount++;
		}
		if (weapon.magazine)
		{
			totalCritDamage += weapon.magazine.critDamage;
			partCount++;
		}
		if (weapon.ammunition)
		{
			totalCritDamage += weapon.ammunition.critDamage;
			partCount++;
		}
		if (weapon.triggerMechanism)
		{
			totalCritDamage += weapon.triggerMechanism.critDamage;
			partCount++;
		}
		if (weapon.sight)
		{
			totalCritDamage += weapon.sight.critDamage;
			partCount++;
		}

		if(partCount <= 0) // base
			return _currentWeaponSkeleton.skeletonBaseStats.critDamage;

		float partsCritDamage = damageCalcKind == DamageCalcKind.Mean ? totalCritDamage / partCount : totalCritDamage;
		float critDamage = _currentWeaponSkeleton.skeletonBaseStats.critDamage + partsCritDamage;

		if(critDamage < _currentWeaponSkeleton.skeletonBaseStats.minCritDamage) // min
			return _currentWeaponSkeleton.skeletonBaseStats.minCritDamage;

		// no max

		return critDamage;
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

		if(partCount <= 0) // base
			return _currentWeaponSkeleton.skeletonBaseStats.range;

		float partsRange = damageCalcKind == DamageCalcKind.Mean ? totalRange / partCount : totalRange;
		float range = _currentWeaponSkeleton.skeletonBaseStats.range + partsRange;

		if(range < _currentWeaponSkeleton.skeletonBaseStats.minRange) // min
			return _currentWeaponSkeleton.skeletonBaseStats.minRange;

		// no max

		return range;
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
				projectile.motionPattern.explosionVfxPool = _attackPatternExplosiveVFXPool;
				projectile.motionPattern.BeginMotion(projectile);

				projectile.OnHit += CleanUpProjectile;
				projectile.OnLifeTimeEnd += CleanUpProjectile;

				_capacity--;
			}

			_shotDelay = 1 / weaponStats.attackspeed;

			_currentWeaponSkeleton.MuzzleFlash.Play();
			AudioManager.Instance.PlaySound("WeaponShot");
			StatsTracker.Instance.shotsFiredLevel++;
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
		// could play first reload sound, then 2nd when reload is finished

		FindObjectOfType<ReloadIndicator>().UseReloadIndicator(weaponSprite, _reloadTime);
		AudioManager.Instance.PlaySound("WeaponReloadEmpty");

		await Task.Delay((int)(_reloadTime * 1000));
		AudioManager.Instance.PlaySound("WeaponReloadFull");

		_capacity = magazine.capacity;
		_isReloading = false;
	}

	public WeaponPart GetWeaponPartOfType(WeaponPart weaponPart)
	{
		if (weaponPart.GetType() == typeof(Grip))
			return grip;
		else if (weaponPart.GetType() == typeof(Barrel))
			return barrel;
		else if (weaponPart.GetType() == typeof(Magazine))
			return magazine;
		else if (weaponPart.GetType() == typeof(Ammunition))
			return ammunition;
		else if (weaponPart.GetType() == typeof(TriggerMechanism))
			return triggerMechanism;
		else if (weaponPart.GetType() == typeof(Sight))
			return sight;

		return null;
	}
}
