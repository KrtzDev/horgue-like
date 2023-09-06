using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.AI;
using System.Collections;

[DefaultExecutionOrder(-1)]
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
	public int maxLevels;

	private EnemySpawner _currentEnemySpawner;

	[Header("WinningCondition")]
	[SerializeField]
	private WinningCondition _winningCondition;

	[HideInInspector]
	public float _currentTimeToSurvive;

	private EnemySpawner _enemySpawner;
	public int _neededEnemyKill;
	public int _enemyCount;
	public int _enemiesKilled;
	public bool killedBoss;
	private bool _hasWon;
	private bool _hasLost;

	public int _currentLevel = 1;
	// private int _lastLevel;
	public int _currentLeveslArray;

	private int _numberOfRewards = 6;

	public bool _gameIsPaused;

	[Header("New Game Plus")]
	public bool _newGamePlus;
	public int firstLevelNumberInBuild;
	public int lastLevelNumberInBuild;
	public Vector2Int lastLevelNumbers;

	[Header("Player")]
	public GameObject _player;
	public bool _playerCanUseAbilities;
	public Ability _currentAbility;

	[SerializeField]
	public Inventory inventory;
	public ObjectPool<CollectibleAttractor> coinPool;
	[SerializeField] private CollectibleAttractor _coin;
	public ObjectPool<CollectibleAttractor> healthPackPool;
	[SerializeField] private CollectibleAttractor _healthPack;

	[Header("Boss Cheat")]
	public bool bossCheat;
	[Header("Boss Items")]
	public Ammunition amm1;
	public Ammunition amm2;
	public TriggerMechanism trigger1;
	public TriggerMechanism trigger2;
	public Grip grip1;
	public Grip grip2;
	public Magazine magazine1;
	public Magazine magazine2;
	public Barrel barrel1;
	public Barrel barrel2;
	public Sight sight1;
	public Sight sight2;

	private void Start()
	{
		SceneLoader.Instance.CompletedSceneLoad += OnCompletedSceneLoad;

		ResetGame();
	}

	private void ResetGame()
    {
		inventory.Wallet.Reset();
		inventory.ResetInventory();
		_currentLevel = 1;
		_gameIsPaused = false;
		_newGamePlus = false;
		lastLevelNumbers = Vector2Int.zero;
		bossCheat = false;
		StatsTracker.Instance.ResetAllStats();
	}

	private void OnCompletedSceneLoad()
	{
		if (SceneManager.GetActiveScene().name.Contains("04"))
		{
			AudioManager.Instance.PlaySound("Swamp");
		}
		else
		{
			AudioManager.Instance.StopSound("Swamp", 0.125f);
		}

		if (SceneManager.GetActiveScene().name == "SCENE_Weapon_Crafting")
		{
			return;
		}

		if (SceneManager.GetActiveScene().name == "SCENE_Main_Menu")
		{
			ResetGame();
			return;
		}

		if (SceneManager.GetActiveScene().name.Contains("Boss"))
        {
			_winningCondition = WinningCondition.KillSpecificEnemy;
		}
		else
        {
			_winningCondition = WinningCondition.SurviveForTime;
		 	killedBoss = false;
		}

		coinPool = ObjectPool<CollectibleAttractor>.CreatePool(_coin, 1000, null);
		healthPackPool = ObjectPool<CollectibleAttractor>.CreatePool(_healthPack, 100, null);

		StatsTracker.Instance.ResetLevelStats();

		_gameDataReader.GetGameData();
		_gameDataReader.SetGameData();

		if(_currentLevel - 1 < maxLevels)
        {
			_currentTimeToSurvive = GameManagerValues[_currentLevel - 1]._timeToSurvive;
			_currentEnemySpawner = FindObjectOfType<EnemySpawner>();
			_currentEnemySpawner._enemySpawnerData = GameManagerValues[maxLevels];
		}
		else
        {
			_currentTimeToSurvive = GameManagerValues[maxLevels]._timeToSurvive;
			_currentEnemySpawner = FindObjectOfType<EnemySpawner>();
			_currentEnemySpawner._enemySpawnerData = GameManagerValues[maxLevels];
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

		if (bossCheat)
		{
			WeaponHolster weaponHolster = FindObjectOfType<WeaponHolster>();
			BossCheat(weaponHolster);
			weaponHolster.Initialize();
		}
		else
        {
			WeaponHolster weaponHolster = FindObjectOfType<WeaponHolster>();
			weaponHolster.Initialize();
		}

		_currentAbility = null;

		if (_currentAbility != null)
        {
			AbilityCooldownToReplace abilityCoolDownToReplace = FindObjectOfType<AbilityCooldownToReplace>();
			abilityCoolDownToReplace.GetComponent<Image>().sprite = _currentAbility._icon;
		}

		_playerCanUseAbilities = true;

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

			if(_currentLevel - 1 < maxLevels)
			{
				_currentTimeToSurvive = GameManagerValues[_currentLevel - 1]._timeToSurvive;
			}
			else
			{
				_currentTimeToSurvive = GameManagerValues[maxLevels]._timeToSurvive;
			}

			RoundWon();
		}
	}

	public void EnemyDied()
	{
		_neededEnemyKill--;
		_enemyCount--;
		_enemiesKilled++;

		StatsTracker.Instance.enemiesKilledLevel++;

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
		AudioManager.Instance.PlaySound("LevelConfirmation");
		StatsTracker.Instance.AddLevelStatsToTotal();

		Debug.Log("Round won");
		InputManager.Instance.CharacterInputActions.Disable();
		_playerCanUseAbilities = false;

		List<WeaponPart> rewards = new List<WeaponPart>();
		for (int i = 0; i < _numberOfRewards; i++)
		{
			rewards.Add(RewardManager.Instance.GetReward());
		}

		UIManager.Instance.DisplayRewards(rewards);

		_currentLevel += 1;

		// UIManager.Instance.ShowWaveEndScreen(LevelStatus.Won);

		EnemyStopFollowing();
		StartCoroutine(AttractCoins());
	}

	private IEnumerator AttractCoins()
    {
		bool coinMove = false;

		Debug.Log(coinPool.ActiveCount());

		while (coinPool.ActiveCount() > 0)
		{
			if (!coinMove)
			{
				Collider playerCollider = _player.GetComponent<Collider>();

				for (int i = 0; i < coinPool.Count; i++)
				{
					if(coinPool.GetObjectAtIndex(i).isActiveAndEnabled)
                    {
						coinPool.GetObjectAtIndex(i).GetComponent<Rigidbody>().useGravity = false;
						coinPool.GetObjectAtIndex(i).GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
						coinPool.GetObjectAtIndex(i).playerCollider = playerCollider;
						coinPool.GetObjectAtIndex(i).moveToPlayer = true;
						coinPool.GetObjectAtIndex(i).attractorSpeed *= 4f;
						coinPool.GetObjectAtIndex(i).GetComponentInChildren<Coin_Collectible>().givenScore = Mathf.RoundToInt(coinPool.GetObjectAtIndex(i).GetComponentInChildren<Coin_Collectible>().givenScore * 0.5f);
					}
				}

				coinMove = true;
			}
			yield return null;
		}

		UIManager.Instance.ShowLevelEndScreen(LevelStatus.Won);
	}

	private void EnemyStopFollowing()
	{
		_enemySpawner._enemyObjectPoolParent.SetActive(false);
	}

	private void RoundLost()
	{
		Debug.Log("Round Lost");
		StatsTracker.Instance.AddLevelStatsToTotal();
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

	private void BossCheat(WeaponHolster weaponHolster)
    {
			Ammunition newAmm = amm1;
			newAmm.levelObtained = _currentLevel - 1;
			RewardManager.Instance.ScaleWeaponPartToLevel(newAmm);

			Barrel newBarrel = barrel1;
			newBarrel.levelObtained = _currentLevel - 1;
			RewardManager.Instance.ScaleWeaponPartToLevel(newBarrel);

			Grip newGrip = grip1;
			newGrip.levelObtained = _currentLevel - 1;
			RewardManager.Instance.ScaleWeaponPartToLevel(newGrip);

			Magazine newMagazine = magazine1;
			newMagazine.levelObtained = _currentLevel - 1;
			RewardManager.Instance.ScaleWeaponPartToLevel(newMagazine);

			Sight newSight = sight1;
			newSight.levelObtained = _currentLevel - 1;
			RewardManager.Instance.ScaleWeaponPartToLevel(newSight);

			TriggerMechanism newTrigger = trigger1;
			newTrigger.levelObtained = _currentLevel - 1;
			RewardManager.Instance.ScaleWeaponPartToLevel(newTrigger);

			weaponHolster.weapons[0].ammunition = newAmm;
			weaponHolster.weapons[0].barrel = newBarrel;
			weaponHolster.weapons[0].grip = newGrip;
			weaponHolster.weapons[0].magazine = newMagazine;
			weaponHolster.weapons[0].sight = newSight;
			weaponHolster.weapons[0].triggerMechanism = newTrigger;

			newAmm = amm2;
			newAmm.levelObtained = _currentLevel - 1;
			RewardManager.Instance.ScaleWeaponPartToLevel(newAmm);

			newBarrel = barrel2;
			newBarrel.levelObtained = _currentLevel - 1;
			RewardManager.Instance.ScaleWeaponPartToLevel(newBarrel);

			newGrip = grip2;
			newGrip.levelObtained = _currentLevel - 1;
			RewardManager.Instance.ScaleWeaponPartToLevel(newGrip);

			newMagazine = magazine2;
			newMagazine.levelObtained = _currentLevel - 1;
			RewardManager.Instance.ScaleWeaponPartToLevel(newMagazine);

			newSight = sight2;
			newSight.levelObtained = _currentLevel - 1;
			RewardManager.Instance.ScaleWeaponPartToLevel(newSight);

			newTrigger = trigger2;
			newTrigger.levelObtained = _currentLevel - 1;
			RewardManager.Instance.ScaleWeaponPartToLevel(newTrigger);

			weaponHolster.weapons[1].ammunition = newAmm;
			weaponHolster.weapons[1].barrel = newBarrel;
			weaponHolster.weapons[1].grip = newGrip;
			weaponHolster.weapons[1].magazine = newMagazine;
			weaponHolster.weapons[1].sight = newSight;
			weaponHolster.weapons[1].triggerMechanism = newTrigger;
		
	}
}
