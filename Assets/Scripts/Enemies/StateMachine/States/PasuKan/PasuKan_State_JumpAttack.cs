using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PasuKan_State_JumpAttack : AI_State_SpecialAttack
{
    Vector3 _startingPosition;
    Vector3 _followPosition;
    float _time = 0;
    float _jumpTime = 0;
    float _jumpPrepTime = 0;
    float _jumpFactor = 0;

    public override void Enter(AI_Agent agent)
    {
        agent._navMeshAgent.enabled = false;
        agent._rb.velocity = new Vector3(0, agent._rb.velocity.y, 0);

        _startingPosition = agent.transform.position;
        _followPosition = agent._player.transform.position;
        agent._animator.SetTrigger("jumpAttack");
        agent._animator.SetFloat("jumpTime", -1f);
    }

    public override void Update(AI_Agent agent)
    {
        if(_jumpPrepTime <= agent._enemyData._jumpPrepTime)
        {
            _jumpPrepTime += Time.deltaTime;
        }
        else
        {
            if (_jumpTime <= agent._enemyData._jumpTime)
            {
                _jumpTime += Time.deltaTime;
                _jumpFactor += Time.deltaTime * agent._enemyData._jumpForce;

                agent._animator.transform.position = Vector3.Lerp(_startingPosition, _followPosition, _jumpFactor) + Vector3.up * agent._enemyData._heightCurve.Evaluate(_jumpTime) * agent._enemyData._jumpForce;
                agent._animator.transform.rotation = Quaternion.Slerp(agent._animator.transform.rotation, Quaternion.LookRotation(_followPosition - agent._animator.transform.position), _jumpFactor);
                agent._animator.SetFloat("jumpTime", _jumpTime);

                if (agent._animator.transform.position == _followPosition)
                {
                    agent._animator.SetTrigger("jumpAttackEnded");
                    agent._animator.SetBool("isChasing", true);
                    agent._stateMachine.ChangeState(AI_StateID.ChasePlayer);
                }
            }
            else
            {
                agent._animator.SetTrigger("jumpAttackEnded");
                agent._animator.SetBool("isChasing", true);
                agent._stateMachine.ChangeState(AI_StateID.ChasePlayer);
            }
        }

        /*
        time += Time.deltaTime * agent._enemyData._jumpForce;
        jumpTime += Time.deltaTime;
        agent._animator.SetFloat("jumpTime", jumpTime);

        agent.transform.position = Vector3.Lerp(startingPosition, agent._player.transform.position, time)
            + Vector3.up * agent._enemyData._heightCurve.Evaluate(time);
        agent.transform.rotation = Quaternion.Slerp(agent.transform.rotation,
            Quaternion.LookRotation(agent._player.transform.position - agent.transform.position),
            time);

        if(_jumpTime >= 2)
        {
            agent._animator.SetBool("isChasing", true);
            agent._stateMachine.ChangeState(AI_StateID.ChasePlayer);
        }

        */
    }

    public override void Exit(AI_Agent agent)
    {
        agent._navMeshAgent.enabled = true;
        if(NavMesh.SamplePosition(agent.transform.position, out NavMeshHit hit, 1f, agent._navMeshAgent.areaMask))
        {
            agent._navMeshAgent.Warp(hit.position);
        }
    }
}
