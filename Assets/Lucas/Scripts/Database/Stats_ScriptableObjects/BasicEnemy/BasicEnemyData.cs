using UnityEngine;

[CreateAssetMenu(fileName = "newBasicEnemyData", menuName = "Data/Enemy Data/Base Data")]
public class BasicEnemyData : ScriptableObject
{
    public int _maxHealth = 100;
    public int _damagePerHit = 100;
    public int _attackSpeed = 1;
    public int _givenXP = 1;
    public int _moveSpeed = 1;
    public int _armor = 1;
    public int _elementalResistance = 1;
    public int _technicalResistance = 1;
}
