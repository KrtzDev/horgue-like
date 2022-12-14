using UnityEngine;

public class UIImageFillAmountWaveProgress : UIImageFillAmount
{
    private EnemySpawner _EnemySpawner;

    public override void Awake()
    {
        _EnemySpawner = GameObject.FindGameObjectWithTag("EnemySpawner").GetComponent<EnemySpawner>();
        _maxValue = _EnemySpawner.EnemyWavesToSpawn * _EnemySpawner.EnemyWaveSize;

        base.Awake();
    }

    public override void FixedUpdate()
    {
        // maxValue Update falls es im Level ver?ndert wird
        _currentValue = _EnemySpawner.EnemiesThatHaveSpawned;

        base.FixedUpdate();
    }
}
