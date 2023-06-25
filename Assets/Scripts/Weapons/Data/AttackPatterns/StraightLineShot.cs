using UnityEngine;

[CreateAssetMenu(fileName = "new StraightLineShot", menuName = "ModularWeapon/Data/AttackPattern/StraightLineShot")]
public class StraightLineShot : AttackPattern
{
	[SerializeField] private string _patternName;

	[SerializeField] private bool _randomSpawnPositions;
	[DrawIf(nameof(_randomSpawnPositions), true)]
	[SerializeField] private float _randomPositionRadiusFromPlayer;
	//Todo: Adjust DrawIfAttribute to Handle (not)drawing of Collections
	[SerializeField] private Pattern _pattern;

	[SerializeField] private int _maxSimultaniousProjectiles;



	public override Pattern GetPattern()
	{
		return _pattern;
	}

	public override string PatternName()
	{
		return _patternName;
	}

	public override Projectile[] SpawnProjectiles(WeaponStats weaponStats, ObjectPool<Projectile> projectilePool, Pattern spawnedPattern)
    {
		_pattern = spawnedPattern;

		Projectile[] spawnedProjectiles = new Projectile[Mathf.Min(_pattern.positions.Length, weaponStats.capacity)];

		for (int i = 0; i < spawnedProjectiles.Length; i++)
		{
			Projectile spawnedProjectile = projectilePool.GetObject();
			spawnedProjectile.transform.position = _pattern.positions[i].transform.position;
			spawnedProjectile.transform.rotation = _pattern.positions[i].rotation;

			spawnedProjectiles[i] = spawnedProjectile;
		}

		return spawnedProjectiles;

		//projectile.GetComponent<Rigidbody>().velocity = projectile.transform.forward * 18f;
    }

}
