using UnityEngine;
using UnityEngine.AI;

public class Enemy : PoolableObject
{
    [field: SerializeField] public EnemyMovement Movement { get; set; }
    [field: SerializeField] public NavMeshAgent Agent { get; set; }

    public override void OnDisable()
    {
        base.OnDisable();

        Agent.enabled = false;
    }
}
