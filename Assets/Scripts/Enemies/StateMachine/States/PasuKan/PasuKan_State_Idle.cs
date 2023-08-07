using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PasuKan_State_Idle : AI_State_Idle
{
    private AI_Agent_PasuKan _pasuKan;

    public override void Enter(AI_Agent agent)
    {
        base.Enter(agent);

        agent._animator.SetBool("isIdle", true);

        _pasuKan = agent as AI_Agent_PasuKan;

        _pasuKan._followPosition = agent.transform.position;

        agent.SetTarget(agent, _pasuKan._followPosition);
    }

    public override void Update(AI_Agent agent)
    {
        if (!agent._navMeshAgent.enabled)
        {
            return;
        }

        agent._attackTimer -= Time.deltaTime;

        if (agent._followDecoy)
        {
            _pasuKan._followPosition = agent._decoyTransform.position;
        }
        else
        {
            _pasuKan._followPosition = agent._playerTransform.position;
        }

        float distance = Vector3.Distance(agent.transform.position, _pasuKan._followPosition);

        RaycastHit hit;
        if (Physics.Raycast(_pasuKan.transform.position, (_pasuKan._followPosition + new Vector3(0, 0.5f, 0) - _pasuKan.transform.position), out hit, distance, agent._groundLayer))
        {
            if (distance >= _enemy._enemyData._attackRange)
            {
                agent._stateMachine.ChangeState(AI_StateID.ChasePlayer);
            }
        }
        else
        {
            if (distance <= _enemy._enemyData._attackRange && _enemy._attackTimer <= 0)
            {
                agent._attackTimer = _enemy._enemyData._attackSpeed;
                agent.transform.LookAt(_pasuKan._followPosition);
                agent._animator.SetTrigger("attack");
                agent._stateMachine.ChangeState(AI_StateID.Attack);
            }
            else if (distance >= _enemy._enemyData._attackRange)
            {
                agent._stateMachine.ChangeState(AI_StateID.ChasePlayer);
            }
        }
    }

    public override void Exit(AI_Agent agent)
    {
        agent._animator.SetBool("isIdle", false);
    }
}
