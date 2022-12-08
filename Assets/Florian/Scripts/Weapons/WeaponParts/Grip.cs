using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ModularWeapon/WeaponParts/Grip")]
public class Grip : ScriptableObject
{
    [Header("Stats")]
    public float cooldown;

    [Header("Attachements")]
    public uint maxBarrels;
    public uint maxMagazines;
}
