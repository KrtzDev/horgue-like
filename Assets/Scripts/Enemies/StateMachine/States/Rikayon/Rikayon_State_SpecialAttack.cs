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

        _rikayon._followPosition = agent._playerTransform.position;
        agent.transform.LookAt(_rikayon._followPosition);

        _rikayon._followPosition = agent.transform.position;

        agent.SetTarget(agent, _rikayon._followPosition);

        _rikayon._currentSpecialAttackProbablity = _rikayon._baseSpecialAttackProbability;

        int random = Random.Range(1, _rikayon._numberOfSpecialAttacks + 1);

        agent._animator.SetFloat("specialAttackNumber", AbilityBias(random));
        SafeLastAbility(AbilityBias(random));

        agent._animator.SetBool("isSpecialAttacking", true);
    }

    public override void Update(AI_Agent agent)
    {
        if(agent._attackTimer > 0)
            agent._attackTimer -= Time.deltaTime;

        if (agent._animator.GetCurrentAnimatorStateInfo(0).IsName("SpecialAttack"))
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
        else
        {
            agent._animator.speed = agent._originalAnimationSpeed;
            agent._stateMachine.ChangeState(AI_StateID.Idle);
        }
    }

    public override void Exit(AI_Agent agent)
    {
        agent._animator.SetBool("isSpecialAttacking", false);
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
