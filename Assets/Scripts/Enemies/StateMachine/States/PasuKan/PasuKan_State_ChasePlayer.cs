using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PasuKan_State_ChasePlayer : AI_State_ChasePlayer
{

    public override void Enter(AI_Agent agent)
    {

    }

    public override void Update(AI_Agent agent)
    {
        if(!agent._navMeshAgent.enabled)
        {
            Debug.Log("Hello World");
            return;
        }

        _timer -= Time.deltaTime;

        if(_timer < 0f)
        {
            if (agent._navMeshAgent.pathStatus != UnityEngine.AI.NavMeshPathStatus.PathPartial)
            {
                if(!agent._useMovementPrediction)
                {
                    if (agent._followDecoy)
                    {
                        _followPosition = agent._decoyTransform.position;
                        SetTarget(agent);
                    }
                    else
                    {
                        _followPosition = agent._playerTransform.position;
                        SetTarget(agent);
                    }
                }
                else
                {
                    if (agent._followDecoy)
                    {
                        _followPosition = agent._decoyTransform.position;
                        SetTarget(agent);
                    }
                    else
                    {
                        _followPosition = agent._playerTransform.position + (agent._player.GetComponent<PlayerMovement>().AverageVelocity * agent._movementPredictionTime);

                        Vector3 directionToTarget = (_followPosition - agent.transform.position).normalized;
                        Vector3 directionToPlayer = (agent._playerTransform.position - agent.transform.position).normalized;

                        float dot = Vector3.Dot(directionToPlayer, directionToTarget);
                        if(dot < agent._movementPredictionThreshold)
                        {
                            _followPosition = agent._playerTransform.position;
                        }

                        SetTarget(agent);
                    }
                }

                StartRotating(agent);
            }

            _timer = _maxTime;
        }
    }

    public override void Exit(AI_Agent agent)
    {

    }

    private void SetTarget(AI_Agent agent)
    {
        if (agent._obstacleAgent.enabled)
        {
            agent._obstacleAgent.SetDestination(_followPosition);
        }
        else
        {
            agent._navMeshAgent.SetDestination(_followPosition);
        }
    }

    private void StartRotating(AI_Agent agent)
    {
        if (LookCoroutine != null)
        {
            AI_Manager.Instance.StopCoroutine(LookCoroutine);
        }

        LookCoroutine = AI_Manager.Instance.StartCoroutine(AI_Manager.Instance.LookAtTarget(agent, _followPosition, _maxTime));
    }
}
