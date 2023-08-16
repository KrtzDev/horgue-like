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

        NavMeshAgent = GetComponent<NavMeshAgent>();
        ObstacleAgent = GetComponent<ObstacleAgent>();
        Animator = GetComponentInChildren<Animator>();
        RigidBody = GetComponent<Rigidbody>();
        _healthComponent = GetComponent<EnemyHealthComponent>();
        Player = GameObject.FindGameObjectWithTag("Player");
        PlayerTransform = Player.GetComponent<Transform>();
        DecoyTransform = GameObject.FindGameObjectWithTag("Decoy").transform;

        SetEnemyData();

        StateMachine = new AI_StateMachine(this);
        RegisterStates();
        StateMachine.ChangeState(InitialState);
    }

    protected virtual void Update()
    {
        StateMachine.Update(GetComponent<AI_Agent_Enemy>());
    }

    protected virtual void SetEnemyData()
    {
        NavMeshAgent.speed = _enemyData._maxMoveSpeed;
        NavMeshAgent.acceleration = _enemyData._acceleration;
        AttackTimer = 0f;
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
