using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_State_Attack : AI_State 
{
    protected Coroutine LookCoroutine;

    public Vector3 _followPosition;
    public AI_Agent_Enemy _enemy;
    public float _maxTime = 0.1f;

    public AI_StateID GetID()
    {
        return AI_StateID.Attack;
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
