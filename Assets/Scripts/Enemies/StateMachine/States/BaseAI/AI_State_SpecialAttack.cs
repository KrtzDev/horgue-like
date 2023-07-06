using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_State_SpecialAttack : AI_State
{
    AI_StateID AI_State.GetID()
    {
        return AI_StateID.SpecialAttack;
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
