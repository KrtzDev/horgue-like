using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : PoolableObject
{
    public EnemyMovement Movement;
    public NavMeshAgent Agent;

    public override void OnDisable()
    {
        base.OnDisable();

        Agent.enabled = false;
    }
}
