using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_State_Idle : AI_State
{
    public Vector3 _followPosition;
    public float _followTimer;

    public AI_StateID GetID()
    {
        return AI_StateID.Idle;
    }

    public virtual void Enter(AI_Agent agent)
    {

    }

    public virtual void Update(AI_Agent agent)
    {

    }

    public virtual void Exit(AI_Agent agent)
    {

    }
}