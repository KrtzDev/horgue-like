using System.Collections.Generic;
using UnityEngine;

public class WeaponPart : ScriptableObject
{
	[field: SerializeField]
	public Sprite WeaponPartUISprite { get; private set; }

	[field: SerializeField]
	public GameObject WeaponPartPrefab { get; private set; }

	[HideInInspector]
	public bool isSlotted;
}
