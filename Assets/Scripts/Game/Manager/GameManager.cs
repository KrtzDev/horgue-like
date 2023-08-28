using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.AI;

public class GameManager : Singleton<GameManager>
{
	private enum WinningCondition
	{
		KillAllEnemies,
		SurviveForTime,
		KillSpecificEnemy
	}

	public WeaponControllKind weaponControll = WeaponControllKind.AllAuto;
	public bool returnToAutoShooting = true;

	public DamageCalcKind damageCalcKind = DamageCalcKind.Mean;

	[SerializeField]
	private GameDataReader _gameDataReader;

	public List<EnemySpawnerData> GameManagerValues = new List<EnemySpawnerData>();
	[SerializeField] private int _maxLevels;

	private EnemySpawner _currentEnemySpawner;

	[Header("WinningCondition")]
	[SerializeField]
	private WinningCondition _winningCondition;

	[HideInInspector]
	public float _currentTimeToSurvive;

	public int _currentScore;

	private EnemySpawner _enemySpawner;
	public int _neededEnemyKill;
	public int _enemyCount;
	public int _enemiesKilled;
	public bool killedBoss;
	private bool _hasWon;
	private bool _hasLost;

	public int _currentLevel = 1;
	private int _lastLevel;
	public int _currentLevelArray;

	private int _numberOfRewards = 6;

	public bool _gameIsPaused;

	[Header("New Game Plus")]
	public bool _newGamePlus;
	public int firstLevelNumberInBuild;
	public int lastLevelNumberInBuild;
	public Vector2Int lastLevelNumbers;

	[Header("Player")]
	public GameObject _player;
	public int _currentPlayerHealth;
	public bool _playerCanUseAbilities;
	public Ability _currentAbility;

	[SerializeField]
	public Inventory inventory;

	private void Start()
	{
		SceneLoader.Instance.CompletedSceneLoad += OnCompletedSceneLoad;

		_currentScore = 0;
		_currentLevel = 1;
		_lastLevel = 0;
		_currentLevelArray = _currentLevel - 1;
		_gameIsPaused = false;
	}

	private void OnCompletedSceneLoad()
	{

		if (SceneManager.GetActiveScene().name == "SCENE_Weapon_Crafting")
		{
			return;
		}

		if (SceneManager.GetActiveScene().name == "SCENE_Main_Menu")
		{
			_currentScore = 0;
			_currentLevel = 1;
			_lastLevel = 0;
			_currentLevelArray = _currentLevel - 1;
			_gameIsPaused = false;
			return;
		}

		if(SceneManager.GetActiveScene().name.Contains("Boss"))
        {
			_winningCondition = WinningCondition.KillSpecificEnemy;
		}
		else
        {
			_winningCondition = WinningCondition.SurviveForTime;
		 	killedBoss = false;
		}

		_gameDataReader.GetGameData();
		_gameDataReader.SetGameData();

		if(_currentLevelArray < _maxLevels)
        {
			_currentTimeToSurvive = GameManagerValues[_currentLevelArray]._timeToSurvive;
        }
		else
        {
			_currentTimeToSurvive = GameManagerValues[_maxLevels]._timeToSurvive;
			_currentEnemySpawner = FindObjectOfType<EnemySpawner>();
			_currentEnemySpawner._enemySpawnerData = GameManagerValues[_maxLevels];
		}

		_hasWon = false;
		_hasLost = false;

		_enemyCount = 0;
		_enemySpawner = GameObject.FindGameObjectWithTag("EnemySpawner").GetComponent<EnemySpawner>();
		_neededEnemyKill = _enemySpawner._enemySpawnerData._maxEnemyCount;
		_enemiesKilled = 0;

		_player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerCharacter>().gameObject;
		MovePlayerToRandomPos(DetermineRandomSpawnLocation());

		if (_currentLevel == 1)
		{
			WeaponHolster weaponHolster = FindObjectOfType<WeaponHolster>();
			foreach (var weapon in weaponHolster.weapons)
			{
				weapon.ResetWeaponParts();
			}
		}

		_currentAbility = null;

		if (_currentAbility != null)
        {
			AbilityCooldownToReplace abilityCoolDownToReplace = FindObjectOfType<AbilityCooldownToReplace>();
			abilityCoolDownToReplace.GetComponent<Image>().sprite = _currentAbility._icon;
		}

		_playerCanUseAbilities = true;

		if(_lastLevel == _currentLevel)
        {
			_player.GetComponent<HealthComponent>()._currentHealth = _currentPlayerHealth;
        }

		Debug.Log("neededEnemyKill ( " + _neededEnemyKill + " ) = enemySpawner.MaxAmount ( " + _enemySpawner._enemySpawnerData._maxEnemyCount + " )");
	}

