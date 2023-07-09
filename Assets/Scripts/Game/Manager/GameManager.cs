using System.Collections;
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
	private int _lastLevel;
	public int _currentLevelArray;
	public int _currentWave = 0;

	private int _numberOfRewards = 3;


	[Header("Player")]
	public PlayerCharacter player;
	public int _currentPlayerHealth;
	public bool _playerCanUseAbilities;

	private void Start()
	{
		StartCoroutine(GetGameData());

		SceneLoader.Instance.CompletedSceneLoad += OnCompletedSceneLoad;

		_currentScore = 0;
		_currentLevel = 1;
		_lastLevel = 0;
		_currentLevelArray = _currentLevel - 1;
	}

	private void OnCompletedSceneLoad()
	{
		_currentTimeToSurvive = _GameManagerValues[_currentLevelArray]._timeToSurvive;

		if (SceneManager.GetActiveScene().name == "SCENE_Weapon_Crafting")
		{
			return;
		}

		if (SceneManager.GetActiveScene().name == "SCENE_Main_Menu")
		{
			_currentLevel = 1;
			_currentWave = 0;

			return;
		}

		_hasWon = false;
		_hasLost = false;

		_enemySpawner = GameObject.FindGameObjectWithTag("EnemySpawner").GetComponent<EnemySpawner>();
		_neededEnemyKill = _enemySpawner._enemySpawnerData._maxEnemyCount;

		player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerCharacter>();

		if (_currentLevel == 1 && _currentWave == 0)
		{
			WeaponHolster weaponHolster = FindObjectOfType<WeaponHolster>();
			foreach (var weapon in weaponHolster.weapons)
			{
				weapon.ResetWeaponParts();
			}
		}

		if(_lastLevel == _currentLevel)
        {
			player.GetComponent<HealthComponent>()._currentHealth = _currentPlayerHealth;
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

		Debug.Log("neededEnemyKill ( " + _neededEnemyKill + " ) = enemySpawner.MaxAmount ( " + _enemySpawner._enemySpawnerData._maxEnemyCount + " )");
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
			// _enemySpawner.SpawnRandomEnemy();
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
		_playerCanUseAbilities = false;

		_lastLevel = _currentLevel;

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

		_currentPlayerHealth = player.GetComponent<HealthComponent>()._currentHealth;

		EnemyStopFollowing();
	}

	private void EnemyStopFollowing()
	{
		_enemySpawner._enemyObjectPoolParent.SetActive(false);
	}

	private void RoundLost()
	{
		Debug.Log("Round Lost");
		InputManager.Instance.CharacterInputActions.Disable();
		UIManager.Instance.ShowLevelEndScreen(LevelStatus.Lost);
		UIManager.Instance.WaveEndScreen.gameObject.SetActive(false);
		_playerCanUseAbilities = false;
		EnemyStopFollowing();
	}

	IEnumerator GetGameData()
    {
		_gameDataReader.SetActive(true);
		yield return null;
	}
}
