using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyStateID
{
    Idle,
    Death,
    ChasePlayer
}

public interface EnemyState
{
    EnemyStateID GetID();
    void Enter(EnemyAgent agent);
    void Update(EnemyAgent agent);
    void Exit(EnemyAgent agent);
}
