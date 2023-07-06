using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PasuKan_State_ChasePlayer : AI_State_ChasePlayer
{
    public override void Enter(AI_Agent agent)
    {
        agent._animator.SetBool("isChasing", true);
        agent._attackTimer = agent._enemyData._attackSpeed;
    }

    public override void Update(AI_Agent agent)
    {
        if(!agent._navMeshAgent.enabled)
        {
            return;
        }

        _timer -= Time.deltaTime;

        if(_timer < 0f)
        {
            if (agent._navMeshAgent.pathStatus != UnityEngine.AI.NavMeshPathStatus.PathPartial)
            {
                if(!agent._useMovementPrediction)
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
                        if(dot < agent._movementPredictionThreshold)
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

        float distance = Vector3.Distance(agent.transform.position, agent._playerTransform.position);
        // CheckForJumpAttack(agent, distance);
        CheckForAttack(agent, distance);
    }

    public override void Exit(AI_Agent agent)
    {
        agent._animator.SetBool("isChasing", false);
    }

    private void SetTarget(AI_Agent agent)
    {
        if (agent._obstacleAgent.enabled && agent.enabled)
        {
            agent._obstacleAgent.SetDestination(_followPosition);
        }
        else if (agent._navMeshAgent.enabled && agent.enabled)
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

    private void CheckForJumpAttack(AI_Agent agent, float distance)
    {
        float random = Random.Range(0f, 100f);

        if (random <= agent._enemyData._jumpAttackChance
            && agent._canUseSkill
            && distance >= agent._enemyData._minJumpAttackRange
            && distance <= agent._enemyData._maxJumpAttackRange
            && agent._enemyData._jumpTime + agent._enemyData._jumpAttackCooldown < Time.time)
        {
            agent._animator.SetBool("isChasing", false);
            agent._stateMachine.ChangeState(AI_StateID.SpecialAttack);
            Debug.Log("ACTIVATE JUMP ATTACK");
            return;
        }
    }

    private void CheckForAttack(AI_Agent agent, float distance)
    {
        if (distance < agent._enemyData._attackRange && agent._attackTimer < 0)
        {
            agent._attackTimer = agent._enemyData._attackSpeed;
            agent.transform.LookAt(_followPosition);
            agent._animator.SetTrigger("attack");
            agent._animator.SetBool("isChasing", false);
            agent._stateMachine.ChangeState(AI_StateID.Attack);
        }
        else
        {
            agent._attackTimer -= Time.deltaTime;
        }
    }
}
