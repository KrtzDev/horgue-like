using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RangedRobot_State_Retreat : AI_State_Retreat
{
    private AI_Agent_RangedRobot _rangedRobot;

    public override void Enter(AI_Agent agent)
    {
        base.Enter(agent);

        _rangedRobot = agent as AI_Agent_RangedRobot;

        agent.Animator.SetBool("isRetreating", true);
        agent.NavMeshAgent.SetDestination(agent.transform.position);
        _retreatDistance = Random.Range(_enemy._enemyData._retreatRange + 1, _enemy._enemyData._attackRange - 1);

        if (agent.FollowDecoy)
        {
            _rangedRobot._followPosition = agent.DecoyTransform.position;
        }
        else
        {
            _rangedRobot._followPosition = agent.PlayerTransform.position;
        }

        Vector3 dirToPlayer = agent.transform.position - _rangedRobot._followPosition;
        _retreatPosition = agent.transform.position + dirToPlayer;

        if (NavMesh.SamplePosition(_retreatPosition, out NavMeshHit hit, 1f, agent.NavMeshAgent.areaMask))
        {
            _retreatPosition = hit.position;
        }
    }

    public override void Update(AI_Agent agent)
    {
        if (!agent.NavMeshAgent.enabled)
        {
            return;
        }

        if (agent.FollowDecoy)
        {
            _rangedRobot._followPosition = agent.DecoyTransform.position;
        }
        else
        {
            _rangedRobot._followPosition = agent.PlayerTransform.position;
        }

        float distanceToPlayer = Vector3.Distance(agent.transform.position, _rangedRobot._followPosition);

        if (distanceToPlayer > _retreatDistance && distanceToPlayer < _enemy._enemyData._attackRange)
        {
            agent.StateMachine.ChangeState(AI_StateID.ChasePlayer);
        }
        else
        {
            if (agent.ObstacleAgent.enabled && agent.enabled)
            {
                Vector3 dirToPlayer = agent.transform.position - _rangedRobot._followPosition;

                if (NavMesh.SamplePosition(agent.transform.position + dirToPlayer, out NavMeshHit hit, 2f, agent.NavMeshAgent.areaMask))
                {
                    _retreatPosition = hit.position;
                }
                else
                {
                    _retreatPosition = agent.transform.position;
                    agent.StateMachine.ChangeState(AI_StateID.Idle);
                    return;
                }

                agent.ObstacleAgent.SetDestination(_retreatPosition);
            }
            else if (agent.NavMeshAgent.enabled && agent.enabled)
            {
                Vector3 dirToPlayer = agent.transform.position - _rangedRobot._followPosition;

                if (NavMesh.SamplePosition(agent.transform.position + dirToPlayer, out NavMeshHit hit, 2f, agent.NavMeshAgent.areaMask))
                {
                    _retreatPosition = hit.position;
                }
                else
                {
                    _retreatPosition = agent.transform.position;
                    agent.StateMachine.ChangeState(AI_StateID.Idle);
                    return;
                }

                agent.NavMeshAgent.SetDestination(_retreatPosition);
            }
        }
    }

    public override void Exit(AI_Agent agent)
    {
        agent.Animator.SetBool("isRetreating", false);
    }


}
