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
        if (!agent._navMeshAgent.enabled)
        {
            return;
        }

        if (agent._attackTimer > 0)
            agent._attackTimer -= Time.deltaTime;

        if (agent._followDecoy)
        {
            _rikayon._followPosition = agent._decoyTransform.position;
        }
        else
        {
            _rikayon._followPosition = agent._playerTransform.position;
        }

        float distance = Vector3.Distance(agent.transform.position, _rikayon._followPosition);

        _timer -= Time.deltaTime;

        if (_timer < 0f)
        {
            StartRotating(agent);

            if (distance > _enemy._enemyData._attackRange)
            {
                agent._stateMachine.ChangeState(AI_StateID.ChasePlayer);
                return;
            }
            else if (distance > _enemy._enemyData._attackRange && agent._attackTimer <= 0)
            {
                agent._stateMachine.ChangeState(AI_StateID.Attack);
                return;
            }
            else
            {
                agent._stateMachine.ChangeState(AI_StateID.SpecialAttack);
                return;
            }

            _timer = _maxTime;
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
