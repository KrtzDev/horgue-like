using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rikayon_State_Attack : AI_State_Attack
{
    private AI_Agent_Rikayon _rikayon;

    public override void Enter(AI_Agent agent)
    {
        base.Enter(agent);

        _rikayon = agent as AI_Agent_Rikayon;

        _followPosition = agent.transform.position;

        agent.SetTarget(agent, _followPosition);
    }

    public override void Update(AI_Agent agent)
    {
        if (agent._animator.GetCurrentAnimatorStateInfo(0).IsName("Attack") || agent._animator.GetCurrentAnimatorStateInfo(0).IsName("Intimidate"))
        {

        }
        else
        {
            agent._animator.SetBool("isIntimidating", false);
            agent._stateMachine.ChangeState(AI_StateID.Idle);
        }
    }

    public override void Exit(AI_Agent agent)
    {
        agent._animator.SetBool("isAttacking", false);
    }
}
