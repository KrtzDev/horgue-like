using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PasuKan_State_Attack : AI_State_Attack
{
    private AI_Agent_PasuKan _pasuKan;
    public override void Enter(AI_Agent agent)
    {
        base.Enter(agent);

        _pasuKan = agent as AI_Agent_PasuKan;

        agent.SetTarget(agent, agent.transform.position);
    }

    public override void Update(AI_Agent agent)
    {
        if(agent.Animator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
        {
        }
        else
        {
            float distance = Vector3.Distance(agent.transform.position, _pasuKan._followPosition);

            if (distance > _enemy._enemyData._attackRange)
            {
                agent.StateMachine.ChangeState(AI_StateID.ChasePlayer);
            }
            else
            {
                agent.StateMachine.ChangeState(AI_StateID.Idle);
            }
        }
    }

    public override void Exit(AI_Agent agent)
    {

    }
}
