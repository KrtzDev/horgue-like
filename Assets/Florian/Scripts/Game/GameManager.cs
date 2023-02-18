using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.AI;

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
	[SerializeField]
	private GameObject _waveendscreen_Prefab;

	[SerializeField]
	private GameObject _loadData;

	public List<GameManagerValues> levelBuff = new List<GameManagerValues>();

	[Header("WinningCondition")]
	[SerializeField]
	private WinningCondition _winningCondition;
	[SerializeField]
	private float _timeToSurvive;

	[HideInInspector]
	public float _currentTimeToSurvive;

	public int _currentScore;

	private EnemySpawner _enemySpawner;
	public int _neededEnemyKill;
	private bool _hasWon;
	private bool _hasLost;

	public int _currentLevel = 0;
	public int _currentWave = 0;

	public bool _playerCanUseAbilities;

	private void Start()
	{
		SceneLoader.Instance.CompletedSceneLoad += OnCompletedSceneLoad;

		if (_currentLevel == 0)
		{
			_currentLevel += 1;
		}

		_currentScore = 0;
		_currentTimeToSurvive = _timeToSurvive;
	}

	private void OnCompletedSceneLoad()
	{
		Debug.Log("Scene Load");


		if (SceneManager.GetActiveScene().name == "SCENE_Main_Menu")
        {
			_loadData.SetActive(true);
			return;
        }

		_loadData.SetActive(false);

		_hasWon = false;
		_hasLost = false;

		_enemySpawner = GameObject.Find("EnemySpawner").GetComponent<EnemySpawner>();
		_neededEnemyKill = _enemySpawner.EnemyMaxAmount;

		_currentWave += 1;

		if (_currentLevel >= 2)
		{
			_playerCanUseAbilities = true;
		}
		else
		{
			_playerCanUseAbilities = false;
		}

		Debug.Log("neededEnemyKill ( " + _neededEnemyKill + " ) = enemySpawner.MaxAmount ( " + _enemySpawner.EnemyMaxAmount + " )");
	}

	private void Update()
	{
		if (SceneManager.GetActiveScene().name == "SCENE_Main_Menu") return;

		// Zeit abgelaufen

		if (!_hasWon)
            if (!_hasLost)
				_currentTimeToSurvive -= Time.deltaTime;
		if (!_hasWon && _currentTimeToSurvive <= 0 && _winningCondition == WinningCondition.SurviveForTime)
		{
			_hasWon = true;
			_currentTimeToSurvive = _timeToSurvive;
			RoundWon();
		}
	}

	public void EnemyDied()
	{
		_neededEnemyKill--;

		// alle Gegner get�tet

		if (!_hasWon && _neededEnemyKill == 0 && _winningCondition == WinningCondition.KillAllEnemies)
		{
			_hasWon = true;
			RoundWon();
		}

		// einen bestimmten Gegner get�tet

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
		if (!_hasWon)
			_hasLost = true;
			RoundLost();
	}

	private void RoundWon()
	{
		Debug.Log("Round won");
		InputManager.Instance.CharacterInputActions.Disable();

		if(_currentWave >= 3)
        {
			UIManager.Instance.Endscreen.gameObject.SetActive(true);

			_currentLevel += 1;
			_currentWave = 0;

			if(_currentLevel > 3)
            {
				_currentLevel = 0;
            }
        }
		else
        {
			UIManager.Instance.WaveEndScreen.gameObject.SetActive(true);
		}

		EnemyStopFollowing();
	}

	private void EnemyStopFollowing()
	{
		for (int i = 0; i < _enemySpawner.transform.childCount; i++)
		{
			for (int j = 0; j < _enemySpawner.transform.GetChild(i).childCount; j++)
			{
				// _EnemySpawner.transform.GetChild(i).GetChild(j).GetComponent<EnemyMovement>().PlayerTarget = _Decoy.transform;
				_enemySpawner.transform.GetChild(i).GetChild(j).GetComponent<NavMeshAgent>().enabled = false;
			}
		}
		_enemySpawner.gameObject.SetActive(false);
	}

	private void RoundLost()
	{
		Debug.Log("Round Lost");
		InputManager.Instance.CharacterInputActions.Disable();
		UIManager.Instance.Endscreen.gameObject.SetActive(true);
		UIManager.Instance.WaveEndScreen.gameObject.SetActive(false);
		EnemyStopFollowing();
	}
}
