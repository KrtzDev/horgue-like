using UnityEngine;

public class Pattern : MonoBehaviour
{
	[SerializeField] private float _randomizeAmount;
	[SerializeField] private Transform[] _transforms;

	public Transform[] GetTransforms() 
	{ 
		return _transforms; 
	}

	public Vector3[] GetPositions()
	{
		Vector3[] positions = new Vector3[_transforms.Length];
		for (int i = 0; i < positions.Length; i++)
		{
			positions[i] = _transforms[i].position;

			float random = Random.Range(-_randomizeAmount, _randomizeAmount);
			positions[i] = new Vector3(positions[i].x + random, _transforms[i].position.y, positions[i].z  + random);
		}

		return positions;
	}
}