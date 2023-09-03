using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orc_State_ChasePlayer : AI_State_ChasePlayer
{
    private AI_Agent_Orc _orc;

    public override void Enter(AI_Agent agent)
    {
        base.Enter(agent);

        _orc = agent as AI_Agent_Orc;

        agent.Animator.SetBool("isChasing", true);
    }

    public override void Update(AI_Agent agent)
    {
        if (!agent.NavMeshAgent.enabled)
        {
            return;
        }

        _timer -= Time.deltaTime;

        if (_timer < 0f)
        {
            if (agent.NavMeshAgent.pathStatus != UnityEngine.AI.NavMeshPathStatus.PathPartial)
            {
                if (!agent.UseMovementPrediction)
                {
                    if (agent.FollowDecoy)
                    {
                        _orc._followPosition = agent.DecoyTransform.position;
                        agent.SetTarget(agent, _orc._followPosition);
                    }
                    else
                    {
                        _orc._followPosition = agent.PlayerTransform.position;
                        agent.SetTarget(agent, _orc._followPosition);
                    }
                }
                else
                {
                    if (agent.FollowDecoy)
                    {
                        _orc._followPosition = agent.DecoyTransform.position;
                        agent.SetTarget(agent, _orc._followPosition);
                    }
                    else
                    {
                        _orc._followPosition = agent.PlayerTransform.position + (agent.Player.GetComponent<PlayerMovement>().AverageVelocity * agent.MovementPredictionTime);

                        Vector3 directionToTarget = (_orc._followPosition - agent.transform.position).normalized;
                        Vector3 directionToPlayer = (agent.PlayerTransform.position - agent.transform.position).normalized;

                        float dot = Vector3.Dot(directionToPlayer, directionToTarget);
                        if (dot < agent.MovementPredictionThreshold)
                        {
                            _orc._followPosition = agent.PlayerTransform.position;
                        }

                        agent.SetTarget(agent, _orc._followPosition);
                    }
                }
            }

            _timer = _maxTime;
        }

        float distance = Vector3.Distance(agent.transform.position, agent.PlayerTransform.position);
        CheckForAttack(_enemy, distance);
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

        LookCoroutine = AI_Manager.Instance.StartCoroutine(AI_Manager.Instance.LookAtTarget(agent, _orc._followPosition, _maxTime));
    }

    private void CheckForAttack(AI_Agent agent, float distance)
    {
        if (distance < _enemy._enemyData._attackRange && agent.AttackTimer <= 0)
        {
            agent.AttackTimer = _enemy._enemyData._attackSpeed;
            agent.transform.LookAt(_orc._followPosition);
            agent.Animator.SetTrigger("attack");
            agent.Animator.SetBool("isChasing", false);
            agent.StateMachine.ChangeState(AI_StateID.Attack);
        }
        else if (distance < _enemy._enemyData._attackRange)
        {
            agent.StateMachine.ChangeState(AI_StateID.Idle);
        }
        else
        {
            agent.AttackTimer -= Time.deltaTime;
        }
    }
}
