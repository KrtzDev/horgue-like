using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    private enum WinningCondition
    {
        KillAllEnemies,
        SurviveForTime,
        KillSpecificEnemy
    }

    [SerializeField]
    private List<GameObject> managers = new List<GameObject>();

    [SerializeField]
    private GameObject _endscreen_Prefab;

    [Header("WinningCondition")]
    [SerializeField]
    private WinningCondition _winningCondition;

    [SerializeField]
    private float _timeToSurvive;

    private EnemySpawner _enemySpawner;
    private int _neededEnemyKill;
    private bool _hasWon;

    private void Start()
    {
        SceneLoader.Instance.CompletedSceneLoad += OnCompletedSceneLoad;
    }

    private void OnCompletedSceneLoad()
    {
        Debug.Log("Scene Load");
        if (SceneManager.GetActiveScene().name == "SCENE_Main_Menu") return;
        _enemySpawner = GameObject.Find("EnemySpawner").GetComponent<EnemySpawner>();
        _neededEnemyKill = _enemySpawner._enemySpawnerData._enemyWavesToSpawn * _enemySpawner._enemySpawnerData._enemyWaveSize;
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

    public void PlayerDied()
    {
        RoundLost();
    }

    private void RoundWon()
    {
        Debug.Log("Round won");
        InputManager.Instance.CharacterInputActions.Disable();
        UIManager.Instance.Endscreen.gameObject.SetActive(true);
        _hasWon = false;
    }

    private void RoundLost()
    {
        Debug.Log("Round Lost");
        InputManager.Instance.CharacterInputActions.Disable();
        UIManager.Instance.Endscreen.gameObject.SetActive(true);
        _hasWon = false;
    }
}
