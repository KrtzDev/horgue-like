using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sniper_State_Idle : AI_State_Idle
{
    private AI_Agent_Sniper _sniper;

    public override void Enter(AI_Agent agent)
    {
        base.Enter(agent);

        _sniper = agent as AI_Agent_Sniper;

        _sniper._followPosition = agent.transform.position;

        agent.SetTarget(agent, _sniper._followPosition);
    }

    public override void Update(AI_Agent agent)
    {
        if (!agent.NavMeshAgent.enabled)
        {
            return;
        }

        if (agent.FollowDecoy)
        {
            _sniper._followPosition = agent.DecoyTransform.position;
        }
        else
        {
            _sniper._followPosition = agent.PlayerTransform.position;
        }

        float distance = Vector3.Distance(agent.transform.position, _sniper._followPosition);

        RaycastHit hit;
        if (Physics.Raycast(_sniper.ProjectilePoint.transform.position, (_sniper._followPosition + new Vector3(0, 0.5f, 0) - _sniper.ProjectilePoint.transform.position), out hit, distance, agent.GroundLayer))
        {
            if (distance <= _enemy._enemyData._retreatRange)
            {
                agent.StateMachine.ChangeState(AI_StateID.Retreat);
            }
            else
            {
                agent.StateMachine.ChangeState(AI_StateID.ChasePlayer);
            }
        }
        else
        {
            if (distance <= _enemy._enemyData._retreatRange)
            {
                agent.StateMachine.ChangeState(AI_StateID.Retreat);
            }
            else if (distance < _enemy._enemyData._attackRange)
            {
                agent.StateMachine.ChangeState(AI_StateID.Attack);
            }
            else if (distance >= _enemy._enemyData._attackRange)
            {
                agent.StateMachine.ChangeState(AI_StateID.ChasePlayer);
            }
        }
    }

    public override void Exit(AI_Agent agent)
    {

    }
}
