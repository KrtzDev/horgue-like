using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedRobot_State_ChasePlayer : AI_State_ChasePlayer
{
    public override void Enter(AI_Agent agent)
    {
        agent._animator.SetBool("isChasing", true);
        agent._attackTimer = agent._enemyData._attackSpeed;
    }

    public override void Update(AI_Agent agent)
    {
        if (!agent._navMeshAgent.enabled)
        {
            return;
        }

        _timer -= Time.deltaTime;

        if (_timer < 0f)
        {
            if (agent._navMeshAgent.pathStatus != UnityEngine.AI.NavMeshPathStatus.PathPartial)
            {
                if (!agent._useMovementPrediction)
                {
                    if (agent._followDecoy)
                    {
                        _followPosition = agent._decoyTransform.position;
                        SetTarget(agent);
                    }
                    else
                    {
                        _followPosition = agent._playerTransform.position;
                        SetTarget(agent);
                    }
                }
                else
                {
                    if (agent._followDecoy)
                    {
                        _followPosition = agent._decoyTransform.position;
                        SetTarget(agent);
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

                        SetTarget(agent);
                    }
                }

                StartRotating(agent);
            }

            _timer = _maxTime;
        }

        float distance = Vector3.Distance(agent.transform.position, _followPosition);
        CheckForBehaviour(agent, distance);
    }

    public override void Exit(AI_Agent agent)
    {
        agent._animator.SetBool("isChasing", false);
    }

    private void SetTarget(AI_Agent agent)
    {
        if (agent._obstacleAgent.enabled)
        {
            agent._obstacleAgent.SetDestination(_followPosition);
        }
        else
        {
            agent._navMeshAgent.SetDestination(_followPosition);
        }
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
        if (Physics.Raycast(agent._rangedRobot.ProjectilePoint.transform.position, (_followPosition + new Vector3(0, 0.5f, 0) - agent._rangedRobot.ProjectilePoint.transform.position), out hit, distance, agent._groundLayer))
        {
            if (distance < agent._enemyData._retreatRange)
            {
                agent._animator.SetBool("isRetreating", true);
                agent._animator.SetBool("isAttacking", false);
                agent._animator.SetBool("isChasing", false);
                agent._stateMachine.ChangeState(AI_StateID.Retreat);
                agent._rb.velocity = Vector3.zero;
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
                agent._rb.velocity = Vector3.zero;
            }
            else if (distance < agent._enemyData._retreatRange)
            {
                agent._animator.SetBool("isRetreating", true);
                agent._animator.SetBool("isAttacking", false);
                agent._animator.SetBool("isChasing", false);
                agent._stateMachine.ChangeState(AI_StateID.Retreat);
                agent._rb.velocity = Vector3.zero;
            }
        }
    }
}
