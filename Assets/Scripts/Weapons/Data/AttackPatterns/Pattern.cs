using UnityEngine;

public class Pattern : MonoBehaviour
{
	[SerializeField] private Vector3 _randomizePositions;
	[SerializeField] private Vector3 _randomizeRotations;
	[SerializeField] private Transform[] _transforms;

	public Transform[] GetTransforms()
	{
		return _transforms;
	}

	public Quaternion[] GetRotations()
	{
		Quaternion[] rotations = new Quaternion[_transforms.Length];
		for (int i = 0; i < rotations.Length; i++)
		{
			rotations[i] = _transforms[i].rotation;

			if (_randomizeRotations.sqrMagnitude > 0)
			{
				float randomX = Random.Range(-_randomizeRotations.x, _randomizeRotations.x);
				float randomY = Random.Range(-_randomizeRotations.y, _randomizeRotations.y);
				float randomZ = Random.Range(-_randomizeRotations.z, _randomizeRotations.z);
				rotations[i] = Quaternion.Euler(rotations[i].x + randomX, rotations[i].y + randomY, rotations[i].z  + randomZ);
			}
		}

		return rotations;
	}

	public Vector3[] GetPositions()
	{
		Vector3[] positions = new Vector3[_transforms.Length];
		for (int i = 0; i < positions.Length; i++)
		{
			positions[i] = _transforms[i].position;

			if (_randomizePositions.sqrMagnitude > 0)
			{
				float randomX = Random.Range(-_randomizePositions.x, _randomizePositions.x);
				float randomY = Random.Range(-_randomizePositions.y, _randomizePositions.y);
				float randomZ = Random.Range(-_randomizePositions.z, _randomizePositions.z);
				positions[i] = new Vector3(positions[i].x + randomX, positions[i].y + randomY, positions[i].z  + randomZ);
			}
		}

		return positions;
	}
}