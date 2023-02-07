using UnityEngine;
using UnityEngine.UI;

public class UIImageFillAmountWaveProgress : UIImageFillAmount
{
    private EnemySpawner _EnemySpawner;

    public Text EnemiesKilledText;
    public Text ScoreText;
    public Text LevelTimer;

    private int _enemiesKilled;

    public override void Awake()
    {
        _EnemySpawner = GameObject.FindGameObjectWithTag("EnemySpawner").GetComponent<EnemySpawner>();

        base.Awake();
    }

    public void Start()
    {
        _maxValue = _EnemySpawner.EnemyMaxAmount;
    }

    public override void FixedUpdate()
    {
        // maxValue Update falls es im Level verändert wird
        _enemiesKilled = (int)(_maxValue - Mathf.Abs(GameManager.Instance._neededEnemyKill));

        // _currentValue = _EnemySpawner.EnemiesThatHaveSpawned;
        _currentValue = _enemiesKilled;

        if(GameManager.Instance._timeToSurvive <= 0)
        {
            float tempTimer = Mathf.Abs(GameManager.Instance._timeToSurvive);

            float minutes = Mathf.FloorToInt(tempTimer / 60);
            float seconds = Mathf.FloorToInt(tempTimer % 60);
            float milliseconds = tempTimer * 1000;
            milliseconds = (milliseconds % 1000);

            LevelTimer.text = string.Format("{0:00}:{1:00}.{2:000}", minutes, seconds, milliseconds);
        }
        EnemiesKilledText.text = _enemiesKilled + " / " + _maxValue;
        ScoreText.text = GameManager.Instance._currentScore.ToString("0000");

        base.FixedUpdate();
    }
}
