using UnityEngine;

[CreateAssetMenu(fileName = "new Grip", menuName = "ModularWeapon/WeaponParts/Grip")]
public class Grip : ScriptableObject
{
    [Header("Stats")]
    public float attackSpeed;
    public float cooldown;
    public float critChance;

    [Header("Attachements")]
    public uint maxBarrels;
    public uint maxMagazines;
}
