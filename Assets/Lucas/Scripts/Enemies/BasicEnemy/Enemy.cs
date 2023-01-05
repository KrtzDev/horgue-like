using UnityEngine;
using UnityEngine.AI;

public class Enemy : PoolableObject
{
    [field: SerializeField] public NavMeshAgent Agent { get; set; }

    public bool _followDecoy;

    public override void OnDisable()
    {
        base.OnDisable();

        Agent.enabled = false;
    }
}
