using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_State_Death : AI_State
{
    AI_StateID AI_State.GetID()
    {
        return AI_StateID.Death;
    }

    public virtual void Enter(AI_Agent agent)
    {
        agent.Animator.enabled = true;
        agent.Animator.SetBool("death", true);
        agent.Animator.SetTrigger("deathTrigger");
    }

    public virtual void Update(AI_Agent agent)
    {
    }

    public virtual void Exit(AI_Agent agent)
    {
       
    }

}
