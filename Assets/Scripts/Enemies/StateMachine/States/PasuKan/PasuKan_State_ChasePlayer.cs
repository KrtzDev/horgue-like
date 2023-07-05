using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PasuKan_State_ChasePlayer : AI_State_ChasePlayer
{
    private float _oldSpeed;
    private float _oldAcceleration;
    private float _jumpAttackTimer;

    private bool _rageMode;

    public override void Enter(AI_Agent agent)
    {

    }

    public override void Update(AI_Agent agent)
    {
        if (!agent._followDecoy)
        {
            _followPosition = new Vector3(agent._playerTransform.position.x, agent._playerTransform.position.y, agent._playerTransform.position.z);
            if (agent._obstacleAgent.enabled) agent._obstacleAgent.SetDestination(_followPosition);
        }
        else
        {
            _followPosition = _followPosition = new Vector3(agent._decoyTransform.position.x, agent._decoyTransform.position.y, agent._decoyTransform.position.z);
            if (agent._obstacleAgent.enabled) agent._obstacleAgent.SetDestination(_followPosition);
        }

        float distance = Vector3.Distance(agent.transform.position, _followPosition);

        if (distance < agent._enemyData._maxJumpAttackRange && distance > agent._enemyData._minJumpAttackRange && distance > agent._enemyData._attackRange && _jumpAttackTimer < 0 && !_rageMode)
        {
            int random = Random.Range(0, 100);

            if (random < agent._enemyData._jumpAttackChance)
            {
                _jumpAttackTimer = agent._enemyData._jumpAttackCooldown;
                agent._animator.transform.LookAt(_followPosition);
                agent._animator.SetTrigger("rageMode");
                agent._animator.SetBool("isRageMode", true);
                _rageMode = true;
                _oldSpeed = agent._navMeshAgent.speed;
                _oldAcceleration = agent._navMeshAgent.acceleration;
                agent._navMeshAgent.acceleration *= 1.5f;
                agent._navMeshAgent.speed *= 1.5f;

                if (agent.GetComponent<HealthComponent>()._currentHealth > 0)
                {
                    agent.GetComponent<HealthComponent>()._maxHealth *= 2;
                    agent.GetComponent<HealthComponent>()._currentHealth = agent.GetComponent<HealthComponent>()._maxHealth;
                }
            }
            else
            {
                _jumpAttackTimer = agent._enemyData._jumpAttackCooldown;
            }
        }

        if (_jumpAttackTimer >= 0)
        {
            _jumpAttackTimer -= Time.deltaTime;
        }
        else
        {
            if (_rageMode && agent._navMeshAgent.speed != _oldSpeed)
            {
                agent._navMeshAgent.speed = _oldSpeed;
                agent._navMeshAgent.acceleration = _oldAcceleration;
                _jumpAttackTimer = agent._enemyData._jumpAttackCooldown;
                agent._animator.SetBool("isRageMode", false);
            }
        }

        if (distance < agent._enemyData._attackRange && agent._attackTimer < 0)
        {
            agent._attackTimer = agent._enemyData._attackSpeed;
            agent._animator.transform.LookAt(_followPosition);
            agent._animator.SetTrigger("attack");
            agent._animator.SetBool("isChasing", false);
            agent._stateMachine.ChangeState(AI_StateID.Attack);
        }
        else
        {
            agent._attackTimer -= Time.deltaTime;
        }
    }

    public override void Exit(AI_Agent agent)
    {
        if (agent._obstacleAgent.enabled)
            agent._obstacleAgent.SetDestination(agent.transform.position);
    }
}
