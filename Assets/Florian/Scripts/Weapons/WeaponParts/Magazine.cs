using UnityEngine;

[CreateAssetMenu(fileName = "new Magazine", menuName = "ModularWeapon/WeaponParts/Magazine")]
public class Magazine : ScriptableObject
{
    [Header("Stats")]
    public float attackSpeed;
    public float cooldown;
    public float projectileSize;

    public int capacity;

    [Header("Attachements")]
    public int maxAmmoSlots;
}
