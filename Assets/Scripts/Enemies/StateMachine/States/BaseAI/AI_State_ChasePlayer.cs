using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AI_State_ChasePlayer : AI_State
{
    protected Coroutine LookCoroutine;

    public Vector3 _followPosition;
    public float _timer;
    public float _maxTime = 0.1f;

    public AI_StateID GetID()
    {
        return AI_StateID.ChasePlayer;
    }

    public virtual void Enter(AI_Agent_Enemy agent)
    {
    }

    public virtual void Update(AI_Agent_Enemy agent)
    {

    }

    public virtual void Exit(AI_Agent_Enemy agent)
    {
        
    }
}
