using UnityEngine;

[CreateAssetMenu(fileName = "newBasicEnemyData", menuName = "Data/Enemy Data/Base Data")]
public class BasicEnemyData : ScriptableObject
{
    [Header("General")]
    public int _maxHealth = 100;
    public int _damagePerHit = 100;
    public float _attackSpeed = 1;
    public int _givenXP = 1;
    public int _moveSpeed = 1;
    public int _armor = 1;
    public int _elementalResistance = 1;
    public int _technicalResistance = 1;

    [Header("Movement")]
    public float _attackRange;
    public float _retreatRange;
    public float _followTime;
    public float _maxJumpAttackRange;
    public float _minJumpAttackRange;
    public float _jumpAttackChance;
    public float _jumpAttackCooldown;
    public float _jumpPrepTime;
    public float _jumpTime;
    public float _jumpForce;
    public AnimationCurve HeightCurve;
}
