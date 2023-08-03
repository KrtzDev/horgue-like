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

        agent.transform.LookAt(_drone._followPosition + new Vector3(0, 0.5f, 0));
    }

    public override void Update(AI_Agent agent)
    {
        if (!agent._followDecoy)
        {
            _drone._followPosition = agent._playerTransform.position;
        }
        else
        {
            _drone._followPosition = agent._decoyTransform.position;
        }

        if (agent._attackTimer <= 0)
        {
            agent._attackTimer = _enemy._enemyData._attackSpeed;

            _drone.DetermineTargetPosition(_drone._followPosition);
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
