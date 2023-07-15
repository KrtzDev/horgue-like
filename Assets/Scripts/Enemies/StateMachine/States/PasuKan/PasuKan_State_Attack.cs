using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PasuKan_State_Attack : AI_State_Attack
{
    public override void Enter(AI_Agent_Enemy agent)
    {
    }

    public override void Update(AI_Agent_Enemy agent)
    {
        if(agent._animator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
        {
            
        }
        else
        {
            agent._animator.SetBool("isChasing", true);
            agent._stateMachine.ChangeState(AI_StateID.ChasePlayer);
        }
    }

    public override void Exit(AI_Agent_Enemy agent)
    {

    }
}
