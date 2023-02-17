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

	public int _currentLevel = 0;
	public int _currentWave = 0;

	public bool _playerCanUseAbilities;

	private void Start()
	{
		SceneLoader.Instance.CompletedSceneLoad += OnCompletedSceneLoad;

		_currentScore = 0;
		_currentTimeToSurvive = _timeToSurvive;
	}

	private void OnCompletedSceneLoad()
	{
		Debug.Log("Scene Load");
		if (SceneManager.GetActiveScene().name == "SCENE_Main_Menu") return;
		_hasWon = false;
		_enemySpawner = GameObject.Find("EnemySpawner").GetComponent<EnemySpawner>();
		_neededEnemyKill = _enemySpawner.EnemyMaxAmount;

		if (_currentLevel == 0)
		{
			_currentLevel += 1;
		}

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

		// alle Gegner getötet

		if (!_hasWon && _neededEnemyKill == 0 && _winningCondition == WinningCondition.KillAllEnemies)
		{
			_hasWon = true;
			RoundWon();
		}

		// einen bestimmten Gegner getötet

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
