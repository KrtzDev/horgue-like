using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIImageFillAmountWaveProgress : UIImageFillAmount
{
    private EnemySpawner _enemySpawner;

    public TextMeshProUGUI _enemiesKilledText;
    public TextMeshProUGUI _CurrentLevelWave;
    public TextMeshProUGUI _ScoreText;
    public TextMeshProUGUI _LevelTimer;

    private int _enemiesKilled;
    private int _maxEnemiesAmount;

    public override void Awake()
    {
        _enemySpawner = GameObject.FindGameObjectWithTag("EnemySpawner").GetComponent<EnemySpawner>();

        base.Awake();
    }

    public void Start()
    {
		_maxEnemiesAmount = _enemySpawner._enemySpawnerData._maxEnemyCount;

        _CurrentLevelWave.text = "Level: " + GameManager.Instance._currentLevel;
	}

    public override void OnGUI()
    {
		if (!GameManager.Instance) return;

		if (_maxValue == 0)
			_maxValue = Mathf.Abs(GameManager.Instance._currentTimeToSurvive);

		// maxValue Update falls es im Level verändert wird
		_enemiesKilled = (GameManager.Instance._enemiesKilled);

        // _currentValue = _EnemySpawner.EnemiesThatHaveSpawned;

        if(GameManager.Instance._currentTimeToSurvive >= 0)
        {
            float tempTimer = Mathf.Abs(GameManager.Instance._currentTimeToSurvive);
			_currentValue = tempTimer;

            /* float minutes = Mathf.FloorToInt(tempTimer / 60);
            float seconds = Mathf.FloorToInt(tempTimer % 60);
            float milliseconds = tempTimer * 1000;
            milliseconds = (milliseconds % 1000);

            LevelTimer.text = string.Format("{0:00}:{1:00}.{2:000}", minutes, seconds, milliseconds);
            */

            float seconds = Mathf.FloorToInt(tempTimer);
            float milliseconds = tempTimer * 1000;
            milliseconds = (milliseconds % 1000);

            _LevelTimer.text = string.Format("{0:00}.{1:000}", seconds, milliseconds);
        }

        // EnemiesKilledText.text = _enemiesKilled + " / " + _maxEnemiesAmount;
        _enemiesKilledText.text = _enemiesKilled.ToString("000");
		int money = GameManager.Instance.inventory.Wallet.GetMoneyAmount();
		_ScoreText.text = money.ToString("000");

        base.OnGUI();
    }
}
