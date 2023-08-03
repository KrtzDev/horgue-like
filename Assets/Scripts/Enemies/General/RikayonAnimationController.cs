using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RikayonAnimationController : EnemyAnimationController
{
    private AI_Agent_Rikayon _rikayon;

    public override void Start()
    {
        base.Start();

        _rikayon = _enemyType as AI_Agent_Rikayon;
    }

    public void SprayAttack()
    {
        _rikayon.SprayAttack();
    }

    public void RadialSpikeAttack()
    {
        _rikayon.RadialSpikeAttack();
    }

    public void DestroySpawnPositionChilds()
    {
        _rikayon.DestroySpawnPositionChilds();
    }

    public void SpitAttack()
    {
        _rikayon.SpitAttack();
    }
}
