using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drone_State_Attack : AI_State_Attack
{

    private AI_Agent_Drone _drone;

    public override void Enter(AI_Agent agent)
    {
        base.Enter(agent);

        _drone = agent as AI_Agent_Drone;

        agent._animator.SetBool("isAttacking", true);
        agent._navMeshAgent.SetDestination(agent.transform.position);
    }

    public override void Update(AI_Agent agent)
    {
        if (!agent._followDecoy)
        {
            _followPosition = agent._playerTransform.position;
        }
        else
        {
            _followPosition = agent._decoyTransform.position;
        }

        Vector3 _lookPosition = new Vector3(_followPosition.x, agent.transform.position.y, _followPosition.z);
        agent.transform.LookAt(_lookPosition);

        float distance = Vector3.Distance(agent.transform.position, _followPosition);

        RaycastHit hit;
        if (!Physics.Raycast(_drone.ProjectilePoint.transform.position, (_followPosition + new Vector3(0, 0.5f, 0) - _drone.ProjectilePoint.transform.position), out hit, distance, agent._groundLayer))
        {
            if (agent._attackTimer <= 0)
            {
                agent._attackTimer = _enemy._enemyData._attackSpeed;
                _drone.TargetDirection = (_followPosition - agent.transform.position).normalized;
                agent._animator.SetTrigger("shoot");
                agent._animator.SetBool("isShooting", true);
                return;
            }
            else if (!agent._animator.GetBool("isShooting"))
            {
                agent._attackTimer -= Time.deltaTime;

                if (distance > _enemy._enemyData._attackRange)
                {
                    agent._stateMachine.ChangeState(AI_StateID.Idle);
                }
            }
        }
        else if (!agent._animator.GetBool("isShooting"))
        {
            agent._stateMachine.ChangeState(AI_StateID.Idle);
        }
    }

    public override void Exit(AI_Agent agent)
    {
        agent._animator.SetBool("isAttacking", false);
    }
}
