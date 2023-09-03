using UnityEngine;

[CreateAssetMenu(fileName = "new Magazine", menuName = "ModularWeapon/WeaponParts/Magazine")]
public class Magazine : WeaponPart
{
    public int capacity;

    [Header("Attachements")]
    public int maxAmmoSlots;
}
