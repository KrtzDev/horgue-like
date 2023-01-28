using UnityEngine;

[CreateAssetMenu(fileName = "new TriggerMechanism",menuName = "ModularWeapon/WeaponParts/TriggerMechanism")]
public class TriggerMechanism : ScriptableObject
{
    [Header("Stats")]
    public float baseDamage;
    public float attackSpeed;
    public float cooldown;
    public float critChance;
    public float range;
}
