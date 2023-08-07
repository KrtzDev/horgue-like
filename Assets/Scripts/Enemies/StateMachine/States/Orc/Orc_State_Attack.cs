using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Orc_State_Attack : AI_State_Attack
{
    public override void Enter(AI_Agent agent)
    {
        base.Enter(agent);

        agent.SetTarget(agent, agent.transform.position);
    }

    public override void Update(AI_Agent agent)
    {
        if (agent._animator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
        {
        }
        else
        {
            agent._animator.SetBool("isChasing", true);
            agent._stateMachine.ChangeState(AI_StateID.ChasePlayer);
        }
    }

    public override void Exit(AI_Agent agent)
    {

    }
}
