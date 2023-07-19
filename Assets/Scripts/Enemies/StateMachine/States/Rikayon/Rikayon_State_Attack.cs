using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rikayon_State_Attack : AI_State_Attack
{
    private AI_Agent_Rikayon _rikayon;

    public override void Enter(AI_Agent agent)
    {
        base.Enter(agent);

        _rikayon = agent as AI_Agent_Rikayon;

        _followPosition = agent.transform.position;

        agent.SetTarget(agent, _followPosition);
    }

    public override void Update(AI_Agent agent)
    {
        if (agent._animator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
        {
            switch (_rikayon._currentBossStage)
            {
                case 1:
                    agent._animator.speed = _rikayon._bossStageAnimationMultiplier.x;
                    break;
                case 2:
                    agent._animator.speed = _rikayon._bossStageAnimationMultiplier.y;
                    break;
            }
        }
        else if (agent._animator.GetCurrentAnimatorStateInfo(0).IsName("Intimidate"))
        {
            switch(_rikayon._currentBossStage)
            {
                case 1:
                    agent._animator.speed = _rikayon._bossStageAnimationMultiplier.x;
                    break;
                case 2:
                    agent._animator.speed = _rikayon._bossStageAnimationMultiplier.y;
                    break;
            }

            if (agent._animator.GetFloat("intimidateNumber") == 1)
            {
                StartRotating(agent); // can't get Rikayon to rotate ... 
            }
        }
        else
        {
            agent._animator.speed = agent._originalAnimationSpeed;
            agent._animator.SetBool("isIntimidating", false);
            agent._stateMachine.ChangeState(AI_StateID.Idle);
        }
    }

    private void StartRotating(AI_Agent agent)
    {
        if (LookCoroutine != null)
        {
            AI_Manager.Instance.StopCoroutine(LookCoroutine);
        }

        if (agent._followDecoy)
        {
            _followPosition = agent._decoyTransform.position;
        }
        else
        {
            _followPosition = agent._playerTransform.position;
        }

        LookCoroutine = AI_Manager.Instance.StartCoroutine(AI_Manager.Instance.LookAtTarget(agent, _followPosition, _maxTime));
    }


    public override void Exit(AI_Agent agent)
    {
        agent._animator.SetBool("isAttacking", false);
    }
}
