using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PasuKan_State_Idle : AI_State_Idle
{
    public override void Enter(AI_Agent agent)
    {
        agent._attackTimer = agent._enemyData._attackSpeed;
    }

    public override void Update(AI_Agent agent)
    {
        if(!agent._followDecoy) _followPosition = new Vector3(agent._playerTransform.position.x, agent._playerTransform.position.y, agent._playerTransform.position.z);
        if(agent._followDecoy) _followPosition = new Vector3(agent._decoyTransform.position.x, agent._decoyTransform.position.y, agent._decoyTransform.position.z);

        agent.transform.LookAt(_followPosition);

        float distance = Vector3.Distance(agent.transform.position, _followPosition);

        if (distance > agent._enemyData._attackRange)
        {
            agent._animator.SetBool("isChasing", true);
            agent._stateMachine.ChangeState(AI_StateID.ChasePlayer);
        }
        else if (agent._attackTimer < 0)
        {
            agent._attackTimer = agent._enemyData._attackSpeed;
            agent._animator.SetTrigger("attack");
            agent._stateMachine.ChangeState(AI_StateID.Attack);
        }
        else
        {
            agent._attackTimer -= Time.deltaTime;
        }
    }

    public override void Exit(AI_Agent agent)
    {

    }
}
