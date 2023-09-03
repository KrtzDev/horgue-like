using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AI_State_ChasePlayer : AI_State
{
    protected Coroutine LookCoroutine;

    public float _timer;
    public float _maxTime = 0.0f;

    public AI_Agent_Enemy _enemy;

    public AI_StateID GetID()
    {
        return AI_StateID.ChasePlayer;
    }

    public virtual void Enter(AI_Agent agent)
    {
        _enemy = agent as AI_Agent_Enemy;
    }

    public virtual void Update(AI_Agent agent)
    {

    }

    public virtual void Exit(AI_Agent agent)
    {
        
    }
}
