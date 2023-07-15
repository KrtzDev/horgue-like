using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_State_Retreat : AI_State
{
    public Vector3 _followPosition;
    public Vector3 _retreatPosition;
    public float _retreatDistance;

    public AI_StateID GetID()
    {
        return AI_StateID.Retreat;
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
