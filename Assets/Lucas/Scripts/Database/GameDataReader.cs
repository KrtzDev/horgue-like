using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDataReader : MonoBehaviour
{
    public TextAsset gameData;

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
        public float WavesToSpawn;
        public float WaveSize;
        public float SpawnDelay;
        public float TimeToSurvive;
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
    }

    private void Update()
    {
        
    }
}
