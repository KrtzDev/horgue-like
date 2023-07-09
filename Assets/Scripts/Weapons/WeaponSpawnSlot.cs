using UnityEngine;

[System.Serializable]
public class WeaponSpawnSlot
{
	public Transform spawnTransform;

	[HideInInspector] 
	public bool isOccupied;

	public WeaponSpawnSlot(Transform spawnTransform, bool isOccupied)
	{
		this.spawnTransform = spawnTransform;
		this.isOccupied = isOccupied;
	}
}
