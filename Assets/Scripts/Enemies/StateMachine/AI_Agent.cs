using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

// https://www.youtube.com/watch?v=1H9jrKyWKs0&t=34s

[RequireComponent(typeof(NavMeshAgent), typeof(Animator), typeof(EnemyHealthComponent))]
public class AI_Agent : MonoBehaviour
{
    [HideInInspector] public AI_StateMachine _stateMachine;
    public AI_StateID _initialState;
    public BasicEnemyData _enemyData;
    [HideInInspector] public NavMeshAgent _navMeshAgent;
    [HideInInspector] public ObstacleAgent _obstacleAgent;
    [HideInInspector] public Animator _animator;
    [HideInInspector] public EnemyHealthComponent _healthComponent;

    public LayerMask _groundLayer;
    public LayerMask _enemyLayer;
    public LayerMask _playerLayer;

    [HideInInspector] public Transform _playerTransform;
    [HideInInspector] public Transform _decoyTransform;

    [HideInInspector] public float _attackTimer;
    [HideInInspector] public bool _followDecoy;

    protected virtual void Start()
    {
        _stateMachine = new AI_StateMachine(this);
        _stateMachine.ChangeState(_initialState);
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _obstacleAgent = GetComponent<ObstacleAgent>();
        _animator = GetComponent<Animator>();
        _healthComponent = GetComponent<EnemyHealthComponent>();
        _playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        _decoyTransform = GameObject.FindGameObjectWithTag("Decoy").transform;

        SetEnemyData();

        // Register States
        RegisterStates();
    }

    protected virtual void Update()
    {
        _stateMachine.Update();
    }

    protected virtual void RegisterStates()
    {
        _stateMachine.RegisterState(new AI_State_Idle());
        _stateMachine.RegisterState(new AI_State_ChasePlayer());
        _stateMachine.RegisterState(new AI_State_Retreat());
        _stateMachine.RegisterState(new AI_State_Attack());
        _stateMachine.RegisterState(new AI_State_Death());
        _stateMachine.RegisterState(new AI_State_Damage());
    }

    private void SetEnemyData()
    {
        _navMeshAgent.speed = _enemyData._maxMoveSpeed;
        _navMeshAgent.acceleration = _enemyData._acceleration;
        _attackTimer = _enemyData._attackSpeed;
    }

    private void SetIdleState() => _stateMachine.ChangeState(AI_StateID.Idle);
}
