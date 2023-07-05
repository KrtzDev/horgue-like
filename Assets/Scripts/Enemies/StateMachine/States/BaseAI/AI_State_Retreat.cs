using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_State_Retreat : AI_State
{
    public AI_StateID GetID()
    {
        return AI_StateID.Retreat;
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
