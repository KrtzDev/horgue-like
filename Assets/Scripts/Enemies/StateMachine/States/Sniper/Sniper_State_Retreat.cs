using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Sniper_State_Retreat : AI_State_Retreat
{
    public AI_Agent_Sniper _sniper;

    public override void Enter(AI_Agent agent)
    {
        base.Enter(agent);

        _sniper = agent as AI_Agent_Sniper;

        agent._animator.SetBool("isRetreating", true);
        agent._navMeshAgent.SetDestination(agent.transform.position);
        _retreatDistance = Random.Range(_enemy._enemyData._retreatRange + 1, _enemy._enemyData._attackRange - 1);

        if (agent._followDecoy)
        {
            _sniper._followPosition = agent._decoyTransform.position;
        }
        else
        {
            _sniper._followPosition = agent._playerTransform.position;
        }

        Vector3 dirToPlayer = agent.transform.position - _sniper._followPosition;
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

        if (agent._followDecoy)
        {
            _sniper._followPosition = agent._decoyTransform.position;
        }
        else
        {
            _sniper._followPosition = agent._playerTransform.position;
        }

        float distanceToPlayer = Vector3.Distance(agent.transform.position, _sniper._followPosition);

        if (distanceToPlayer > _retreatDistance && distanceToPlayer < _enemy._enemyData._attackRange)
        {
            agent._stateMachine.ChangeState(AI_StateID.ChasePlayer);
        }
        else
        {
            if (agent._obstacleAgent.enabled && agent.enabled)
            {
                Vector3 dirToPlayer = agent.transform.position - _sniper._followPosition;
                _retreatPosition = agent.transform.position + dirToPlayer;

                if (NavMesh.SamplePosition(_retreatPosition, out NavMeshHit hit, 1f, agent._navMeshAgent.areaMask))
                {
                    _retreatPosition = hit.position;
                }

                agent._obstacleAgent.SetDestination(_retreatPosition);
            }
            else if (agent._navMeshAgent.enabled && agent.enabled)
            {
                Vector3 dirToPlayer = agent.transform.position - _sniper._followPosition;
                _retreatPosition = agent.transform.position + dirToPlayer;

                if (NavMesh.SamplePosition(_retreatPosition, out NavMeshHit hit, 1f, agent._navMeshAgent.areaMask))
                {
                    _retreatPosition = hit.position;
                }

                agent._navMeshAgent.SetDestination(_retreatPosition);
            }
        }
    }

    public override void Exit(AI_Agent agent)
    {
        agent._animator.SetBool("isRetreating", false);
    }
}
