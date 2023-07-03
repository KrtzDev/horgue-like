using UnityEngine;

public class Pattern : MonoBehaviour
{
	[SerializeField] private ManiulationVector _randomizePositions;
	[SerializeField] private ManiulationVector _randomizeRotations;

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

			if (_randomizeRotations.vector.sqrMagnitude > 0 || _randomizeRotations.radius.max > 0)
			{
				float randomX = 0;
				float randomY = 0;
				float randomZ = 0;

				if (_randomizeRotations.manipulationMode == ManipulationMode.Randomize)
				{
					randomX = Random.Range(-_randomizeRotations.vector.x, _randomizeRotations.vector.x);
					randomY = Random.Range(-_randomizeRotations.vector.y, _randomizeRotations.vector.y);
					randomZ = Random.Range(-_randomizeRotations.vector.z, _randomizeRotations.vector.z);
				}
				else if (_randomizeRotations.manipulationMode == ManipulationMode.Set)
				{
					randomX = _randomizeRotations.vector.x;
					randomY = _randomizeRotations.vector.y;
					randomZ = _randomizeRotations.vector.z;
				}

				rotations[i] = Quaternion.Euler(rotations[i].eulerAngles.x + randomX, rotations[i].eulerAngles.y + randomY, rotations[i].eulerAngles.z  + randomZ);
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

			if (_randomizePositions.vector.sqrMagnitude > 0 || _randomizePositions.radius.max > 0)
			{
				Vector3 randomVector = Vector3.zero;

				if (_randomizePositions.manipulationMode == ManipulationMode.Randomize)
				{
					randomVector.x = Random.Range(-_randomizePositions.vector.x, _randomizePositions.vector.x);
					randomVector.y = Random.Range(-_randomizePositions.vector.y, _randomizePositions.vector.y);
					randomVector.z = Random.Range(-_randomizePositions.vector.z, _randomizePositions.vector.z);
				}
				else if (_randomizePositions.manipulationMode == ManipulationMode.Radius)
				{
					Vector3 randomUnitVector = new Vector3(
							Random.Range(-_randomizePositions.vector.x, _randomizePositions.vector.x),
							Random.Range(-_randomizePositions.vector.y, _randomizePositions.vector.y),
							Random.Range(-_randomizePositions.vector.z, _randomizePositions.vector.z)).normalized;

					randomVector = randomUnitVector * Random.Range(_randomizePositions.radius.min, _randomizePositions.radius.max);
				}

				positions[i] = new Vector3(positions[i].x + randomVector.x, positions[i].y + randomVector.y, positions[i].z  + randomVector.z);
			}
		}

		return positions;
	}
}