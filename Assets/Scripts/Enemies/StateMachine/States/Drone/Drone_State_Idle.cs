using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drone_State_Idle : AI_State_Idle
{
    private AI_Agent_Drone _drone;

    public override void Enter(AI_Agent agent)
    {
        base.Enter(agent);

        agent._animator.SetBool("isShooting", false);

        _drone = agent as AI_Agent_Drone;

        _followPosition = agent.transform.position;

        agent.SetTarget(agent, _followPosition);
    }

    public override void Update(AI_Agent agent)
    {
        if (!agent._navMeshAgent.enabled)
        {
            return;
        }

        if (agent._followDecoy)
        {
            _followPosition = agent._decoyTransform.position;
        }
        else
        {
            _followPosition = agent._playerTransform.position;
        }

        float distance = Vector3.Distance(agent.transform.position, _followPosition);

        agent.transform.LookAt(_followPosition);

        agent._attackTimer -= Time.deltaTime;

        RaycastHit hit;
        if (!Physics.Raycast(_drone.ProjectilePoint.transform.position, (_followPosition + new Vector3(0, 0.5f, 0) - _drone.ProjectilePoint.transform.position), out hit, distance, agent._groundLayer))
        {
            if (distance >= _enemy._enemyData._attackRange)
            {
                agent._stateMachine.ChangeState(AI_StateID.ChasePlayer);
            }
            else if (distance < _enemy._enemyData._attackRange && _enemy._attackTimer <= 0)
            {
                agent._stateMachine.ChangeState(AI_StateID.Attack);
            }
        }
    }

    public override void Exit(AI_Agent agent)
    {

    }
}
