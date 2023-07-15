using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_State_Idle : AI_State
{
    public Vector3 _followPosition;
    public float _followTimer;

    public AI_Agent_Enemy _enemy;

    public AI_StateID GetID()
    {
        return AI_StateID.Idle;
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
