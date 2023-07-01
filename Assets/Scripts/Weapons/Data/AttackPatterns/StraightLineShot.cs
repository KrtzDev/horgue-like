using System.ComponentModel;
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

	public override Projectile[] SpawnProjectiles(int capacity, ObjectPool<Projectile> projectilePool, Pattern spawnedPattern)
    {
		Transform[] patternTransforms = spawnedPattern.GetTransforms();
		Vector3[] patternPositions = spawnedPattern.GetPositions();

		Projectile[] spawnedProjectiles = new Projectile[Mathf.Min(_maxSimultaniousProjectiles, capacity)];
		for (int i = 0; i < spawnedProjectiles.Length; i++)
		{
			Projectile spawnedProjectile = projectilePool.GetObject();
			spawnedProjectile.transform.position = patternPositions[i];
			spawnedProjectile.transform.rotation = patternTransforms[i].rotation;

			spawnedProjectiles[i] = spawnedProjectile;
		}

		return spawnedProjectiles;

    }

	#if UNITY_EDITOR
	private void OnValidate()
	{
		_maxSimultaniousProjectiles = Mathf.Min(_maxSimultaniousProjectiles, _pattern.GetTransforms().Length);
	}
	#endif

}