	private void Update()
	{
		if (SceneManager.GetActiveScene().name == "SCENE_Main_Menu" || SceneManager.GetActiveScene().name == "SCENE_Init" || SceneManager.GetActiveScene().name.StartsWith("SCENE_Test") || SceneManager.GetActiveScene().name == "SCENE_Weapon_Crafting") 
			return;

		// Zeit abgelaufen

		if (!_hasWon)
			if (!_hasLost)
				if(!SceneManager.GetActiveScene().name.Contains("Boss"))
					_currentTimeToSurvive -= Time.deltaTime;
		if (!_hasWon && _currentTimeToSurvive <= 0 && _winningCondition == WinningCondition.SurviveForTime)
		{
			_hasWon = true;

			if(_currentLevelArray < _maxLevels)
			{
				_currentTimeToSurvive = GameManagerValues[_currentLevelArray]._timeToSurvive;
			}
			else
			{
				_currentTimeToSurvive = GameManagerValues[_maxLevels]._timeToSurvive;
			}

			RoundWon();
		}
	}

	public void EnemyDied()
	{
		_neededEnemyKill--;
		_enemyCount--;
		_enemiesKilled++;

		// alle Gegner get�tet

		if (!_hasWon && _neededEnemyKill == 0 && _winningCondition == WinningCondition.KillAllEnemies)
		{
			_hasWon = true;
			RoundWon();
		}

		// einen bestimmten Gegner get�tet

		if (!_hasWon && killedBoss && _winningCondition == WinningCondition.KillSpecificEnemy)
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

		List<WeaponPart> rewards = new List<WeaponPart>();
		for (int i = 0; i < _numberOfRewards; i++)
		{
			rewards.Add(RewardManager.Instance.GetRandomReward());
		}

		UIManager.Instance.ShowLevelEndScreen(LevelStatus.Won);
		UIManager.Instance.DisplayRewards(rewards);

		_currentLevel += 1;
		_currentLevelArray = _currentLevel - 1;

		// UIManager.Instance.ShowWaveEndScreen(LevelStatus.Won);

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

	private void MovePlayerToRandomPos(Vector3 spawnPos)
    {
		_player.transform.position = spawnPos;
	}

	private Vector3 DetermineRandomSpawnLocation()
	{
		Vector3 spawnPos = Vector3.zero;

		if (_enemySpawner._levelZone.Count > 0)
		{
			int zoneNumber = Random.Range(0, _enemySpawner._levelZone.Count);
			spawnPos = _enemySpawner._levelZone[zoneNumber].transform.position;
		}

		NavMeshHit nv_hit;
		if (NavMesh.SamplePosition(spawnPos, out nv_hit, Mathf.Infinity, 2))
		{
			spawnPos = nv_hit.position;
		}

		RaycastHit hit;
		if(Physics.Raycast(spawnPos, Vector3.down, out hit, Mathf.Infinity, _player.GetComponent<PlayerCharacter>().GroundLayer))
		{
			spawnPos = new Vector3(spawnPos.x, spawnPos.y - hit.distance, spawnPos.z);
        }

		return spawnPos;
	}
}
