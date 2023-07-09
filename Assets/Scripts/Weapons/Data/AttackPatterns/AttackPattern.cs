using UnityEngine;

[CreateAssetMenu(fileName = "new AttackPattern", menuName = "ModularWeapon/Data/AttackPattern")]
public class AttackPattern : ScriptableObject
{
	[SerializeField] private string _patternName;

	[SerializeField] private Pattern _pattern;

	[SerializeField] private int _maxSimultaniousProjectiles;



	public Pattern GetPattern()
	{
		return _pattern;
	}

	public string PatternName()
	{
		return _patternName;
	}

	public Projectile[] SpawnProjectiles(int capacity, ObjectPool<Projectile> projectilePool, Pattern spawnedPattern)
	{
		Vector3[] patternPositions = spawnedPattern.GetPositions();
		Quaternion[] patternRotations = spawnedPattern.GetRotations();

		Projectile[] spawnedProjectiles = new Projectile[Mathf.Min(_maxSimultaniousProjectiles, capacity)];
		for (int i = 0; i < spawnedProjectiles.Length; i++)
		{
			Projectile spawnedProjectile = projectilePool.GetObject();
			spawnedProjectile.transform.position = patternPositions[i];
			spawnedProjectile.transform.rotation = patternRotations[i];

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
