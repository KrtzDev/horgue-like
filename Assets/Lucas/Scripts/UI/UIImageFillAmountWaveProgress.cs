using UnityEngine;

public class UIImageFillAmountWaveProgress : UIImageFillAmount
{
    private EnemySpawner _EnemySpawner;

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
        _currentValue = _EnemySpawner.EnemiesThatHaveSpawned;

        base.FixedUpdate();
    }
}
