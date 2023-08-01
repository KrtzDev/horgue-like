using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drone_State_Attack : AI_State_Attack
{

    private AI_Agent_Drone _drone;
    private bool _hasShot;

    public override void Enter(AI_Agent agent)
    {
        base.Enter(agent);

        _drone = agent as AI_Agent_Drone;

        agent._navMeshAgent.SetDestination(agent.transform.position);
        agent.transform.LookAt(_followPosition);
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

        if (agent._attackTimer <= 0)
        {
            agent._attackTimer = _enemy._enemyData._attackSpeed;

            _drone.DetermineTargetPosition(_drone, _followPosition);
            agent._animator.SetTrigger("shoot");
            agent._animator.SetBool("isShooting", true);
            return;
        }

        if (!agent._animator.GetCurrentAnimatorStateInfo(0).IsName("Shoot"))
        {
            agent._stateMachine.ChangeState(AI_StateID.Idle);
        }
    }

    public override void Exit(AI_Agent agent)
    {
        agent._animator.SetBool("isShooting", false);
    }
}
