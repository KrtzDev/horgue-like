using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AI_Agent : MonoBehaviour
{
    [HideInInspector] public AI_StateMachine _stateMachine;
    public AI_StateID _initialState;
    [HideInInspector] public NavMeshAgent _navMeshAgent;
    [HideInInspector] public ObstacleAgent _obstacleAgent;
    [HideInInspector] public Animator _animator;
    [HideInInspector] public Rigidbody _rb;

    public LayerMask _groundLayer;
    public LayerMask _enemyLayer;
    public LayerMask _playerLayer;

    [HideInInspector] public GameObject _player;
    [HideInInspector] public Transform _playerTransform;
    [HideInInspector] public Transform _decoyTransform;

    [HideInInspector] public float _attackTimer;
    [HideInInspector] public float _lookRotationSpeed = 1f;
    [HideInInspector] public bool _followDecoy;
    public bool _canUseSkill;

    [Header("Movement Prediction")]
    public bool _useMovementPrediction;
    [Range(-1f, 1f)] public float _movementPredictionThreshold = 0f;
    [Range(0.25f, 2f)] public float _movementPredictionTime = 1f;

    protected virtual void Start()
    {

    }

    protected virtual void RegisterStates()
    {
        _stateMachine.RegisterState(new AI_State_Idle());
        _stateMachine.RegisterState(new AI_State_ChasePlayer());
        _stateMachine.RegisterState(new AI_State_Retreat());
        _stateMachine.RegisterState(new AI_State_Attack());
        _stateMachine.RegisterState(new AI_State_SpecialAttack());
        _stateMachine.RegisterState(new AI_State_Death());
    }

    protected void SetState(AI_StateID state) => _stateMachine.ChangeState(state);

    public void SetTarget(AI_Agent agent, Vector3 followPosition)
    {
        if (agent._obstacleAgent.enabled && agent.enabled)
        {
            agent._obstacleAgent.SetDestination(followPosition);
        }
        else if (agent._navMeshAgent.enabled && agent.enabled)
        {
            if (agent._navMeshAgent.pathStatus == NavMeshPathStatus.PathInvalid)
            {
                GameManager.Instance.EnemyDied();
                agent.gameObject.SetActive(false);
                return;
            }
            else
            {
                agent._navMeshAgent.SetDestination(followPosition);
            }
        }
    }
}
