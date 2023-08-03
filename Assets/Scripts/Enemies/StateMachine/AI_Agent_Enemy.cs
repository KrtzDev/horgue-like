using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

// https://www.youtube.com/watch?v=1H9jrKyWKs0&t=34s

[RequireComponent(typeof(NavMeshAgent), typeof(EnemyHealthComponent))]
[DefaultExecutionOrder(1)]
public class AI_Agent_Enemy : AI_Agent
{
    public BasicEnemyData _enemyData;
    public Vector3 _followPosition;
    public bool _isBossEnemy;
    [HideInInspector] public EnemyHealthComponent _healthComponent;

    [Header("Height Control")]
    public bool _useHeightControl;
    public GameObject _heightGO;

    protected override void Start()
    {
        base.Start();

        // Register States

        _navMeshAgent = GetComponent<NavMeshAgent>();
        _obstacleAgent = GetComponent<ObstacleAgent>();
        _animator = GetComponentInChildren<Animator>();
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
        _stateMachine.Update(GetComponent<AI_Agent_Enemy>());
    }

    protected virtual void SetEnemyData()
    {
        _navMeshAgent.speed = _enemyData._maxMoveSpeed;
        _navMeshAgent.acceleration = _enemyData._acceleration;
        _attackTimer = 0f;
    }

    public virtual void SetDeactive()
    {
        this.gameObject.SetActive(false);
    }

    public virtual void CheckForBossStage()
    {

    }

    private void SetHealthToPercentOfMax(float percent)
    {
        _healthComponent._currentHealth = (int)(_healthComponent._maxHealth * percent);
        if(_healthComponent._currentHealth > _healthComponent._maxHealth)
        {
            _healthComponent._currentHealth = _healthComponent._maxHealth;
        }
        _healthComponent._enemyHealthBar.HandleHealthChanged(percent);
    }

    public virtual void Shoot()
    {
    }

    public virtual void DoneShooting()
    {
    }
}
