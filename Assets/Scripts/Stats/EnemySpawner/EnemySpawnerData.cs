using UnityEngine;

[CreateAssetMenu(fileName = "newEnemySpawnerData", menuName = "Data/EnemySpawner Data/Base Data")]
public class EnemySpawnerData : ScriptableObject
{
    [Header("Level Data")]
    public int _timeToSurvive;
    public float _weaponPartMultiplierPerLevel;

    [Header("Spawn Settings")]
    public float _spawnTick;
    public int _spawnsPerTick;
    public int _minEnemyCount;
    public int _maxEnemyCount;

    [Header("Spawn Delay")]
    public float _spawnAnimDelay;
    public float _minSpawnDelay;
    public float _maxSpawnDelay;

    [Header("Square Zone Sizes")]
    public float _safeZoneSize;
    public float _closeZoneSize;
    public float _midZoneSize;
    public float _farZoneSize;

    [Header("Zone Maxx Occupation")]
    public int _maxCloseZoneOcc;
    public int _maxMidZoneOcc;
    public int _maxFarZoneOcc;
}
