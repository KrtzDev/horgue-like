using UnityEngine;

[CreateAssetMenu(fileName = "new MotionPattern", menuName = "ModularWeapon/Data/MotionPattern")]
public class MotionPattern : ScriptableObject
{
	public bool shouldExplodeOnDeath;
	[DrawIf(nameof(shouldExplodeOnDeath),true)]
	public float explosionRange;
	[DrawIf(nameof(shouldExplodeOnDeath),true)]
	public HorgueVFX explosionVfx;
	[HideInInspector] public ObjectPool<HorgueVFX> explosionVfxPool;

	[SerializeField] private bool _shouldSnapToGround;

	[SerializeField] private bool _hasGravity;

	[SerializeField] private bool _isHoming;
	[DrawIf(nameof(_isHoming), true)]
	[SerializeField] private float _homingRadius;
	[DrawIf(nameof(_isHoming), true)]
	[SerializeField] private float _homingStrength;

	[SerializeField] private bool _isSineWave;

	[Header("General")]
	[SerializeField] private float _speed;
	[SerializeField] private float _lifeTime;

	[SerializeField] private LayerMask _enemyLayerMask;

	public void BeginMotion(Projectile projectile)
	{
		projectile.OnHit += DisableGravity;
		projectile.OnLifeTimeEnd += DisableGravity;

		projectile.LifeTime = _lifeTime;

		if (_shouldSnapToGround)
			SnapToGround(projectile);

		if (_hasGravity)
			projectile.GetComponent<Rigidbody>().useGravity = true;

		projectile.GetComponent<Rigidbody>().velocity = projectile.transform.forward * _speed;
	}

	private void DisableGravity(Projectile projectile)
	{
		if (Application.isPlaying)
			projectile.GetComponent<Rigidbody>().useGravity = false;
	}

	private void SnapToGround(Projectile projectile)
	{
		RaycastHit hit;
		if (Physics.Raycast(projectile.transform.position, Vector3.down, out hit, 1000))
		{
			projectile.transform.position = hit.point + Vector3.up * (projectile.finalProjectileSize +.1f);
		}
	}

	public void UpdateMotion(Projectile projectile)
	{
		projectile.LifeTime -= Time.deltaTime;
		if (projectile.LifeTime <= 0)
			projectile.OnLifeTimeEnd?.Invoke(projectile);

		CheckHoming(projectile);
	}

	private void CheckHoming(Projectile projectile)
	{
		if (!_isHoming)
			return;

		if (projectile.TargetedEnemy.isActiveAndEnabled)
		{
			Vector3 direction = (projectile.TargetedEnemy.transform.position + Vector3.up) - projectile.transform.position;
			Vector3 rotateTowardsDirection = Vector3.RotateTowards(projectile.transform.forward, direction, _homingStrength * Time.deltaTime, .0f);
			projectile.transform.rotation = Quaternion.LookRotation(rotateTowardsDirection);
			projectile.GetComponent<Rigidbody>().velocity = projectile.transform.forward * projectile.GetComponent<Rigidbody>().velocity.magnitude;
		}
		else
		{
			float smallestAngle = float.MaxValue;

			Collider[] enemies = Physics.OverlapSphere(projectile.transform.position, _homingRadius, _enemyLayerMask);
			for (int i = 0; i < enemies.Length; i++)
			{
				Vector3 directionToEnemy = (enemies[i].transform.position + Vector3.up) - projectile.transform.position;
				float angle = Vector3.Angle(directionToEnemy, projectile.transform.forward);
				if (angle < smallestAngle)
				{
					smallestAngle = angle;
					projectile.TargetedEnemy = enemies[i].GetComponent<Enemy>();
				}
			}
		}

	}
}