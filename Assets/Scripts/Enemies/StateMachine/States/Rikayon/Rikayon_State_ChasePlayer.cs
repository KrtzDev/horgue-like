using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rikayon_State_ChasePlayer : AI_State_ChasePlayer
{
    AI_Agent_Rikayon _rikayon;

    public override void Enter(AI_Agent agent)
    {
        base.Enter(agent);

        agent.Animator.SetBool("isChasing", true);

        _rikayon = agent as AI_Agent_Rikayon;
    }

    public override void Update(AI_Agent agent)
    {
        if (!agent.NavMeshAgent.enabled)
        {
            return;
        }

        _timer -= Time.deltaTime;

        float distance = Vector3.Distance(agent.transform.position, agent.PlayerTransform.position);
        CheckForAttack(_enemy, distance);

        if (_timer < 0f)
        {
            if (agent.NavMeshAgent.pathStatus != UnityEngine.AI.NavMeshPathStatus.PathPartial)
            {
                if (!agent.UseMovementPrediction)
                {
                    if (agent.FollowDecoy)
                    {
                        _rikayon._followPosition = agent.DecoyTransform.position;
                        agent.SetTarget(agent, _rikayon._followPosition);
                    }
                    else
                    {
                        _rikayon._followPosition = agent.PlayerTransform.position;
                        agent.SetTarget(agent, _rikayon._followPosition);
                    }
                }
                else
                {
                    if (agent.FollowDecoy)
                    {
                        _rikayon._followPosition = agent.DecoyTransform.position;
                        agent.SetTarget(agent, _rikayon._followPosition);
                    }
                    else
                    {
                        _rikayon._followPosition = agent.PlayerTransform.position + (agent.Player.GetComponent<PlayerMovement>().AverageVelocity * agent.MovementPredictionTime);

                        Vector3 directionToTarget = (_rikayon._followPosition - agent.transform.position).normalized;
                        Vector3 directionToPlayer = (agent.PlayerTransform.position - agent.transform.position).normalized;

                        float dot = Vector3.Dot(directionToPlayer, directionToTarget);
                        if (dot < agent.MovementPredictionThreshold)
                        {
                            _rikayon._followPosition = agent.PlayerTransform.position;
                        }

                        agent.SetTarget(agent, _rikayon._followPosition);
                    }
                }

                StartRotating(agent);
            }

            _timer = _maxTime;
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

        LookCoroutine = AI_Manager.Instance.StartCoroutine(AI_Manager.Instance.LookAtTarget(agent, _rikayon._followPosition, _maxTime));
    }

    private void CheckForAttack(AI_Agent agent, float distance)
    {
        if (agent.AttackTimer > 0)
            agent.AttackTimer -= Time.deltaTime;

        if (distance < _enemy._enemyData._attackRange && agent.AttackTimer <= 0)
        {
            agent.AttackTimer = _enemy._enemyData._attackSpeed;
            _rikayon._followPosition = agent.PlayerTransform.position;
            agent.transform.LookAt(_rikayon._followPosition);

            agent.StateMachine.ChangeState(AI_StateID.Attack);

            return;
        }
    }
}
