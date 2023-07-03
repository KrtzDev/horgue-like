using UnityEngine;

[CreateAssetMenu(fileName = "new MotionPattern", menuName = "ModularWeapon/Data/MotionPattern")]
public class MotionPattern : ScriptableObject
{
	public bool shouldExplodeOnDeath;
	public bool shouldSnapToGround;
	public bool hasGravity;

	[SerializeField] private float _speed;
	[SerializeField] private float _lifeTime;

	public void BeginMotion(Projectile projectile)
	{
		projectile.OnHit += DisableGravity;
		projectile.OnLifeTimeEnd += DisableGravity;

		projectile.LifeTime = _lifeTime;

		if (shouldSnapToGround)
			SnapToGround(projectile);

		if (hasGravity)
			projectile.GetComponent<Rigidbody>().useGravity = true;

		projectile.GetComponent<Rigidbody>().velocity = projectile.transform.forward * _speed;
	}

	private void DisableGravity(Projectile projectile) 
	{
		if(Application.isPlaying)
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
	}
}