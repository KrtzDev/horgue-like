using UnityEngine;

[CreateAssetMenu(fileName = "new Barrel", menuName = "ModularWeapon/WeaponParts/Ammunition")]
public class Ammunition : WeaponPart
{
    public Projectile projectilePrefab;
    public StatusEffectSO statusEffect;
}
