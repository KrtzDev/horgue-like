using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Orc_State_Attack : AI_State_Attack
{
    private AI_Agent_Orc _orc;

    public override void Enter(AI_Agent agent)
    {
        base.Enter(agent);

        _orc = agent as AI_Agent_Orc;

        agent.SetTarget(agent, agent.transform.position);
    }

    public override void Update(AI_Agent agent)
    {
        if (agent._animator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
        {
        }
        else
        {
            float distance = Vector3.Distance(agent.transform.position, _orc._followPosition);

            if (distance > _enemy._enemyData._attackRange)
            {
                agent._stateMachine.ChangeState(AI_StateID.ChasePlayer);
            }
            else
            {
                agent._stateMachine.ChangeState(AI_StateID.Idle);
            }
        }
    }

    public override void Exit(AI_Agent agent)
    {

    }
}
