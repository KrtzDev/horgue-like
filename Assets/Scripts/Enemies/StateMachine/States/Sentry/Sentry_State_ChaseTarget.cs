using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameDataReader;

public class Sentry_State_ChaseTarget : AI_State_ChasePlayer
{
    private AI_Agent_Sentry _sentry;

    public override void Enter(AI_Agent agent)
    {
        base.Enter(agent);

        _sentry = agent as AI_Agent_Sentry;
    }

    public override void Update(AI_Agent agent)
    {
        switch (_sentry._sentryStatus)
        {
            case SentryStatus.Ally:
                _sentry.RotateTowardsEnemy();
                break;

            case SentryStatus.Enemy:
                float _distance = Vector3.Distance(agent.transform.position, agent.PlayerTransform.position);

                if (_distance < _sentry._range)
                {
                    _sentry.LookAtTarget();

                    if (agent.AttackTimer < 0)
                    {
                        agent.Animator.SetBool("isAttacking", true);
                        agent.Animator.SetBool("isChasing", false);
                        agent.StateMachine.ChangeState(AI_StateID.Attack);
                    }

                    agent.AttackTimer -= Time.deltaTime;
                }
                break;

            case SentryStatus.Off:
                agent.Animator.SetBool("isAttacking", false);
                agent.Animator.SetBool("isChasing", false);
                agent.StateMachine.ChangeState(AI_StateID.Death);
                break;
        }
    }

    public override void Exit(AI_Agent agent)
    {

    }

    private void StartRotating(AI_Agent agent)
    {
        
    }
}
