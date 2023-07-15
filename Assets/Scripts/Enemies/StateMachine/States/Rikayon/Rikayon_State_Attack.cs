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
    }

    public override void Update(AI_Agent agent)
    {
        if (agent._animator.GetCurrentAnimatorStateInfo(0).IsName("Attack" + _rikayon._currentAttackNumber))
        {

        }
        else
        {
            agent._animator.SetBool("isAttacking", false);
            agent._animator.SetBool("isChasing", true);
            agent._stateMachine.ChangeState(AI_StateID.ChasePlayer);
        }
    }

    public override void Exit(AI_Agent agent)
    {

    }
}
