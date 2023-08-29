using UnityEngine;

public class WeaponPart : ScriptableObject
{
	[field: SerializeField]
	public Sprite WeaponPartUISprite { get; private set; }

	[field: SerializeField]
	public GameObject WeaponPartPrefab { get; private set; }

	[Header("Stats")]
	public float baseDamage;
	public float attackSpeed;
	public float cooldown;
	public float projectileSize;
	public float critChance;
	public float critDamage;
	public float range;

	[Header("Level Obainted")]
	public int levelObtained = 1;

	[Space(22)]
	[SerializeField]
	public float cost;
}
