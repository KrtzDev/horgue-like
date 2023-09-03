using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedRobot_State_Idle : AI_State_Idle
{
    private AI_Agent_RangedRobot _rangedRobot;

    public override void Enter(AI_Agent agent)
    {
        base.Enter(agent);

        _rangedRobot = agent as AI_Agent_RangedRobot;

        _rangedRobot._followPosition = agent.transform.position;

        agent.SetTarget(agent, _rangedRobot._followPosition);
    }

    public override void Update(AI_Agent agent)
    {
        if (!agent.NavMeshAgent.enabled)
        {
            return;
        }

        if (agent.FollowDecoy)
        {
            _rangedRobot._followPosition = agent.DecoyTransform.position;
        }
        else
        {
            _rangedRobot._followPosition = agent.PlayerTransform.position;
        }

        float distance = Vector3.Distance(agent.transform.position, _rangedRobot._followPosition);

        RaycastHit hit;
        if (Physics.Raycast(_rangedRobot.ProjectilePoint.transform.position, (_rangedRobot._followPosition + new Vector3(0, 0.5f, 0) - _rangedRobot.ProjectilePoint.transform.position), out hit, distance, agent.GroundLayer))
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
