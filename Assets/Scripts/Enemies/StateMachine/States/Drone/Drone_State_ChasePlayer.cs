using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drone_State_ChasePlayer : AI_State_ChasePlayer
{
    private AI_Agent_Drone _drone;

    public override void Enter(AI_Agent agent)
    {
        base.Enter(agent);

        _drone = agent as AI_Agent_Drone;

        agent._animator.SetBool("isChasing", true);
    }

    public override void Update(AI_Agent agent)
    {
        if (!agent._navMeshAgent.enabled)
        {
            return;
        }

        if (!agent._useMovementPrediction)
        {
            if (agent._followDecoy)
            {
                _followPosition = agent._decoyTransform.position;
                agent.SetTarget(agent, _followPosition);
            }
            else
            {
                _followPosition = agent._playerTransform.position;
                agent.SetTarget(agent, _followPosition);
            }
        }
        else
        {
            if (agent._followDecoy)
            {
                _followPosition = agent._decoyTransform.position;
                agent.SetTarget(agent, _followPosition);
            }
            else
            {
                _followPosition = agent._playerTransform.position + (agent._player.GetComponent<PlayerMovement>().AverageVelocity * agent._movementPredictionTime);

                Vector3 directionToTarget = (_followPosition - agent.transform.position).normalized;
                Vector3 directionToPlayer = (agent._playerTransform.position - agent.transform.position).normalized;

                float dot = Vector3.Dot(directionToPlayer, directionToTarget);
                if (dot < agent._movementPredictionThreshold)
                {
                    _followPosition = agent._playerTransform.position;
                }

                agent.SetTarget(agent, _followPosition);
            }
        }

        float distance = Vector3.Distance(agent.transform.position, _followPosition);
        CheckForBehaviour(agent, distance);
    }

    public override void Exit(AI_Agent agent)
    {
        agent._animator.SetBool("isChasing", false);
    }

    private void StartRotating(AI_Agent agent)
    {
        if (LookCoroutine != null)
        {
            AI_Manager.Instance.StopCoroutine(LookCoroutine);
        }

        LookCoroutine = AI_Manager.Instance.StartCoroutine(AI_Manager.Instance.LookAtTarget(agent, _followPosition, _maxTime));
    }

    private void CheckForBehaviour(AI_Agent agent, float distance)
    {
        RaycastHit hit;
        if (Physics.Raycast(_drone.ProjectilePoint.transform.position, (_followPosition + new Vector3(0, 0.5f, 0) - _drone.ProjectilePoint.transform.position), out hit, distance, agent._groundLayer))
        {

        }
        else
        {
            if (distance < _enemy._enemyData._attackRange)
            {
                agent._stateMachine.ChangeState(AI_StateID.Attack);
            }
        }
    }
}
