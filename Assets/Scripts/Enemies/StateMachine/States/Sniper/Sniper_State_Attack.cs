using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sniper_State_Attack : AI_State_Attack
{

    private AI_Agent_Sniper _sniper;

    public override void Enter(AI_Agent agent)
    {
        base.Enter(agent);

        _sniper = agent as AI_Agent_Sniper;

        agent.Animator.SetBool("isAttacking", true);
        agent.NavMeshAgent.SetDestination(agent.transform.position);
    }

    public override void Update(AI_Agent agent)
    {
        if (!agent.FollowDecoy)
        {
            _sniper._followPosition = agent.PlayerTransform.position;
        }
        else
        {
            _sniper._followPosition = agent.DecoyTransform.position;
        }

        Vector3 _lookPosition = new Vector3(_sniper._followPosition.x, agent.transform.position.y, _sniper._followPosition.z);
        agent.transform.LookAt(_lookPosition);

        float distance = Vector3.Distance(agent.transform.position, _sniper._followPosition);

        RaycastHit hit;
        if (!Physics.Raycast(_sniper.ProjectilePoint.transform.position, (_sniper._followPosition + new Vector3(0, 0.5f, 0) - _sniper.ProjectilePoint.transform.position), out hit, distance, agent.GroundLayer))
        {
            if (agent.AttackTimer <= 0)
            {
                agent.AttackTimer = _enemy._enemyData._attackSpeed;
                _sniper.TargetDirection = (_sniper._followPosition - agent.transform.position).normalized;
                agent.Animator.SetTrigger("shoot");
                agent.Animator.SetBool("isShooting", true);
                return;
            }
            else if (!agent.Animator.GetBool("isShooting"))
            {
                agent.AttackTimer -= Time.deltaTime;

                if (distance > _enemy._enemyData._attackRange)
                {
                    agent.StateMachine.ChangeState(AI_StateID.Idle);
                }
                else if (distance < _enemy._enemyData._retreatRange)
                {
                    agent.StateMachine.ChangeState(AI_StateID.Retreat);
                }
            }
        }
        else if (!agent.Animator.GetBool("isShooting"))
        {
            agent.StateMachine.ChangeState(AI_StateID.Idle);
        }
    }

    public override void Exit(AI_Agent agent)
    {
        agent.Animator.SetBool("isAttacking", false);
    }
}
