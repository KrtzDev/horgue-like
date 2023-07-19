using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rikayon_State_ChasePlayer : AI_State_ChasePlayer
{
    AI_Agent_Rikayon _rikayon;

    public override void Enter(AI_Agent agent)
    {
        base.Enter(agent);

        agent._animator.SetBool("isChasing", true);

        _rikayon = agent as AI_Agent_Rikayon;
    }

    public override void Update(AI_Agent agent)
    {
        if (!agent._navMeshAgent.enabled)
        {
            return;
        }

        _timer -= Time.deltaTime;

        float distance = Vector3.Distance(agent.transform.position, agent._playerTransform.position);
        CheckForAttack(_enemy, distance);

        if (_timer < 0f)
        {
            if (agent._navMeshAgent.pathStatus != UnityEngine.AI.NavMeshPathStatus.PathPartial)
            {
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

                StartRotating(agent);
            }

            _timer = _maxTime;
        }
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

    private void CheckForAttack(AI_Agent agent, float distance)
    {
        if (distance < _enemy._enemyData._attackRange && agent._attackTimer < 0)
        {
            agent._attackTimer = _enemy._enemyData._attackSpeed;
            _followPosition = agent._playerTransform.position;
            agent.transform.LookAt(_followPosition);

            int random = Random.Range(1, _rikayon._numberOfAttacks + 1);
            agent._animator.SetFloat("attackNumber", random);

            random = Random.Range(1, _rikayon._numberOfIntimidations + 1);

            agent._animator.SetFloat("intimidateNumber", AbilityBias(random));
            SafeLastAbility(AbilityBias(random));

            agent._animator.SetBool("isAttacking", true);
            agent._stateMachine.ChangeState(AI_StateID.Attack);

            return;
        }
        else if (distance < _enemy._enemyData._attackRange && agent._attackTimer > 0)
        {
            _followPosition = agent.transform.position;
            agent.SetTarget(agent, _followPosition);

            int random = Random.Range(1, _rikayon._numberOfIntimidations + 1);

            agent._animator.SetFloat("intimidateNumber", AbilityBias(random));
            SafeLastAbility(AbilityBias(random));

            agent._animator.SetBool("isIntimidating", true);
            agent._stateMachine.ChangeState(AI_StateID.Idle);
            return;
        }
        else
        {
            agent._attackTimer -= Time.deltaTime;
        }
    }

    private int AbilityBias(int random)
    {
        if (random == _rikayon._lastAbilities[0])
        {
            if (random == _rikayon._lastAbilities[1] || random == _rikayon._lastAbilities[2])
            {
                if (random == _rikayon._numberOfIntimidations)
                {
                    random = 1;
                }
                else
                {
                    random++;
                }
            }

            return random;
        }

        if (random == _rikayon._lastAbilities[1])
        {
            if (random == _rikayon._lastAbilities[0] || random == _rikayon._lastAbilities[2])
            {
                if (random == _rikayon._numberOfIntimidations)
                {
                    random = 1;
                }
                else
                {
                    random++;
                }
            }

            return random;
        }

        if (random == _rikayon._lastAbilities[2])
        {
            if (random == _rikayon._lastAbilities[0] || random == _rikayon._lastAbilities[1])
            {
                if (random == _rikayon._numberOfIntimidations)
                {
                    random = 1;
                }
                else
                {
                    random++;
                }
            }

            return random;
        }

        return random;
    }

    private void SafeLastAbility(int random)
    {
        _rikayon._lastAbilities.x = _rikayon._lastAbilities.y;
        _rikayon._lastAbilities.y = _rikayon._lastAbilities.z;
        _rikayon._lastAbilities.z = random;
    }
}
