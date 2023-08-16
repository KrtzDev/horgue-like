using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedRobot_State_Attack : AI_State_Attack
{

    private AI_Agent_RangedRobot _rangedRobot;

    public override void Enter(AI_Agent agent)
    {
        base.Enter(agent);

        _rangedRobot = agent as AI_Agent_RangedRobot;

        agent.Animator.SetBool("isAttacking", true);
        agent.NavMeshAgent.SetDestination(agent.transform.position);
    }

    public override void Update(AI_Agent agent)
    {
        if (!agent.FollowDecoy)
        {
            _rangedRobot._followPosition = agent.PlayerTransform.position;
        }
        else
        {
            _rangedRobot._followPosition = agent.DecoyTransform.position;
        }

        Vector3 _lookPosition = new Vector3(_rangedRobot._followPosition.x, agent.transform.position.y, _rangedRobot._followPosition.z);
        agent.transform.LookAt(_lookPosition);

        float distance = Vector3.Distance(agent.transform.position, _rangedRobot._followPosition);

        RaycastHit hit;
        if(!Physics.Raycast(_rangedRobot.ProjectilePoint.transform.position, (_rangedRobot._followPosition + new Vector3(0, 0.5f, 0) - _rangedRobot.ProjectilePoint.transform.position), out hit, distance, agent.GroundLayer))
        {
            if (agent.AttackTimer <= 0)
            {
                agent.AttackTimer = _enemy._enemyData._attackSpeed;
                _rangedRobot.TargetDirection = (_rangedRobot._followPosition - agent.transform.position).normalized;
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
