using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PasuKan_State_ChasePlayer : AI_State_ChasePlayer
{
    private AI_Agent_PasuKan _pasuKan;

    public override void Enter(AI_Agent agent)
    {
        base.Enter(agent);

        _pasuKan = agent as AI_Agent_PasuKan;

        agent.Animator.SetBool("isChasing", true);
    }

    public override void Update(AI_Agent agent)
    {
        if(!agent.NavMeshAgent.enabled)
        {
            return;
        }

        _timer -= Time.deltaTime;

        if(_timer < 0f)
        {
            if (agent.NavMeshAgent.pathStatus != UnityEngine.AI.NavMeshPathStatus.PathPartial)
            {
                if(!agent.UseMovementPrediction)
                {
                    if (agent.FollowDecoy)
                    {
                        _pasuKan._followPosition = agent.DecoyTransform.position;
                        agent.SetTarget(agent, _pasuKan._followPosition);
                    }
                    else
                    {
                        _pasuKan._followPosition = agent.PlayerTransform.position;
                        agent.SetTarget(agent, _pasuKan._followPosition);
                    }
                }
                else
                {
                    if (agent.FollowDecoy)
                    {
                        _pasuKan._followPosition = agent.DecoyTransform.position;
                        agent.SetTarget(agent, _pasuKan._followPosition);
                    }
                    else
                    {
                        _pasuKan._followPosition = agent.PlayerTransform.position + (agent.Player.GetComponent<PlayerMovement>().AverageVelocity * agent.MovementPredictionTime);

                        Vector3 directionToTarget = (_pasuKan._followPosition - agent.transform.position).normalized;
                        Vector3 directionToPlayer = (agent.PlayerTransform.position - agent.transform.position).normalized;

                        float dot = Vector3.Dot(directionToPlayer, directionToTarget);
                        if(dot < agent.MovementPredictionThreshold)
                        {
                            _pasuKan._followPosition = agent.PlayerTransform.position;
                        }

                        agent.SetTarget(agent, _pasuKan._followPosition);
                    }
                }
            }

            _timer = _maxTime;
        }

        float distance = Vector3.Distance(agent.transform.position, agent.PlayerTransform.position);
        // CheckForJumpAttack(_enemy, distance);
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

        LookCoroutine = AI_Manager.Instance.StartCoroutine(AI_Manager.Instance.LookAtTarget(agent, _pasuKan._followPosition, _maxTime));
    }

    private void CheckForJumpAttack(AI_Agent agent, float distance)
    {
        float random = Random.Range(0f, 100f);

        if (random <= _enemy._enemyData._jumpAttackChance
            && agent.CanUseSkill
            && distance >= _enemy._enemyData._minJumpAttackRange
            && distance <= _enemy._enemyData._maxJumpAttackRange
            && _enemy._enemyData._jumpTime + _enemy._enemyData._jumpAttackCooldown < Time.time)
        {
            agent.Animator.SetBool("isChasing", false);
            agent.StateMachine.ChangeState(AI_StateID.SpecialAttack);
            Debug.Log("ACTIVATE JUMP ATTACK");
            return;
        }
    }

    private void CheckForAttack(AI_Agent agent, float distance)
    {
        if (distance < _enemy._enemyData._attackRange && agent.AttackTimer <= 0)
        {
            agent.AttackTimer = _enemy._enemyData._attackSpeed;
            agent.transform.LookAt(_pasuKan._followPosition);
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
