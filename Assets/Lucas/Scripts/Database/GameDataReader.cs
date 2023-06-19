using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameDataReader : MonoBehaviour
{
    public TextAsset gameData;
    public Button startButton;

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
    private List<EnemySpawnerData> _enemySpawnerData = new List<EnemySpawnerData>();

    [System.Serializable]
    public class Player
    {
        public string Name;
        public float movementSpeed;
        public float maxHealth;
    }

    [System.Serializable]
    public class Enemy
    {
        public string Name;
        public float maxHealth;
        public float damagePerHit;
        public float attackSpeed;
        public float givenXP;
        public float moveSpeed;
        public float armor;
        public float elementalResistance;
        public float technicalResistance;
    }

    [System.Serializable]
    public class Level
    {
        public string Name;
        public float healthBonus;
        public float damageBonus;
        public int TimeToSurvive;
        public int spawnTick;
        public int spawnsPerTick;
        public int minEnemyCount;
        public int maxEnemyCount;
        public float spawnAnimDelay;
        public float minSpawnDelay;
        public float maxSpawnDelay;
        public float safeZoneSize;
        public float closeZoneSize;
        public float midZoneSize;
        public float farZoneSize;
        public int maxCloseZoneOcc;
        public int maxMidZoneOcc;
        public int maxFarZoneOcc;
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

    private void OnEnable()
    {
        StartCoroutine(GetListsFromJSON());
    }

    private IEnumerator GetListsFromJSON()
    {
        myPlayerList = JsonUtility.FromJson<PlayerList>(gameData.text);
        myEnemyList = JsonUtility.FromJson<EnemyList>(gameData.text);
        myLevelList = JsonUtility.FromJson<LevelList>(gameData.text);

        yield return null;

        StartCoroutine(GetData());
    }

    private IEnumerator GetData()
    {
        StartCoroutine(GetPlayerData());
        StartCoroutine(GetEnemyData());
        StartCoroutine(GetLevelData());

        yield return null;

        Debug.Log("DONE DATA");

        if(startButton != null)
        {
            StartCoroutine(EnableButton());
        }
    }

    private IEnumerator EnableButton()
    {
        startButton.GetComponent<Button>().interactable = true;
        startButton.GetComponentInChildren<TextMeshProUGUI>().text = "START";

        yield return null;

        Debug.Log("DONE BUTTON");
    }

    private IEnumerator GetPlayerData()
    {
        _playerData._movementSpeed = myPlayerList.Player[0].movementSpeed;
        _playerData._maxHealth = Mathf.RoundToInt(myPlayerList.Player[0].maxHealth);

        yield return null;
    }

    private IEnumerator GetEnemyData()
    {
        _basicEnemyData._maxHealth = (int)myEnemyList.Enemies[0].maxHealth; //* GameManager.Instance._GameManagerValues[GameManager.Instance._currentLevelArray]._healthBonus);
        _basicEnemyData._damagePerHit = (int)myEnemyList.Enemies[0].damagePerHit; //* GameManager.Instance._GameManagerValues[GameManager.Instance._currentLevelArray]._damageBonus);
        _basicEnemyData._attackSpeed = myEnemyList.Enemies[0].attackSpeed;
        _basicEnemyData._givenXP = (int)myEnemyList.Enemies[0].givenXP;
        _basicEnemyData._moveSpeed = myEnemyList.Enemies[0].moveSpeed;
        _basicEnemyData._armor = myEnemyList.Enemies[0].armor;
        _basicEnemyData._elementalResistance = myEnemyList.Enemies[0].elementalResistance;
        _basicEnemyData._technicalResistance = myEnemyList.Enemies[0].technicalResistance;
        Debug.Log("Basic");

        _rangedRobotData._maxHealth = Mathf.RoundToInt((myEnemyList.Enemies[1].maxHealth * myEnemyList.Enemies[0].maxHealth));
        _rangedRobotData._damagePerHit = Mathf.RoundToInt(myEnemyList.Enemies[1].damagePerHit * myEnemyList.Enemies[0].damagePerHit);
        _rangedRobotData._attackSpeed = myEnemyList.Enemies[1].attackSpeed;
        _rangedRobotData._givenXP = (int)myEnemyList.Enemies[1].givenXP;
        _rangedRobotData._moveSpeed = myEnemyList.Enemies[1].moveSpeed;
        _rangedRobotData._armor = myEnemyList.Enemies[1].armor;
        _rangedRobotData._elementalResistance = myEnemyList.Enemies[1].elementalResistance;
        _rangedRobotData._technicalResistance = myEnemyList.Enemies[1].technicalResistance;
        Debug.Log("RangedRobot");

        _pasuKanData._maxHealth = Mathf.RoundToInt(myEnemyList.Enemies[2].maxHealth * myEnemyList.Enemies[0].maxHealth);
        _pasuKanData._damagePerHit = Mathf.RoundToInt(myEnemyList.Enemies[2].damagePerHit * myEnemyList.Enemies[0].damagePerHit);
        _pasuKanData._attackSpeed = myEnemyList.Enemies[2].attackSpeed;
        _pasuKanData._givenXP = (int)myEnemyList.Enemies[2].givenXP;
        _pasuKanData._moveSpeed = myEnemyList.Enemies[2].moveSpeed;
        _pasuKanData._armor = myEnemyList.Enemies[2].armor;
        _pasuKanData._elementalResistance = myEnemyList.Enemies[2].elementalResistance;
        _pasuKanData._technicalResistance = myEnemyList.Enemies[2].technicalResistance;
        Debug.Log("PasuKan");

        _gigaRangedRobotData._maxHealth = Mathf.RoundToInt(myEnemyList.Enemies[3].maxHealth * myEnemyList.Enemies[0].maxHealth);
        _gigaRangedRobotData._damagePerHit = Mathf.RoundToInt(myEnemyList.Enemies[3].damagePerHit * myEnemyList.Enemies[0].damagePerHit);
        _gigaRangedRobotData._attackSpeed = myEnemyList.Enemies[3].attackSpeed;
        _gigaRangedRobotData._givenXP = (int)myEnemyList.Enemies[3].givenXP;
        _gigaRangedRobotData._moveSpeed = myEnemyList.Enemies[3].moveSpeed;
        _gigaRangedRobotData._armor = myEnemyList.Enemies[3].armor;
        _gigaRangedRobotData._elementalResistance = myEnemyList.Enemies[3].elementalResistance;
        _gigaRangedRobotData._technicalResistance = myEnemyList.Enemies[3].technicalResistance;
        Debug.Log("GIGA RangedRobot");

        yield return null;
    }
    private IEnumerator GetLevelData()
    {

        for (int i = 0; i < _enemySpawnerData.Count; i++)
        {
            _enemySpawnerData[i]._spawnTick = myLevelList.Level[i].spawnTick;
            _enemySpawnerData[i]._spawnsPerTick = myLevelList.Level[i].spawnsPerTick;
            _enemySpawnerData[i]._minEnemyCount = myLevelList.Level[i].minEnemyCount;
            _enemySpawnerData[i]._maxEnemyCount = myLevelList.Level[i].maxEnemyCount;
            _enemySpawnerData[i]._spawnAnimDelay = myLevelList.Level[i].spawnAnimDelay;
            _enemySpawnerData[i]._minSpawnDelay = myLevelList.Level[i].minSpawnDelay;
            _enemySpawnerData[i]._maxSpawnDelay = myLevelList.Level[i].maxSpawnDelay;
            _enemySpawnerData[i]._safeZoneSize = myLevelList.Level[i].safeZoneSize;
            _enemySpawnerData[i]._closeZoneSize = myLevelList.Level[i].closeZoneSize;
            _enemySpawnerData[i]._midZoneSize = myLevelList.Level[i].midZoneSize;
            _enemySpawnerData[i]._farZoneSize = myLevelList.Level[i].farZoneSize;
            _enemySpawnerData[i]._maxCloseZoneOcc = myLevelList.Level[i].maxCloseZoneOcc;
            _enemySpawnerData[i]._maxMidZoneOcc = myLevelList.Level[i].maxMidZoneOcc;
            _enemySpawnerData[i]._maxFarZoneOcc = myLevelList.Level[i].maxFarZoneOcc;
        }

        _Level01._healthBonus = myLevelList.Level[0].healthBonus;
        _Level01._damageBonus = myLevelList.Level[0].damageBonus;
        _Level01._timeToSurvive = myLevelList.Level[0].TimeToSurvive;

        _Level02._healthBonus = myLevelList.Level[1].healthBonus;
        _Level02._damageBonus = myLevelList.Level[1].damageBonus;
        _Level02._timeToSurvive = myLevelList.Level[1].TimeToSurvive;

        _Level03._healthBonus = myLevelList.Level[2].healthBonus;
        _Level03._damageBonus = myLevelList.Level[2].damageBonus;
        _Level03._timeToSurvive = myLevelList.Level[2].TimeToSurvive;

        yield return null;
    }
}
