using UnityEngine;

[CreateAssetMenu(fileName = "new Sight", menuName = "ModularWeapon/WeaponParts/Sight")]
public class Sight : ScriptableObject
{
    [Header("Stats")]
    public float attackSpeed;
    public float critChance;
    public float range;
}
