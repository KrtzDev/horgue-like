using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDataReader : MonoBehaviour
{
    public TextAsset gameData;

    [Header("PLAYER DATA")]
    [SerializeField]
    private PlayerData _playerData;

    [Header("ENEMY DATA")]
    [SerializeField]
    private BasicEnemyData _basicEnemyData;
    [SerializeField]
    private BasicEnemyData _rangedRobotData;
    [SerializeField]
    private BasicEnemyData _pasuKanData;
    [SerializeField]
    private BasicEnemyData _gigaRangedRobotData;

    [Header("LEVEL DATA")]
    [SerializeField]
    private GameManagerValues _Level01;
    [SerializeField]
    private GameManagerValues _Level02;
    [SerializeField]
    private GameManagerValues _Level03;

    [Header("ENEMY SPAWNER DATA")]
    [SerializeField]
    private EnemySpawnerData _enemySpawnerData1;
    [SerializeField]
    private EnemySpawnerData _enemySpawnerData2;
    [SerializeField]
    private EnemySpawnerData _enemySpawnerData3;

    [System.Serializable]
    public class Player
    {
        public string Name;
        public float movementSpeed;
        public int maxHealth;
    }

    [System.Serializable]
    public class Enemy
    {
        public string Name;
        public int maxHealth;
        public float damagePerHit;
        public float attackSpeed;
        public int givenXP;
        public float moveSpeed;
        public int armor;
        public int elementalResistance;
        public int technicalResistance;
    }

    [System.Serializable]
    public class Level
    {
        public string Name;
        public float healthBonus;
        public float damageBonus;
        public int WavesToSpawn;
        public int WaveSize;
        public int SpawnDelay;
        public int TimeToSurvive;
    }

    [System.Serializable]
    public class PlayerList
    {
        public Player[] Player;
    }

    [System.Serializable]
    public class EnemyList
    {
        public Enemy[] Enemies;
    }

    [System.Serializable]
    public class LevelList
    {
        public Level[] Level;
    }

    public PlayerList myPlayerList = new();
    public EnemyList myEnemyList = new();
    public LevelList myLevelList = new();

    private void Start()
    {
        myPlayerList = JsonUtility.FromJson<PlayerList>(gameData.text);
        myEnemyList = JsonUtility.FromJson<EnemyList>(gameData.text);
        myLevelList = JsonUtility.FromJson<LevelList>(gameData.text);
        GetPlayerData();
        GetEnemyData();
        GetLevelData();
    }

    private void GetPlayerData()
    {
        _playerData._movementSpeed = myPlayerList.Player[0].movementSpeed;
        _playerData._maxHealth = myPlayerList.Player[0].maxHealth;
    }

    private void GetEnemyData()
    {
        _basicEnemyData._maxHealth = myEnemyList.Enemies[0].maxHealth; //* GameManager.Instance._GameManagerValues[GameManager.Instance._currentLevelArray]._healthBonus);
        _basicEnemyData._damagePerHit = (int)myEnemyList.Enemies[0].damagePerHit; //* GameManager.Instance._GameManagerValues[GameManager.Instance._currentLevelArray]._damageBonus);
        _basicEnemyData._attackSpeed = myEnemyList.Enemies[0].attackSpeed;
        _basicEnemyData._givenXP = myEnemyList.Enemies[0].givenXP;
        _basicEnemyData._moveSpeed = myEnemyList.Enemies[0].moveSpeed;
        _basicEnemyData._armor = myEnemyList.Enemies[0].armor;
        _basicEnemyData._elementalResistance = myEnemyList.Enemies[0].elementalResistance;
        _basicEnemyData._technicalResistance = myEnemyList.Enemies[0].technicalResistance;
        Debug.Log("Basic");

        _rangedRobotData._maxHealth = (int)(myEnemyList.Enemies[1].maxHealth * _basicEnemyData._maxHealth);
        _rangedRobotData._damagePerHit = (int)(myEnemyList.Enemies[1].damagePerHit * _basicEnemyData._damagePerHit);
        _rangedRobotData._attackSpeed = myEnemyList.Enemies[1].attackSpeed;
        _rangedRobotData._givenXP = myEnemyList.Enemies[1].givenXP;
        _rangedRobotData._moveSpeed = myEnemyList.Enemies[1].moveSpeed;
        _rangedRobotData._armor = myEnemyList.Enemies[1].armor;
        _rangedRobotData._elementalResistance = myEnemyList.Enemies[1].elementalResistance;
        _rangedRobotData._technicalResistance = myEnemyList.Enemies[1].technicalResistance;
        Debug.Log("RangedRobot");

        _pasuKanData._maxHealth = (int)(myEnemyList.Enemies[2].maxHealth * _basicEnemyData._maxHealth);
        _pasuKanData._damagePerHit = (int)(myEnemyList.Enemies[2].damagePerHit * _basicEnemyData._damagePerHit);
        _pasuKanData._attackSpeed = myEnemyList.Enemies[2].attackSpeed;
        _pasuKanData._givenXP = myEnemyList.Enemies[2].givenXP;
        _pasuKanData._moveSpeed = myEnemyList.Enemies[2].moveSpeed;
        _pasuKanData._armor = myEnemyList.Enemies[2].armor;
        _pasuKanData._elementalResistance = myEnemyList.Enemies[2].elementalResistance;
        _pasuKanData._technicalResistance = myEnemyList.Enemies[2].technicalResistance;
        Debug.Log("PasuKan");

        _gigaRangedRobotData._maxHealth = (int)(myEnemyList.Enemies[3].maxHealth * _basicEnemyData._maxHealth);
        _gigaRangedRobotData._damagePerHit = (int)(myEnemyList.Enemies[3].damagePerHit * _basicEnemyData._damagePerHit);
        _gigaRangedRobotData._attackSpeed = myEnemyList.Enemies[3].attackSpeed;
        _gigaRangedRobotData._givenXP = myEnemyList.Enemies[3].givenXP;
        _gigaRangedRobotData._moveSpeed = myEnemyList.Enemies[3].moveSpeed;
        _gigaRangedRobotData._armor = myEnemyList.Enemies[3].armor;
        _gigaRangedRobotData._elementalResistance = myEnemyList.Enemies[3].elementalResistance;
        _gigaRangedRobotData._technicalResistance = myEnemyList.Enemies[3].technicalResistance;
        Debug.Log("GIGA RangedRobot");
    }
    private void GetLevelData()
    {
        _Level01._healthBonus = myLevelList.Level[0].healthBonus;
        _Level01._damageBonus = myLevelList.Level[0].damageBonus;
        _Level01._timeToSurvive = myLevelList.Level[0].TimeToSurvive;
        _enemySpawnerData1._enemyWavesToSpawn = myLevelList.Level[0].WavesToSpawn;
        _enemySpawnerData1._enemyWaveSize = myLevelList.Level[0].WaveSize;
        _enemySpawnerData1._enemySpawnDelay = myLevelList.Level[0].SpawnDelay;

        _Level02._healthBonus = myLevelList.Level[1].healthBonus;
        _Level02._damageBonus = myLevelList.Level[1].damageBonus;
        _Level02._timeToSurvive = myLevelList.Level[1].TimeToSurvive;
        _enemySpawnerData2._enemyWavesToSpawn = myLevelList.Level[1].WavesToSpawn;
        _enemySpawnerData2._enemyWaveSize = myLevelList.Level[1].WaveSize;
        _enemySpawnerData2._enemySpawnDelay = myLevelList.Level[1].SpawnDelay;

        _Level03._healthBonus = myLevelList.Level[2].healthBonus;
        _Level03._damageBonus = myLevelList.Level[2].damageBonus;
        _Level03._timeToSurvive = myLevelList.Level[2].TimeToSurvive;
        _enemySpawnerData3._enemyWavesToSpawn = myLevelList.Level[2].WavesToSpawn;
        _enemySpawnerData3._enemyWaveSize = myLevelList.Level[2].WaveSize;
        _enemySpawnerData3._enemySpawnDelay = myLevelList.Level[2].SpawnDelay;

    }
}
