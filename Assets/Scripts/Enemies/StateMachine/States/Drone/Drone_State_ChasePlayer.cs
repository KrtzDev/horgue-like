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

        agent.Animator.SetBool("isChasing", true);
    }

    public override void Update(AI_Agent agent)
    {
        float distance = Vector3.Distance(_drone.ProjectilePoint.transform.position, _drone._followPosition + new Vector3(0, 0.5f, 0));
        Debug.DrawLine(_drone.ProjectilePoint.transform.position, _drone._followPosition + new Vector3(0, 0.5f, 0), Color.yellow);
        CheckForBehaviour(agent, distance);

        if (!agent.NavMeshAgent.enabled)
        {
            return;
        }

        if (!agent.UseMovementPrediction)
        {
            if (agent.FollowDecoy)
            {
                _drone._followPosition = agent.DecoyTransform.position;
                agent.SetTarget(agent, new Vector3(_drone._followPosition.x, agent.transform.position.y, _drone._followPosition.z));
            }
            else
            {
                _drone._followPosition = agent.PlayerTransform.position;
                agent.SetTarget(agent, new Vector3(_drone._followPosition.x, agent.transform.position.y, _drone._followPosition.z));
            }
        }
        else
        {
            if (agent.FollowDecoy)
            {
                _drone._followPosition = agent.DecoyTransform.position;
                agent.SetTarget(agent, new Vector3(_drone._followPosition.x, agent.transform.position.y, _drone._followPosition.z));
            }
            else
            {
                _drone._followPosition = agent.PlayerTransform.position + (agent.Player.GetComponent<PlayerMovement>().AverageVelocity * agent.MovementPredictionTime);

                Vector3 directionToTarget = (_drone._followPosition - agent.transform.position).normalized;
                Vector3 directionToPlayer = (agent.PlayerTransform.position - agent.transform.position).normalized;

                float dot = Vector3.Dot(directionToPlayer, directionToTarget);
                if (dot < agent.MovementPredictionThreshold)
                {
                    _drone._followPosition = agent.PlayerTransform.position;
                }

                agent.SetTarget(agent, new Vector3(_drone._followPosition.x, agent.transform.position.y, _drone._followPosition.z));
            }
        }
    }

    public override void Exit(AI_Agent agent)
    {
        agent.Animator.SetBool("isChasing", false);
    }

    private void StartRotating(AI_Agent agent)
    {
        if (LookCoroutine != null)
        {
            AI_Manager.Instance.StopCoroutine(LookCoroutine);
        }

        LookCoroutine = AI_Manager.Instance.StartCoroutine(AI_Manager.Instance.LookAtTarget(agent, _drone._followPosition, _maxTime));
    }

    private void CheckForBehaviour(AI_Agent agent, float distance)
    {
        agent.AttackTimer -= Time.deltaTime;

        if (!Physics.Raycast(_drone.ProjectilePoint.transform.position, (_drone._followPosition + new Vector3(0, 0.5f, 0) - _drone.ProjectilePoint.transform.position).normalized, distance, agent.GroundLayer))
        {
            if (distance <= _enemy._enemyData._attackRange)
            {
                if(agent.AttackTimer <= 0)
                {
                    agent.StateMachine.ChangeState(AI_StateID.Attack);
                    return;
                }
            }
        }
    }
}
