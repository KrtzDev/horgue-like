using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

// https://www.youtube.com/watch?v=1H9jrKyWKs0&t=34s

[RequireComponent(typeof(NavMeshAgent), typeof(Animator), typeof(EnemyHealthComponent))]
[DefaultExecutionOrder(1)]
public class AI_Agent : MonoBehaviour
{
    // Enemy Types

    [HideInInspector] public AI_StateMachine _stateMachine;
    public AI_StateID _initialState;
    public BasicEnemyData _enemyData;
    [HideInInspector] public NavMeshAgent _navMeshAgent;
    [HideInInspector] public ObstacleAgent _obstacleAgent;
    [HideInInspector] public Animator _animator;
    [HideInInspector] public Rigidbody _rb;
    [HideInInspector] public EnemyHealthComponent _healthComponent;

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
        // Register States

        _navMeshAgent = GetComponent<NavMeshAgent>();
        _obstacleAgent = GetComponent<ObstacleAgent>();
        _animator = GetComponent<Animator>();
        _rb = GetComponent<Rigidbody>();
        _healthComponent = GetComponent<EnemyHealthComponent>();
        _player = GameObject.FindGameObjectWithTag("Player");
        _playerTransform = _player.GetComponent<Transform>();
        _decoyTransform = GameObject.FindGameObjectWithTag("Decoy").transform;

        SetEnemyData();

        _stateMachine = new AI_StateMachine(this);
        RegisterStates();
        _stateMachine.ChangeState(_initialState);
    }

    protected virtual void Update()
    {
        _stateMachine.Update(GetComponent<AI_Agent>());
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

    private void SetEnemyData()
    {
        _navMeshAgent.speed = _enemyData._maxMoveSpeed;
        _navMeshAgent.acceleration = _enemyData._acceleration;
        _attackTimer = _enemyData._attackSpeed;
    }

    private void SetState(AI_StateID state) => _stateMachine.ChangeState(state);
}
