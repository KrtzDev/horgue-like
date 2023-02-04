using UnityEngine;

public class UIImageFillAmountWaveProgress : UIImageFillAmount
{
    private EnemySpawner _EnemySpawner;

    public override void Awake()
    {
        _EnemySpawner = GameObject.FindGameObjectWithTag("EnemySpawner").GetComponent<EnemySpawner>();
        _maxValue = _EnemySpawner._enemySpawnerData._enemyWavesToSpawn * _EnemySpawner._enemySpawnerData._enemyWaveSize;

        base.Awake();
    }

    public override void FixedUpdate()
    {
        // maxValue Update falls es im Level verändert wird
        _currentValue = _EnemySpawner.EnemiesThatHaveSpawned;

        base.FixedUpdate();
    }
}
