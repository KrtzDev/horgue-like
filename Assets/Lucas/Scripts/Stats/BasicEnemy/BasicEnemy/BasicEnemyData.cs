using UnityEngine;

[CreateAssetMenu(fileName = "newBasicEnemyData", menuName = "Data/Enemy Data/Base Data")]
public class BasicEnemyData : ScriptableObject
{
    [Header("General")]
    public int _maxHealth;
    public int _damagePerHit;
    public float _attackSpeed;
    public int _givenXP;
    public float _maxMoveSpeed;
    public float _acceleration;
    public float _armor;
    public float _elementalResistance;
    public float _technicalResistance;
    public float _healthDropChance;

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
