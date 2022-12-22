using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance { get; private set; }

    private enum WinningCondition
    {
        KillAllEnemies,
        SurviveForTime,
        KillSpecificEnemy
    }

    [SerializeField]
    private GameObject _endscreen_Prefab;
    [SerializeField]
    private WinningCondition _winningCondition;

    [Header("WinningCondition")]
    [SerializeField]
    private float _timeToSurvive;

    private EnemySpawner _enemySpawner;
    private int _neededEnemyKill;
    private bool _hasWon;

    private void Awake()
    {
        if (!instance)
        {
            instance = this;
        }
    }

    private void Start()
    {
        _enemySpawner = GameObject.Find("EnemySpawner").GetComponent<EnemySpawner>();
        _neededEnemyKill = _enemySpawner.EnemyWavesToSpawn * _enemySpawner.EnemyWaveSize;
    }

    private void Update()
    {
        _timeToSurvive -= Time.deltaTime;
        if (!_hasWon && _timeToSurvive <= 0 && _winningCondition == WinningCondition.SurviveForTime)
        {
            _hasWon = true;
            RoundWon();
        }
    }

    public void EnemyDied()
    {
        _neededEnemyKill--;
        if (!_hasWon && _neededEnemyKill == 0 && _winningCondition == WinningCondition.KillAllEnemies)
        {
            _hasWon = true;
            RoundWon();
        }

        if (!_hasWon && _neededEnemyKill == 0 && _winningCondition == WinningCondition.KillSpecificEnemy)
        {
            _enemySpawner.SpawnRandomEnemy();
        }
        if (!_hasWon && _neededEnemyKill == -1 && _winningCondition == WinningCondition.KillSpecificEnemy)
        {

            _hasWon = true;
            RoundWon();
        }
    }

    private void RoundWon()
    {
        _endscreen_Prefab.SetActive(true);
    }
}