using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new Weapon", menuName = "ModularWeapon/Weapon")]
public class Weapon : ScriptableObject
{
    [Header("WeaponParts")]
    [SerializeField]
    private Grip grip;
    [SerializeField]
    private List<Barrel> _barrels = new List<Barrel>();
    [SerializeField]
    private List<Magazine> _magazines = new List<Magazine>();
    [SerializeField]
    private List<Ammunition> _ammunitions = new List<Ammunition>();
    [SerializeField]
    private TriggerMechanism _triggerMechanism;
    [SerializeField]
    private Sight _sight;

    private float _baseDamage;
    private float _attackSpeed;
    private float _cooldown;
    private float _projectileSize;
    private float _critChance;
    private float _range;

    private AttackPattern _attackPattern;

}
