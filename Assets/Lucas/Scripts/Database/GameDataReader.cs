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
    [SerializeField] private PlayerData _playerData;

    [Header("ENEMY DATA")]
    [SerializeField] private List<BasicEnemyData> _enemyData = new List<BasicEnemyData>();

    [Header("LEVEL DATA")]
    [SerializeField] private List<GameManagerValues> _LevelData = new List<GameManagerValues>();

    [Header("ENEMY SPAWNER DATA")]
    [SerializeField] private List<EnemySpawnerData> _enemySpawnerData = new List<EnemySpawnerData>();

    [Header("FIREARM DATA")]
    [SerializeField] private List<Ammunition> _ammunitionData = new List<Ammunition>();
    [SerializeField] private List<Barrel> _barrelData = new List<Barrel>();
    [SerializeField] private List<Grip> _gripData = new List<Grip>();
    [SerializeField] private List<Magazine> _magazineData = new List<Magazine>();
    [SerializeField] private List<Sight> _sightData = new List<Sight>();
    [SerializeField] private List<TriggerMechanism> _triggerMechanismData = new List<TriggerMechanism>();

    [System.Serializable]
    public class Player
    {
        public string name;
        public float movementSpeed;
        public float maxHealth;
    }

    [System.Serializable]
    public class Enemy
    {
        public string name;
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
        public string name;
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
    public class AmmunitionDB
    {
        public string name;
        public string effect;
        public int numberOfGems;
        public float attackSpeed;
        public float baseDamage;
        public float cooldown;
        public float criticalHitChance;
        public float projectileWidth;
        public float range;
    }

    [System.Serializable]
    public class BarrelDB
    {
        public string name;
        public string attackPattern;
        public int simultaneousProjectiles;
        public string effect;
        public int numberOfGems;
        public float attackSpeed;
        public float baseDamage;
        public float cooldown;
        public float criticalHitChance;
        public float projectileWidth;
        public float range;
    }

    [System.Serializable]
    public class GripDB
    {
        public string name;
        public string effect;
        public int numberOfBarrels;
        public int numberOfGems;
        public int numberOfMagazines;
        public float attackSpeed;
        public float baseDamage;
        public float cooldown;
        public float criticalHitChance;
        public float projectileWidth;
        public float range;
    }

    [System.Serializable]
    public class MagazineDB
    {
        public string name;
        public int capacity;
        public string effect;
        public int numberOfAmmunitionSlots;
        public int numberOfGems;
        public float attackSpeed;
        public float baseDamage;
        public float cooldown;
        public float criticalHitChance;
        public float projectileWidth;
        public float range;
    }

    [System.Serializable]
    public class SightDB
    {
        public string name;
        public string attackPattern;
        public string effect;
        public int numberOfGems;
        public float attackSpeed;
        public float baseDamage;
        public float cooldown;
        public float criticalHitChance;
        public float projectileWidth;
        public float range;
    }

    [System.Serializable]
    public class TriggerMechanismDB
    {
        public string name;
        public string effect;
        public int numberOfGems;
        public float attackSpeed;
        public float baseDamage;
        public float cooldown;
        public float criticalHitChance;
        public float projectileWidth;
        public float range;
    }

    [System.Serializable]
    public class PlayerList
    {
        public Player[] Player;
    }
    public PlayerList myPlayerList = new();

    [System.Serializable]
    public class EnemyList
    {
        public Enemy[] Enemies;
    }
    public EnemyList myEnemyList = new();

    [System.Serializable]
    public class LevelList
    {
        public Level[] Level;
    }
    public LevelList myLevelList = new();

    [System.Serializable]
    public class AmmunitionList
    {
        public AmmunitionDB[] Ammunition;
    }
    public AmmunitionList myAmmunitionList = new();

    [System.Serializable]
    public class BarrelList
    {
        public BarrelDB[] Barrel;
    }
    public BarrelList myBarrelList = new();

    [System.Serializable]
    public class GripList
    {
        public GripDB[] Grip;
    }
    public GripList myGripList = new();

    [System.Serializable]
    public class MagazineList
    {
        public MagazineDB[] Magazine;
    }
    public MagazineList myMagazineList = new();

    [System.Serializable]
    public class SightList
    {
        public SightDB[] Sight;
    }
    public SightList mySightList = new();

    [System.Serializable]
    public class TriggerMechanismList
    {
        public TriggerMechanismDB[] TriggerMechanism;
    }
    public TriggerMechanismList myTriggerMechanismList = new();

    private void OnEnable()
    {
        StartCoroutine(GetListsFromJSON());
    }

    private IEnumerator GetListsFromJSON()
    {
        myPlayerList = JsonUtility.FromJson<PlayerList>(gameData.text);
        myEnemyList = JsonUtility.FromJson<EnemyList>(gameData.text);
        myLevelList = JsonUtility.FromJson<LevelList>(gameData.text);
        myAmmunitionList = JsonUtility.FromJson<AmmunitionList>(gameData.text);
        myBarrelList = JsonUtility.FromJson<BarrelList>(gameData.text);
        myGripList = JsonUtility.FromJson<GripList>(gameData.text);
        myMagazineList = JsonUtility.FromJson<MagazineList>(gameData.text);
        mySightList = JsonUtility.FromJson<SightList>(gameData.text);
        myTriggerMechanismList = JsonUtility.FromJson<TriggerMechanismList>(gameData.text);

        yield return null;

        StartCoroutine(GetData());
    }

    private IEnumerator GetData()
    {
        StartCoroutine(GetPlayerData());
        StartCoroutine(GetEnemyData());
        StartCoroutine(GetLevelData());
        StartCoroutine(GetFireArmData());

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
        for (int i = 0; i < _enemyData.Count; i++)
        {
            if (i == 0)
            {
                _enemyData[i]._maxHealth = (int)myEnemyList.Enemies[0].maxHealth; //* GameManager.Instance._GameManagerValues[GameManager.Instance._currentLevelArray]._healthBonus);
                _enemyData[i]._damagePerHit = (int)myEnemyList.Enemies[0].damagePerHit; //* GameManager.Instance._GameManagerValues[GameManager.Instance._currentLevelArray]._damageBonus);
                _enemyData[i]._attackSpeed = myEnemyList.Enemies[i].attackSpeed;
                _enemyData[i]._givenXP = (int)myEnemyList.Enemies[i].givenXP;
                _enemyData[i]._moveSpeed = myEnemyList.Enemies[i].moveSpeed;
                _enemyData[i]._armor = myEnemyList.Enemies[i].armor;
                _enemyData[i]._elementalResistance = myEnemyList.Enemies[i].elementalResistance;
                _enemyData[i]._technicalResistance = myEnemyList.Enemies[i].technicalResistance;
            }
            else
            {
                _enemyData[i]._maxHealth = Mathf.RoundToInt((myEnemyList.Enemies[i].maxHealth * _enemyData[0]._maxHealth));
                _enemyData[i]._damagePerHit = Mathf.RoundToInt(myEnemyList.Enemies[i].damagePerHit * _enemyData[0]._damagePerHit);
                _enemyData[i]._attackSpeed = myEnemyList.Enemies[i].attackSpeed * _enemyData[0]._attackSpeed;
                _enemyData[i]._givenXP = Mathf.RoundToInt(myEnemyList.Enemies[i].givenXP * _enemyData[0]._givenXP);
                _enemyData[i]._moveSpeed = myEnemyList.Enemies[i].moveSpeed * _enemyData[0]._moveSpeed;
                _enemyData[i]._armor = myEnemyList.Enemies[i].armor * _enemyData[0]._armor;
                _enemyData[i]._elementalResistance = myEnemyList.Enemies[i].elementalResistance * _enemyData[0]._elementalResistance;
                _enemyData[i]._technicalResistance = myEnemyList.Enemies[i].technicalResistance * _enemyData[0]._technicalResistance;
            }
        }

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

        for (int i = 0; i < _LevelData.Count; i++)
        {
            _LevelData[i]._healthBonus = myLevelList.Level[0].healthBonus;
            _LevelData[i]._damageBonus = myLevelList.Level[0].damageBonus;
            _LevelData[i]._timeToSurvive = myLevelList.Level[0].TimeToSurvive;
        }

        yield return null;
    }
    private IEnumerator GetFireArmData()
    {
        GetAmmunitionData();
        GetBarrelData();
        GetGripData();
        GetMagazineData();
        GetSightData();
        GetTriggerMechanismData();

        yield return null;
    }

    private void GetAmmunitionData()
    {
        for (int i = 0; i < _ammunitionData.Count; i++)
        {
            if(i == 0)
            {
                _ammunitionData[i].baseDamage = myAmmunitionList.Ammunition[i].baseDamage;
                _ammunitionData[i].attackSpeed = myAmmunitionList.Ammunition[i].attackSpeed;
                _ammunitionData[i].cooldown = myAmmunitionList.Ammunition[i].cooldown;
                _ammunitionData[i].projectileSize = myAmmunitionList.Ammunition[i].projectileWidth;
                _ammunitionData[i].critChance = myAmmunitionList.Ammunition[i].criticalHitChance;
                _ammunitionData[i].range = myAmmunitionList.Ammunition[i].range;
            }
            else
            {
                _ammunitionData[i].baseDamage = myAmmunitionList.Ammunition[i].baseDamage * myAmmunitionList.Ammunition[0].baseDamage;
                _ammunitionData[i].attackSpeed = myAmmunitionList.Ammunition[i].attackSpeed * myAmmunitionList.Ammunition[0].attackSpeed;
                _ammunitionData[i].cooldown = myAmmunitionList.Ammunition[i].cooldown * myAmmunitionList.Ammunition[0].cooldown;
                _ammunitionData[i].projectileSize = myAmmunitionList.Ammunition[i].projectileWidth * myAmmunitionList.Ammunition[0].projectileWidth;
                _ammunitionData[i].critChance = myAmmunitionList.Ammunition[i].criticalHitChance * myAmmunitionList.Ammunition[0].criticalHitChance;
                _ammunitionData[i].range = myAmmunitionList.Ammunition[i].range * myAmmunitionList.Ammunition[0].range;
            }
        }
    }
    private void GetBarrelData()
    {
        for (int i = 0; i < _barrelData.Count; i++)
        {
            if (i == 0)
            {
                _barrelData[i].baseDamage = myBarrelList.Barrel[i].baseDamage;
                _barrelData[i].attackSpeed = myBarrelList.Barrel[i].attackSpeed;
                _barrelData[i].cooldown = myBarrelList.Barrel[i].cooldown;
                _barrelData[i].projectileSize = myBarrelList.Barrel[i].projectileWidth;
                _barrelData[i].critChance = myBarrelList.Barrel[i].criticalHitChance;
                _barrelData[i].range = myBarrelList.Barrel[i].range;
            }
            else
            {
                _barrelData[i].baseDamage = myBarrelList.Barrel[i].baseDamage * myBarrelList.Barrel[0].baseDamage;
                _barrelData[i].attackSpeed = myBarrelList.Barrel[i].attackSpeed * myBarrelList.Barrel[0].attackSpeed;
                _barrelData[i].cooldown = myBarrelList.Barrel[i].cooldown * myBarrelList.Barrel[0].cooldown;
                _barrelData[i].projectileSize = myBarrelList.Barrel[i].projectileWidth * myBarrelList.Barrel[0].projectileWidth;
                _barrelData[i].critChance = myBarrelList.Barrel[i].criticalHitChance * myBarrelList.Barrel[0].criticalHitChance;
                _barrelData[i].range = myBarrelList.Barrel[i].range * myBarrelList.Barrel[0].range;
            }
        }
    }
    private void GetGripData()
    {
        for (int i = 0; i < _gripData.Count; i++)
        {
            if (i == 0)
            {
                _gripData[i].attackSpeed = myGripList.Grip[i].attackSpeed;
                _gripData[i].cooldown = myGripList.Grip[i].cooldown;
                _gripData[i].critChance = myGripList.Grip[i].criticalHitChance;
            }
            else
            {
                _gripData[i].attackSpeed = myGripList.Grip[i].attackSpeed * myGripList.Grip[0].attackSpeed;
                _gripData[i].cooldown = myGripList.Grip[i].cooldown * myGripList.Grip[0].cooldown;
                _gripData[i].critChance = myGripList.Grip[i].criticalHitChance * myGripList.Grip[0].criticalHitChance;
            }
        }
    }
    private void GetMagazineData()
    {
        for (int i = 0; i < _magazineData.Count; i++)
        {
            if (i == 0)
            {
                _magazineData[i].attackSpeed = myMagazineList.Magazine[i].attackSpeed;
                _magazineData[i].cooldown = myMagazineList.Magazine[i].cooldown;
                _magazineData[i].projectileSize = myMagazineList.Magazine[i].projectileWidth;
            }
            else
            {
                _magazineData[i].attackSpeed = myMagazineList.Magazine[i].attackSpeed * myMagazineList.Magazine[0].attackSpeed;
                _magazineData[i].cooldown = myMagazineList.Magazine[i].cooldown * myMagazineList.Magazine[0].cooldown;
                _magazineData[i].projectileSize = myMagazineList.Magazine[i].projectileWidth * myMagazineList.Magazine[0].projectileWidth;
            }
        }
    }
    private void GetSightData()
    {
        for (int i = 0; i < _sightData.Count; i++)
        {
            if (i == 0)
            {
                _sightData[i].attackSpeed = mySightList.Sight[i].attackSpeed;
                _sightData[i].critChance = mySightList.Sight[i].criticalHitChance;
                _sightData[i].range = mySightList.Sight[i].range;
            }
            else
            {
                _sightData[i].attackSpeed = mySightList.Sight[i].attackSpeed * mySightList.Sight[0].attackSpeed;
                _sightData[i].critChance = mySightList.Sight[i].criticalHitChance * mySightList.Sight[0].criticalHitChance;
                _sightData[i].range = mySightList.Sight[i].range * mySightList.Sight[0].range;
            }
        }
    }
    private void GetTriggerMechanismData()
    {
        for (int i = 0; i < _triggerMechanismData.Count; i++)
        {
            if (i == 0)
            {
                _triggerMechanismData[i].baseDamage = myTriggerMechanismList.TriggerMechanism[i].baseDamage;
                _triggerMechanismData[i].attackSpeed = myTriggerMechanismList.TriggerMechanism[i].attackSpeed;
                _triggerMechanismData[i].cooldown = myTriggerMechanismList.TriggerMechanism[i].cooldown;
                _triggerMechanismData[i].critChance = myTriggerMechanismList.TriggerMechanism[i].criticalHitChance;
                _triggerMechanismData[i].range = myTriggerMechanismList.TriggerMechanism[i].range;
            }
            else
            {
                _triggerMechanismData[i].baseDamage = myTriggerMechanismList.TriggerMechanism[i].baseDamage * myTriggerMechanismList.TriggerMechanism[0].baseDamage;
                _triggerMechanismData[i].attackSpeed = myTriggerMechanismList.TriggerMechanism[i].attackSpeed * myTriggerMechanismList.TriggerMechanism[0].attackSpeed;
                _triggerMechanismData[i].cooldown = myTriggerMechanismList.TriggerMechanism[i].cooldown * myTriggerMechanismList.TriggerMechanism[0].cooldown;
                _triggerMechanismData[i].critChance = myTriggerMechanismList.TriggerMechanism[i].criticalHitChance * myTriggerMechanismList.TriggerMechanism[0].criticalHitChance;
                _triggerMechanismData[i].range = myTriggerMechanismList.TriggerMechanism[i].range * myTriggerMechanismList.TriggerMechanism[0].range;
            }
        }
    }
}
