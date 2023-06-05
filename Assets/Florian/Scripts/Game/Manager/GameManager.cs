using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
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
	private GameObject _loadData;
	[SerializeField]
	private GameObject _gameDataReader;

	public List<GameManagerValues> _GameManagerValues = new List<GameManagerValues>();

	[Header("WinningCondition")]
	[SerializeField]
	private WinningCondition _winningCondition;

	[HideInInspector]
	public float _currentTimeToSurvive;

	public int _currentScore;

	private EnemySpawner _enemySpawner;
	public int _neededEnemyKill;
	public int _enemyCount;
	private bool _hasWon;
	private bool _hasLost;

	public int _currentLevel = 1;
	public int _currentLevelArray;
	public int _currentWave = 0;

	private int _numberOfRewards = 3;

	public bool _playerCanUseAbilities;

	private void Start()
	{
		StartCoroutine(GetGameData());

		SceneLoader.Instance.CompletedSceneLoad += OnCompletedSceneLoad;

		_currentScore = 0;
		_currentLevel = 1;
		_currentLevelArray = _currentLevel - 1;
	}

	private void OnCompletedSceneLoad()
	{
		Debug.Log("Scene Load");

		_currentTimeToSurvive = _GameManagerValues[_currentLevelArray]._timeToSurvive;

		if (SceneManager.GetActiveScene().name == "SCENE_Weapon_Crafting")
		{
			return;
		}

		if (SceneManager.GetActiveScene().name == "SCENE_Main_Menu")
		{
			_currentLevel = 1;
			_currentWave = 0;

			_loadData.SetActive(true);
			return;
		}

		_loadData.SetActive(false);

		_hasWon = false;
		_hasLost = false;

		_enemySpawner = GameObject.Find("EnemySpawner").GetComponent<EnemySpawner>();
		_neededEnemyKill = _enemySpawner.EnemyMaxAmount;

		if (_currentLevel == 1 && _currentWave == 0)
		{
			WeaponHolster weaponHolster = FindObjectOfType<WeaponHolster>();
			foreach (var weapon in weaponHolster.weapons)
			{
				weapon.ResetWeaponParts();
			}
		}

		_currentWave += 1;

		// if (_currentLevel >= 2)
		if (_currentLevel >= 0)
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
		if (SceneManager.GetActiveScene().name == "SCENE_Main_Menu" || SceneManager.GetActiveScene().name == "SCENE_Init") return;

		// Zeit abgelaufen

		if (!_hasWon)
			if (!_hasLost)
				_currentTimeToSurvive -= Time.deltaTime;
		if (!_hasWon && _currentTimeToSurvive <= 0 && _winningCondition == WinningCondition.SurviveForTime)
		{
			_hasWon = true;
			_currentTimeToSurvive = _GameManagerValues[_currentLevelArray]._timeToSurvive;
			RoundWon();
		}
	}

	public void EnemyDied()
	{
		_neededEnemyKill--;
		_enemyCount--;

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
			_hasLost = true;
		RoundLost();
	}

	private void RoundWon()
	{
		Debug.Log("Round won");
		InputManager.Instance.CharacterInputActions.Disable();

		if (_currentWave >= 3)
		{
			List<Reward> rewards = new List<Reward>();
			for (int i = 0; i < _numberOfRewards; i++)
			{
				rewards.Add(RewardManager.Instance.GetRandomReward());
			}

			UIManager.Instance.ShowLevelEndScreen(LevelStatus.Won);
			UIManager.Instance.DisplayRewards(rewards);


			_currentLevel += 1;
			_currentLevelArray = _currentLevel - 1;
			_currentWave = 0;

			if (_currentLevel > 3)
			{
				_currentLevel = 1;
				_currentLevelArray = _currentLevel - 1;
			}
		}
		else
		{
			UIManager.Instance.ShowWaveEndScreen(LevelStatus.Won);
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
		UIManager.Instance.ShowLevelEndScreen(LevelStatus.Lost);
		UIManager.Instance.WaveEndScreen.gameObject.SetActive(false);
		EnemyStopFollowing();
	}

	IEnumerator GetGameData()
    {
		_gameDataReader.SetActive(true);
		yield return null;
	}
}
