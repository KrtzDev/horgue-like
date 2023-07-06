using UnityEngine;

public class Pattern : MonoBehaviour
{
	[SerializeField] private ManiulationVector _modifyPositions;
	[SerializeField] private ManiulationVector _modifyRotations;

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

			if (_modifyRotations.vector.sqrMagnitude > 0 || _modifyRotations.radius.max > 0)
			{
				float randomX = 0;
				float randomY = 0;
				float randomZ = 0;

				if (_modifyRotations.manipulationMode == ManipulationMode.Randomize)
				{
					randomX = Random.Range(-_modifyRotations.vector.x, _modifyRotations.vector.x);
					randomY = Random.Range(-_modifyRotations.vector.y, _modifyRotations.vector.y);
					randomZ = Random.Range(-_modifyRotations.vector.z, _modifyRotations.vector.z);
				}
				else if (_modifyRotations.manipulationMode == ManipulationMode.Set)
				{
					randomX = _modifyRotations.vector.x;
					randomY = _modifyRotations.vector.y;
					randomZ = _modifyRotations.vector.z;
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

			if (_modifyPositions.vector.sqrMagnitude > 0 || _modifyPositions.radius.max > 0)
			{
				Vector3 randomVector = Vector3.zero;

				if (_modifyPositions.manipulationMode == ManipulationMode.Randomize)
				{
					randomVector.x = Random.Range(-_modifyPositions.vector.x, _modifyPositions.vector.x);
					randomVector.y = Random.Range(-_modifyPositions.vector.y, _modifyPositions.vector.y);
					randomVector.z = Random.Range(-_modifyPositions.vector.z, _modifyPositions.vector.z);
				}
				else if (_modifyPositions.manipulationMode == ManipulationMode.Radius)
				{
					Vector3 randomUnitVector = new Vector3(
							Random.Range(-_modifyPositions.vector.x, _modifyPositions.vector.x),
							Random.Range(-_modifyPositions.vector.y, _modifyPositions.vector.y),
							Random.Range(-_modifyPositions.vector.z, _modifyPositions.vector.z)).normalized;

					randomVector = randomUnitVector * Random.Range(_modifyPositions.radius.min, _modifyPositions.radius.max);
				}

				positions[i] = new Vector3(positions[i].x + randomVector.x, positions[i].y + randomVector.y, positions[i].z  + randomVector.z);
			}
		}

		return positions;
	}
}