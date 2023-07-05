using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PasuKan_State_Attack : AI_State_Attack
{
    private float _jumpPrepTime;
    private float _jumpTime;
    private float _jumpFactor;

    public override void Enter(AI_Agent agent)
    {
        _jumpPrepTime = 0;
        _jumpTime = 0;
        _jumpFactor = 0;

        agent._animator.SetFloat("jumpTime", -1);

        agent._navMeshAgent.enabled = false;

        if (!agent._followDecoy) _followPosition = new Vector3(agent._playerTransform.position.x, agent._playerTransform.position.y, agent._playerTransform.position.z);
        if (agent._followDecoy) _followPosition = new Vector3(agent._decoyTransform.position.x, agent._decoyTransform.position.y, agent._decoyTransform.position.z);
    }

    public override void Update(AI_Agent agent)
    {
        if (_jumpPrepTime <= agent._enemyData._jumpPrepTime)
        {
            _jumpPrepTime += Time.deltaTime;
        }
        else
        {
            Vector3 startingPosition = agent.transform.position;

            /* if (_jumpTime <= 0.75 * enemy.EnemyData._jumpTime )
            {
                if (!enemy.FollowDecoy)
                {
                    _followPosition = new Vector3(player.position.x, player.position.y, player.position.z);
                }
                else
                {
                    _followPosition = new Vector3(decoy.position.x, decoy.position.y, decoy.position.z);
                }
            }

            */

            // Vector3 dirToPlayer = _followPosition - animator.transform.position;
            // Vector3 newPos = _followPosition - dirToPlayer.normalized * 2.5f;
            // _followPosition = newPos;

            if (_jumpTime <= agent._enemyData._jumpTime)
            {
                _jumpTime += Time.deltaTime;
                _jumpFactor += Time.deltaTime * agent._enemyData._jumpForce;

                agent.transform.position = Vector3.Lerp(startingPosition, _followPosition, _jumpFactor) + Vector3.up * agent._enemyData._heightCurve.Evaluate(_jumpTime) * agent._enemyData._jumpForce;
                agent.transform.rotation = Quaternion.Slerp(agent.transform.rotation, Quaternion.LookRotation(_followPosition - agent.transform.position), _jumpFactor);
                agent._animator.SetFloat("jumpTime", _jumpTime);

                if (agent.transform.position == _followPosition)
                {
                    agent._animator.SetTrigger("jumpAttackEnded");
                }
            }
            else
            {
                agent._animator.SetTrigger("jumpAttackEnded");
            }
        }
    }

    public override void Exit(AI_Agent agent)
    {
        agent.enabled = true;

        if (NavMesh.SamplePosition(_followPosition, out NavMeshHit hit, 1f, agent._navMeshAgent.areaMask))
        {
            agent._navMeshAgent.Warp(hit.position);
            agent._animator.SetBool("isChasing", true);
            agent._stateMachine.ChangeState(AI_StateID.ChasePlayer);
        }
    }
}
