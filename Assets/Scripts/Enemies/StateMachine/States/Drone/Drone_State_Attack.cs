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
        if (!agent.FollowDecoy)
        {
            _drone._followPosition = agent.PlayerTransform.position;
        }
        else
        {
            _drone._followPosition = agent.DecoyTransform.position;
        }

        if (agent.AttackTimer <= 0)
        {
            agent.AttackTimer = _enemy._enemyData._attackSpeed;

            _drone.DetermineTargetPosition(_drone._followPosition);
            agent.Animator.SetTrigger("shoot");
            agent.Animator.SetBool("isShooting", true);
            return;
        }

        if (!agent.Animator.GetCurrentAnimatorStateInfo(0).IsName("Shoot"))
        {
            agent.StateMachine.ChangeState(AI_StateID.Idle);
        }
    }

    public override void Exit(AI_Agent agent)
    {
        agent.Animator.SetBool("isShooting", false);
    }
}
