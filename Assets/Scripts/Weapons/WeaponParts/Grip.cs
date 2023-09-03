using UnityEngine;

[CreateAssetMenu(fileName = "new Grip", menuName = "ModularWeapon/WeaponParts/Grip")]
public class Grip : WeaponPart
{
    [Header("Attachements")]
    public uint maxBarrels;
    public uint maxMagazines;
}
