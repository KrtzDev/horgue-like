using UnityEngine;

[CreateAssetMenu(fileName = "new MotionPattern", menuName = "ModularWeapon/Data/MotionPattern")]
public class MotionPattern : ScriptableObject
{
	public bool shouldExplodeOnDeath;
	public bool shouldSnapToGround;

	[SerializeField] private float _speed;
	[SerializeField] private float _lifeTime;
	
	public void BeginMotion(Projectile projectile)
	{
		projectile.LifeTime = _lifeTime;
		projectile.GetComponent<Rigidbody>().velocity = projectile.transform.forward * _speed;
	}

	public void UpdateMotion(Projectile projectile)
	{
		projectile.LifeTime -= Time.deltaTime;
		if (projectile.LifeTime <= 0)
			projectile.OnLifeTimeEnd?.Invoke(projectile);
	}
}