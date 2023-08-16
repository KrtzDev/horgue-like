using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PasuKan_State_JumpAttack : AI_State_SpecialAttack
{
    Vector3 _startingPosition;
    Vector3 _followPosition;
    float _jumpTime = 0;
    float _jumpPrepTime = 0;
    float _jumpFactor = 0;

    public override void Enter(AI_Agent agent)
    {
        base.Enter(agent);

        agent.NavMeshAgent.enabled = false;
        agent.RigidBody.velocity = new Vector3(0, agent.RigidBody.velocity.y, 0);

        _startingPosition = agent.transform.position;
        _followPosition = agent.Player.transform.position;
        agent.Animator.SetTrigger("jumpAttack");
        agent.Animator.SetFloat("jumpTime", -1f);
    }

    public override void Update(AI_Agent agent)
    {
        if(_jumpPrepTime <= _enemy._enemyData._jumpPrepTime)
        {
            _jumpPrepTime += Time.deltaTime;
        }
        else
        {
            if (_jumpTime <= _enemy._enemyData._jumpTime)
            {
                _jumpTime += Time.deltaTime;
                _jumpFactor += Time.deltaTime * _enemy._enemyData._jumpForce;

                agent.Animator.transform.position = Vector3.Lerp(_startingPosition, _followPosition, _jumpFactor) + Vector3.up * _enemy._enemyData._heightCurve.Evaluate(_jumpTime) * _enemy._enemyData._jumpForce;
                agent.Animator.transform.rotation = Quaternion.Slerp(agent.Animator.transform.rotation, Quaternion.LookRotation(_followPosition - agent.Animator.transform.position), _jumpFactor);
                agent.Animator.SetFloat("jumpTime", _jumpTime);

                if (agent.Animator.transform.position == _followPosition)
                {
                    agent.Animator.SetTrigger("jumpAttackEnded");
                    agent.Animator.SetBool("isChasing", true);
                    agent.StateMachine.ChangeState(AI_StateID.ChasePlayer);
                }
            }
            else
            {
                agent.Animator.SetTrigger("jumpAttackEnded");
                agent.Animator.SetBool("isChasing", true);
                agent.StateMachine.ChangeState(AI_StateID.ChasePlayer);
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
        agent.NavMeshAgent.enabled = true;
        if(NavMesh.SamplePosition(agent.transform.position, out NavMeshHit hit, 1f, agent.NavMeshAgent.areaMask))
        {
            agent.NavMeshAgent.Warp(hit.position);
        }
    }
}
