using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedRobot_State_Idle : AI_State_Idle
{
    public override void Enter(AI_Agent agent)
    {
        if (agent._followDecoy)
        {
            _followPosition = agent._decoyTransform.position;

            if (agent._obstacleAgent.enabled)
            {
                agent._obstacleAgent.SetDestination(_followPosition);
            }
            else
            {
                agent._navMeshAgent.SetDestination(_followPosition);
            }
        }
        else
        {
            _followPosition = agent._playerTransform.position;

            if (agent._obstacleAgent.enabled)
            {
                agent._obstacleAgent.SetDestination(_followPosition);
            }
            else
            {
                agent._navMeshAgent.SetDestination(_followPosition);
            }
        }

        _followTimer = 0;
    }

    public override void Update(AI_Agent agent)
    {
        if (!agent._navMeshAgent.enabled)
        {
            return;
        }

        agent._navMeshAgent.SetDestination(agent.transform.position);

        if (agent._followDecoy)
        {
            _followPosition = agent._decoyTransform.position;
        }
        else
        {
            _followPosition = agent._playerTransform.position;
        }

        float distance = Vector3.Distance(agent.transform.position, _followPosition);

        RaycastHit hit;
        if (Physics.Raycast(agent._rangedRobot.ProjectilePoint.transform.position, (_followPosition + new Vector3(0, 0.5f, 0) - agent._rangedRobot.ProjectilePoint.transform.position), out hit, distance, agent._groundLayer))
        {
            if (distance < agent._enemyData._retreatRange)
            {
                agent._animator.SetBool("isRetreating", true);
                agent._animator.SetBool("isAttacking", false);
                agent._animator.SetBool("isChasing", false);
                agent._stateMachine.ChangeState(AI_StateID.Retreat);
            }
        }
        else
        {
            if (distance < agent._enemyData._attackRange && distance > agent._enemyData._retreatRange)
            {
                agent._animator.SetBool("isAttacking", true);
                agent._animator.SetBool("isChasing", false);
                agent._animator.SetBool("isRetreating", false);
                agent._stateMachine.ChangeState(AI_StateID.Attack);
            }
            else if (distance < agent._enemyData._retreatRange)
            {
                agent._animator.SetBool("isRetreating", true);
                agent._animator.SetBool("isAttacking", false);
                agent._animator.SetBool("isChasing", false);
                agent._stateMachine.ChangeState(AI_StateID.Retreat);
            }
        }

        _followTimer += Time.deltaTime;

        if (_followTimer >= agent._enemyData._followTime)
        {
            agent._animator.SetBool("isChasing", true);
            agent._stateMachine.ChangeState(AI_StateID.ChasePlayer);
        }
    }

    public override void Exit(AI_Agent agent)
    {

    }
}
