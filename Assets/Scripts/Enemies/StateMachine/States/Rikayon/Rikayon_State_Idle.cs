using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rikayon_State_Idle : AI_State_Idle
{
    private AI_Agent_Rikayon _rikayon;

    public override void Enter(AI_Agent agent)
    {
        base.Enter(agent);

        _rikayon = agent as AI_Agent_Rikayon;

        _rikayon._followPosition = agent.transform.position;

        agent.SetTarget(agent, _rikayon._followPosition);
    }

    public override void Update(AI_Agent agent)
    {
        if (!agent.NavMeshAgent.enabled)
        {
            return;
        }

        if (agent.AttackTimer > 0)
            agent.AttackTimer -= Time.deltaTime;

        if (agent.FollowDecoy)
        {
            _rikayon._followPosition = agent.DecoyTransform.position;
        }
        else
        {
            _rikayon._followPosition = agent.PlayerTransform.position;
        }

        float distance = Vector3.Distance(agent.transform.position, _rikayon._followPosition);

        _timer -= Time.deltaTime;

        if (_timer < 0f)
        {
            StartRotating(agent);
            _timer = _maxTime;

            if (distance > _enemy._enemyData._attackRange)
            {
                agent.StateMachine.ChangeState(AI_StateID.ChasePlayer);
                return;
            }
            else if (distance > _enemy._enemyData._attackRange && agent.AttackTimer <= 0)
            {
                agent.StateMachine.ChangeState(AI_StateID.Attack);
                return;
            }
            else
            {
                agent.StateMachine.ChangeState(AI_StateID.SpecialAttack);
                return;
            }

        }
    }

    public override void Exit(AI_Agent agent)
    {

    }
    private void StartRotating(AI_Agent agent)
    {
        if (LookCoroutine != null)
        {
            AI_Manager.Instance.StopCoroutine(LookCoroutine);
        }

        LookCoroutine = AI_Manager.Instance.StartCoroutine(AI_Manager.Instance.LookAtTarget(agent, _rikayon._followPosition, _maxTime));
    }
}
