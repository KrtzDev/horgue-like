using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDeathState : EnemyState
{
    public EnemyStateID GetID()
    {
        return EnemyStateID.Death;
    }

    public void Enter(EnemyAgent agent)
    {
        agent.gameObject.SetActive(false);
    }

    public void Update(EnemyAgent agent)
    {

    }

    public void Exit(EnemyAgent agent)
    {

    }
}
