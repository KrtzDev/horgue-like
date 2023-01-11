using UnityEngine;

[CreateAssetMenu(fileName = "newEnemySpawnerData", menuName = "Data/EnemySpawner Data/Base Data")]
public class EnemySpawnerData : ScriptableObject
{
    public int _enemyWavesToSpawn = 5;
    public int _enemyWaveSize = 50;
    public int _enemySpawnDelay = 5;
}
