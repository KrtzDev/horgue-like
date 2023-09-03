using UnityEngine;

public class RangedRobot_State_ChasePlayer : AI_State_ChasePlayer
{
    private AI_Agent_RangedRobot _rangedRobot;

    public override void Enter(AI_Agent agent)
    {
        base.Enter(agent);

        _rangedRobot = agent as AI_Agent_RangedRobot;

        agent.Animator.SetBool("isChasing", true);
    }

    public override void Update(AI_Agent agent)
    {
        if (!agent.NavMeshAgent.enabled)
        {
            return;
        }

        if (!agent.UseMovementPrediction)
        {
            if (agent.FollowDecoy)
            {
                _rangedRobot._followPosition = agent.DecoyTransform.position;
                agent.SetTarget(agent, _rangedRobot._followPosition);
            }
            else
            {
                _rangedRobot._followPosition = agent.PlayerTransform.position;
                agent.SetTarget(agent, _rangedRobot._followPosition);
            }
        }
        else
        {
            if (agent.FollowDecoy)
            {
                _rangedRobot._followPosition = agent.DecoyTransform.position;
                agent.SetTarget(agent, _rangedRobot._followPosition);
            }
            else
            {
                _rangedRobot._followPosition = agent.PlayerTransform.position + (agent.Player.GetComponent<PlayerMovement>().AverageVelocity * agent.MovementPredictionTime);

                Vector3 directionToTarget = (_rangedRobot._followPosition - agent.transform.position).normalized;
                Vector3 directionToPlayer = (agent.PlayerTransform.position - agent.transform.position).normalized;

                float dot = Vector3.Dot(directionToPlayer, directionToTarget);
                if (dot < agent.MovementPredictionThreshold)
                {
                    _rangedRobot._followPosition = agent.PlayerTransform.position;
                }

                agent.SetTarget(agent, _rangedRobot._followPosition);
            }
        }

        float distance = Vector3.Distance(agent.transform.position, _rangedRobot._followPosition);
        CheckForBehaviour(agent, distance);
    }

    public override void Exit(AI_Agent agent)
    {
        agent.Animator.SetBool("isChasing", false);
    }

    private void StartRotating(AI_Agent agent)
    {
        if (LookCoroutine != null)
        {
            AI_Manager.Instance.StopCoroutine(LookCoroutine);
        }

        LookCoroutine = AI_Manager.Instance.StartCoroutine(AI_Manager.Instance.LookAtTarget(agent, _rangedRobot._followPosition, _maxTime));
    }

    private void CheckForBehaviour(AI_Agent agent, float distance)
    {
        RaycastHit hit;
        if (Physics.Raycast(_rangedRobot.ProjectilePoint.transform.position, (_rangedRobot._followPosition + new Vector3(0, 0.5f, 0) - _rangedRobot.ProjectilePoint.transform.position), out hit, distance, agent.GroundLayer))
        {
            if (distance < _enemy._enemyData._retreatRange)
            {
                agent.StateMachine.ChangeState(AI_StateID.Retreat);
            }
        }
        else
        {
            if (distance <= _enemy._enemyData._retreatRange)
            {
                agent.StateMachine.ChangeState(AI_StateID.Retreat);
            }
            else if (distance < _enemy._enemyData._attackRange)
            {
                agent.StateMachine.ChangeState(AI_StateID.Attack);
            }
        }
    }
}
