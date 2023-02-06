using UnityEngine;
using UnityEngine.UI;

public class UIImageFillAmountWaveProgress : UIImageFillAmount
{
    private EnemySpawner _EnemySpawner;

    private GameManager _gameManager;

    public Text EnemiesKilledText;
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

        _gameManager = FindObjectOfType<GameManager>();
    }

    public override void FixedUpdate()
    {
        // maxValue Update falls es im Level verändert wird
        _enemiesKilled = (int)(_maxValue - Mathf.Abs(_gameManager._neededEnemyKill));

        // _currentValue = _EnemySpawner.EnemiesThatHaveSpawned;
        _currentValue = _enemiesKilled;

        if(_gameManager._timeToSurvive <= 0)
        {
            float tempTimer = Mathf.Abs(_gameManager._timeToSurvive);

            float minutes = Mathf.FloorToInt(tempTimer / 60);
            float seconds = Mathf.FloorToInt(tempTimer % 60);
            float milliseconds = tempTimer * 1000;
            milliseconds = (milliseconds % 1000);

            LevelTimer.text = string.Format("{0:00}:{1:00}.{2:000}", minutes, seconds, milliseconds);
        }
        EnemiesKilledText.text = _enemiesKilled + " / " + _maxValue;

        base.FixedUpdate();
    }
}
