using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_State_Damage : AI_State
{
    AI_StateID AI_State.GetID()
    {
        return AI_StateID.Damage;
    }

    public virtual void Enter(AI_Agent agent)
    {
        agent._animator.SetTrigger("damage");
    }

    public virtual void Update(AI_Agent agent)
    {

    }

    public virtual void Exit(AI_Agent agent)
    {

    }
}
