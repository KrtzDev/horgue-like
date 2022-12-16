using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance { get; private set; }

    [SerializeField]
    private GameObject _endscreen_Prefab;

    private int _neededEnemyKill;

    private void Awake()
    {
        if (!instance)
        {
            instance = this;
        }
    }

    private void Start()
    {
        EnemySpawner enemySpawner = GameObject.Find("EnemySpawner").GetComponent<EnemySpawner>();
        _neededEnemyKill = enemySpawner.EnemyWavesToSpawn * enemySpawner.EnemyWaveSize;
    }

    public void EnemyDied()
    {
        _neededEnemyKill--;
        if (_neededEnemyKill == 0)
        {
            RoundWon();
        }
    }

    public void RoundWon()
    {
        _endscreen_Prefab.SetActive(true);
    }
}
