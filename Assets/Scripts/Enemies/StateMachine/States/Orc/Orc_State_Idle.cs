using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orc_State_Idle : AI_State_Idle
{
    private AI_Agent_Orc _orc;

    public override void Enter(AI_Agent agent)
    {
        base.Enter(agent);

        agent.Animator.SetBool("isIdle", true);

        _orc = agent as AI_Agent_Orc;

        _orc._followPosition = agent.transform.position;

        agent.SetTarget(agent, _orc._followPosition);
    }

    public override void Update(AI_Agent agent)
    {
        if (!agent.NavMeshAgent.enabled)
        {
            return;
        }

        agent.AttackTimer -= Time.deltaTime;

        if (agent.FollowDecoy)
        {
            _orc._followPosition = agent.DecoyTransform.position;
        }
        else
        {
            _orc._followPosition = agent.PlayerTransform.position;
        }

        float distance = Vector3.Distance(agent.transform.position, _orc._followPosition);

        RaycastHit hit;
        if (Physics.Raycast(_orc.transform.position, (_orc._followPosition + new Vector3(0, 0.5f, 0) - _orc.transform.position), out hit, distance, agent.GroundLayer))
        {
            if (distance >= _enemy._enemyData._attackRange)
            {
                agent.StateMachine.ChangeState(AI_StateID.ChasePlayer);
            }
        }
        else
        {
            if (distance <= _enemy._enemyData._attackRange && _enemy.AttackTimer <= 0)
            {
                agent.AttackTimer = _enemy._enemyData._attackSpeed;
                agent.transform.LookAt(_orc._followPosition);
                agent.Animator.SetTrigger("attack");
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
        agent.Animator.SetBool("isIdle", false);
    }
}
