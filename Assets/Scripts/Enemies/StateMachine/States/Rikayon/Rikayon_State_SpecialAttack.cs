using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rikayon_State_SpecialAttack : AI_State_SpecialAttack
{
    private AI_Agent_Rikayon _rikayon;

    public override void Enter(AI_Agent agent)
    {
        base.Enter(agent);

        _rikayon = agent as AI_Agent_Rikayon;

        _rikayon._followPosition = agent.PlayerTransform.position;
        agent.transform.LookAt(_rikayon._followPosition);

        _rikayon._followPosition = agent.transform.position;

        agent.SetTarget(agent, _rikayon._followPosition);

        _rikayon._currentSpecialAttackProbablity = _rikayon._baseSpecialAttackProbability;

        int random = Random.Range(1, _rikayon._numberOfSpecialAttacks + 1);

        agent.Animator.SetFloat("specialAttackNumber", AbilityBias(random));
        SafeLastAbility(AbilityBias(random));

        agent.Animator.SetBool("isSpecialAttacking", true);
    }

    public override void Update(AI_Agent agent)
    {
        if(agent.AttackTimer > 0)
            agent.AttackTimer -= Time.deltaTime;

        if (agent.Animator.GetCurrentAnimatorStateInfo(0).IsName("SpecialAttack"))
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
            agent.Animator.speed = agent.OriginalAnimationSpeed;
            agent.StateMachine.ChangeState(AI_StateID.Idle);
        }
    }

    public override void Exit(AI_Agent agent)
    {
        agent.Animator.SetBool("isSpecialAttacking", false);
    }

    private int AbilityBias(int random)
    {
        if (random == _rikayon._lastAbilities[0])
        {
            if (random == _rikayon._lastAbilities[1] || random == _rikayon._lastAbilities[2])
            {
                if (random == _rikayon._numberOfSpecialAttacks)
                {
                    random = 1;
                }
                else
                {
                    random++;
                }
            }

            return random;
        }

        if (random == _rikayon._lastAbilities[1])
        {
            if (random == _rikayon._lastAbilities[0] || random == _rikayon._lastAbilities[2])
            {
                if (random == _rikayon._numberOfSpecialAttacks)
                {
                    random = 1;
                }
                else
                {
                    random++;
                }
            }

            return random;
        }

        if (random == _rikayon._lastAbilities[2])
        {
            if (random == _rikayon._lastAbilities[0] || random == _rikayon._lastAbilities[1])
            {
                if (random == _rikayon._numberOfSpecialAttacks)
                {
                    random = 1;
                }
                else
                {
                    random++;
                }
            }

            return random;
        }

        return random;
    }

    private void SafeLastAbility(int random)
    {
        _rikayon._lastAbilities.x = _rikayon._lastAbilities.y;
        _rikayon._lastAbilities.y = _rikayon._lastAbilities.z;
        _rikayon._lastAbilities.z = random;
    }
}
