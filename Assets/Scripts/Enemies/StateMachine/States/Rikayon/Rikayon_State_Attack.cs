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

        _rikayon._followPosition = agent.transform.position;

        agent.SetTarget(agent, _rikayon._followPosition);

        int random = Random.Range(1, _rikayon._numberOfAttacks + 1);
        agent.Animator.SetFloat("attackNumber", random);

        agent.Animator.SetBool("isAttacking", true);
    }

    public override void Update(AI_Agent agent)
    {
        if (agent.Animator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
        {
            switch (_rikayon._currentBossStage)
            {
                case 1:
                    agent.Animator.speed = _rikayon._bossStageAnimationMultiplier.x;
                    break;
                case 2:
                    agent.Animator.speed = _rikayon._bossStageAnimationMultiplier.y;
                    break;
            }
        }
        else
        {
            int random = Random.Range(0, 100);

            if(random < _rikayon._currentSpecialAttackProbablity)
            {
                agent.Animator.speed = agent.OriginalAnimationSpeed;
                agent.StateMachine.ChangeState(AI_StateID.SpecialAttack);
            }
            else
            {
                _rikayon._currentSpecialAttackProbablity = Mathf.RoundToInt(_rikayon._currentSpecialAttackProbablity * _rikayon._specialAttackProbablityModifier);
                agent.Animator.speed = agent.OriginalAnimationSpeed;
                agent.StateMachine.ChangeState(AI_StateID.Idle);
            }
        }
    }

    public override void Exit(AI_Agent agent)
    {
        agent.Animator.SetBool("isAttacking", false);
    }
}
