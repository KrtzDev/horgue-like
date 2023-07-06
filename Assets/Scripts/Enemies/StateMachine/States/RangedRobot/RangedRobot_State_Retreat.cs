using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedRobot_State_Retreat : AI_State_Retreat
{
    public override void Enter(AI_Agent agent)
    {
        agent._animator.SetBool("isRetreating", true);
        _retreatDistance = Random.Range(agent._enemyData._retreatRange + 1, agent._enemyData._attackRange - 1);
    }

    public override void Update(AI_Agent agent)
    {
        if (!agent._navMeshAgent.enabled)
        {
            return;
        }

        _followPosition = agent.transform.position;

        Vector3 dirToPlayer = agent.transform.position - _followPosition;
        Vector3 newPos = agent.transform.position + dirToPlayer;

        agent.transform.LookAt(newPos);
        if (agent._obstacleAgent.enabled)
            agent._obstacleAgent.SetDestination(newPos);

        float distance = Vector3.Distance(agent.transform.position, _followPosition);

        if (distance > _retreatDistance)
        {
            agent._animator.SetBool("isRetreating", false);
            agent._animator.SetBool("isAttacking", false);
            agent._animator.SetBool("isChasing", true);
            agent._stateMachine.ChangeState(AI_StateID.ChasePlayer);
        }
    }

    public override void Exit(AI_Agent agent)
    {
        agent._animator.SetBool("isRetreating", false);
    }
}
