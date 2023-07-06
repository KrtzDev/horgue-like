using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Sniper_State_Retreat : AI_State_Retreat
{
    public override void Enter(AI_Agent agent)
    {
        agent._animator.SetBool("isRetreating", true);
        agent._navMeshAgent.SetDestination(agent.transform.position);
        _retreatDistance = Random.Range(agent._enemyData._retreatRange + 1, agent._enemyData._attackRange - 1);

        if (agent._followDecoy)
        {
            _followPosition = agent._decoyTransform.position;
        }
        else
        {
            _followPosition = agent._playerTransform.position;
        }

        Vector3 dirToPlayer = agent.transform.position - _followPosition;
        _retreatPosition = agent.transform.position + dirToPlayer;

        if (NavMesh.SamplePosition(_retreatPosition, out NavMeshHit hit, 1f, agent._navMeshAgent.areaMask))
        {
            _retreatPosition = hit.position;
        }

    }

    public override void Update(AI_Agent agent)
    {
        if (!agent._navMeshAgent.enabled)
        {
            return;
        }

        agent.transform.LookAt(_retreatPosition);

        if (agent._obstacleAgent.enabled && agent.enabled)
        {
            agent._obstacleAgent.SetDestination(_followPosition);
        }
        else if (agent._navMeshAgent.enabled && agent.enabled)
        {
            agent._navMeshAgent.SetDestination(_followPosition);
        }

        float distanceToPlayer = Vector3.Distance(agent.transform.position, _followPosition);

        if (distanceToPlayer > _retreatDistance)
        {
            agent._animator.SetBool("isRetreating", false);
            agent._animator.SetBool("isAttacking", false);
            agent._animator.SetBool("isChasing", true);
            agent._navMeshAgent.SetDestination(agent.transform.position);
            agent._stateMachine.ChangeState(AI_StateID.Idle);
        }
    }

    public override void Exit(AI_Agent agent)
    {
        agent._animator.SetBool("isRetreating", false);
    }
}
