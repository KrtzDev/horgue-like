using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sniper_State_Attack : AI_State_Attack
{
    public override void Enter(AI_Agent agent)
    {
        agent._animator.SetBool("isAttacking", true);
        agent._navMeshAgent.SetDestination(agent.transform.position);
        agent._navMeshAgent.enabled = false;
    }

    public override void Update(AI_Agent agent)
    {
        if (!agent._followDecoy)
        {
            _followPosition = agent._playerTransform.position;
        }
        else
        {
            _followPosition = agent._decoyTransform.position;
        }

        agent.transform.LookAt(_followPosition);

        if (agent._attackTimer < 0)
        {
            agent._attackTimer = agent._enemyData._attackSpeed;
            agent._sniper.TargetDirection = (_followPosition - agent.transform.position).normalized;
            agent._animator.SetTrigger("shoot");
            agent._animator.SetBool("isShooting", true);
        }
        else if (!agent._animator.GetBool("isShooting"))
        {
            agent._attackTimer -= Time.deltaTime;

            float distance = Vector3.Distance(agent._animator.transform.position, _followPosition);

            if (distance > agent._enemyData._attackRange)
            {
                agent._animator.SetBool("isAttacking", false);
                agent._stateMachine.ChangeState(AI_StateID.Idle);
            }
            else if (distance < agent._enemyData._retreatRange)
            {
                agent._animator.SetBool("isAttacking", false);
                agent._animator.SetBool("isChasing", false);
                agent._stateMachine.ChangeState(AI_StateID.Idle);
            }
        }
    }

    public override void Exit(AI_Agent agent)
    {
        agent._animator.SetBool("isAttacking", false);
        agent._navMeshAgent.enabled = true;
    }
}
