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

    public void SetAgentActive()
    {
        _rikayon.GetComponent<AI_Agent>().enabled = true;
    }

    public void CanTakeDamageActive()
    {
        _rikayon.GetComponent<EnemyHealthComponent>().CanTakeDamageActive();
    }

    public void SetHealthToPercentOfMax1()
    {
        _rikayon.GetComponent<AI_Agent_Enemy>().SetHealthToPercentOfMax(60); // value from 0 to 100
    }

    public void SetHealthToPercentOfMax2()
    {
        _rikayon.GetComponent<AI_Agent_Enemy>().SetHealthToPercentOfMax(30); // value from 0 to 100
    }

    public void ResetFirstSpitAttack()
    {
        _rikayon.GetComponent<AI_Agent_Rikayon>().ResetFirstSpitAttack();
    }
}
