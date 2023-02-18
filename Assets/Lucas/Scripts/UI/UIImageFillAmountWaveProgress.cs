using UnityEngine;
using UnityEngine.UI;

public class UIImageFillAmountWaveProgress : UIImageFillAmount
{
    private EnemySpawner _EnemySpawner;

    public Text EnemiesKilledText;
    public Text CurrentLevelWave;
    public Text ScoreText;
    public Text LevelTimer;

    private int _enemiesKilled;
    private int _maxEnemiesAmount;

    public override void Awake()
    {
        _EnemySpawner = GameObject.FindGameObjectWithTag("EnemySpawner").GetComponent<EnemySpawner>();

        base.Awake();
    }

    public void Start()
    {
		_maxEnemiesAmount = _EnemySpawner.EnemyMaxAmount;

        CurrentLevelWave.text = "Level: " + GameManager.Instance._currentLevel + " -- Wave: " + GameManager.Instance._currentWave;
	}

    public override void FixedUpdate()
    {
		if (!GameManager.Instance) return;

		if (_maxValue == 0)
			_maxValue = Mathf.Abs(GameManager.Instance._currentTimeToSurvive);

		// maxValue Update falls es im Level verändert wird
		_enemiesKilled = (int)(_maxEnemiesAmount - Mathf.Abs(GameManager.Instance._neededEnemyKill));

        // _currentValue = _EnemySpawner.EnemiesThatHaveSpawned;

        if(GameManager.Instance._currentTimeToSurvive >= 0)
        {
            float tempTimer = Mathf.Abs(GameManager.Instance._currentTimeToSurvive);
			_currentValue = tempTimer;

            float minutes = Mathf.FloorToInt(tempTimer / 60);
            float seconds = Mathf.FloorToInt(tempTimer % 60);
            float milliseconds = tempTimer * 1000;
            milliseconds = (milliseconds % 1000);

            LevelTimer.text = string.Format("{0:00}:{1:00}.{2:000}", minutes, seconds, milliseconds);
        }
        EnemiesKilledText.text = _enemiesKilled + " / " + _maxEnemiesAmount;
        ScoreText.text = GameManager.Instance._currentScore.ToString("0000");

        base.FixedUpdate();
    }
}
