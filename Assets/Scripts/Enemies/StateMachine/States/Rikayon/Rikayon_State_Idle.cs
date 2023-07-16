using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rikayon_State_Idle : AI_State_Idle
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
        if (!agent._navMeshAgent.enabled)
        {
            return;
        }

        agent._attackTimer -= Time.deltaTime;

        if (agent._animator.GetCurrentAnimatorStateInfo(0).IsName("Intimidate"))
        {

        }
        else
        {
            if (agent._followDecoy)
            {
                _followPosition = agent._decoyTransform.position;
            }
            else
            {
                _followPosition = agent._playerTransform.position;
            }

            float distance = Vector3.Distance(agent.transform.position, _followPosition);

            if (distance > _enemy._enemyData._attackRange + 1 || agent._attackTimer < 0)
            {
                agent._animator.SetBool("isChasing", true);
                agent._animator.SetBool("isIntimidating", false);
                agent._stateMachine.ChangeState(AI_StateID.ChasePlayer);
                return;
            }
        }
    }

    public override void Exit(AI_Agent agent)
    {

    }
}
