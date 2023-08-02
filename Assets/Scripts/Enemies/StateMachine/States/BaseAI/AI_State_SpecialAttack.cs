using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_State_SpecialAttack : AI_State
{
    AI_StateID AI_State.GetID()
    {
        return AI_StateID.SpecialAttack;
    }

    public AI_Agent_Enemy _enemy;

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
