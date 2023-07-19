using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sentry_State_Off : AI_State_Death
{
    private AI_Agent_Sentry _sentry;

    public override void Enter(AI_Agent agent)
    {
        base.Enter(agent);

        _sentry = agent as AI_Agent_Sentry;
    }

    public override void Update(AI_Agent agent)
    {
        if (_sentry._sentryStatus != SentryStatus.Off)
        {
            agent._animator.SetBool("death", false);
            agent._animator.SetBool("isChasing", true);
            agent._stateMachine.ChangeState(AI_StateID.ChasePlayer);
        }
    }

    public override void Exit(AI_Agent agent)
    {

    }
}
