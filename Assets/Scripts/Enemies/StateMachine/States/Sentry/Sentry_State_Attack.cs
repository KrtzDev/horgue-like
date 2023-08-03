using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sentry_State_Attack : AI_State_Attack
{
    private AI_Agent_Sentry _sentry;

    public override void Enter(AI_Agent agent)
    {
        base.Enter(agent);

        _sentry = agent as AI_Agent_Sentry;

        agent._attackTimer = _sentry._attackRate;

        switch (_sentry._sentryStatus)
        {
            case SentryStatus.Enemy:
                _sentry._targetDirection = (agent._playerTransform.position - agent.transform.position).normalized;
                break;

            case SentryStatus.Ally:
                _sentry._targetDirection = (_sentry._closestEnemy.transform.position - agent.transform.position).normalized;
                break;
        }

        _sentry.Shoot();

        agent._animator.SetBool("isAttacking", false);
        agent._animator.SetBool("isChasing", true);
        agent._stateMachine.ChangeState(AI_StateID.ChasePlayer);
    }

    public override void Update(AI_Agent agent)
    {

    }

    public override void Exit(AI_Agent agent)
    {

    }
}