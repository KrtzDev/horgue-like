using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AI_Agent : MonoBehaviour
{
    [HideInInspector] public AI_StateMachine StateMachine;
    public AI_StateID InitialState;
    [HideInInspector] public NavMeshAgent NavMeshAgent;
    [HideInInspector] public ObstacleAgent ObstacleAgent;
    [HideInInspector] public Animator Animator;
    [HideInInspector] public Rigidbody RigidBody;
    [HideInInspector] public float OriginalAnimationSpeed;
    [HideInInspector] public bool IsAffectedByAbility;

    public LayerMask GroundLayer;
    public LayerMask EnemyLayer;
    public LayerMask PlayerLayer;

    public GameObject Player;
    public Transform PlayerTransform;
    [HideInInspector] public Transform DecoyTransform;

    [HideInInspector] public float AttackTimer;
    [HideInInspector] public float LookRotationSpeed = 1f;
    [HideInInspector] public bool FollowDecoy;
    public bool CanUseSkill;

    [Header("Movement Prediction")]
    public bool UseMovementPrediction;
    [Range(-1f, 1f)] public float MovementPredictionThreshold = 0f;
    [Range(0.25f, 2f)] public float MovementPredictionTime = 1f;

    protected virtual void Start()
    {
        IsAffectedByAbility = false;
    }

    protected virtual void RegisterStates()
    {
        StateMachine.RegisterState(new AI_State_Idle());
        StateMachine.RegisterState(new AI_State_ChasePlayer());
        StateMachine.RegisterState(new AI_State_Retreat());
        StateMachine.RegisterState(new AI_State_Attack());
        StateMachine.RegisterState(new AI_State_SpecialAttack());
        StateMachine.RegisterState(new AI_State_Death());
    }

    public void SetState(AI_StateID state) => StateMachine.ChangeState(state);

    public void SetTarget(AI_Agent agent, Vector3 followPosition)
    {
        if (agent.ObstacleAgent.enabled && agent.enabled)
        {
            agent.ObstacleAgent.SetDestination(followPosition);
        }
        else if (agent.NavMeshAgent.enabled && agent.enabled)
        {
            if (agent.NavMeshAgent.pathStatus == NavMeshPathStatus.PathInvalid)
            {
                GameManager.Instance.EnemyDied();
                agent.gameObject.SetActive(false);
                return;
            }
            else
            {
                agent.NavMeshAgent.SetDestination(followPosition);
            }
        }
    }

    public void SetAgentActive() => gameObject.GetComponent<AI_Agent>().enabled = true;
}
