using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum SentryStatus
{
    Ally, Enemy, Off
}
public class AI_Agent_Sentry : AI_Agent
{
    public SentryStatus _sentryStatus;

    public float _range = 10f;

    public GameObject _allyProjectile;
    public GameObject _enemyProjectile;

    public Transform _turretHead;

    public Transform _projectilePoint;
    public float _projectileSpeed;

    public float _attackRate;

    public Vector3 _targetDirection;

    public AI_Agent_Enemy _closestEnemy;

    public Renderer _sentryLamp;
    public Material _allyLamp;
    public Material _enemyLamp;

    protected override void Start()
    {
        // Register States
        _animator = GetComponent<Animator>();
        _player = GameObject.FindGameObjectWithTag("Player");
        _playerTransform = _player.GetComponent<Transform>();
        _decoyTransform = GameObject.FindGameObjectWithTag("Decoy").transform;

        _stateMachine = new AI_StateMachine(this);
        RegisterStates();
        _stateMachine.ChangeState(_initialState);
    }

    protected override void RegisterStates()
    {
        _stateMachine.RegisterState(new Sentry_State_ChaseTarget());
        _stateMachine.RegisterState(new Sentry_State_Attack());
        _stateMachine.RegisterState(new Sentry_State_Off());
    }

    protected virtual void Update()
    {
        _stateMachine.Update(GetComponent<AI_Agent>());
    }

    public void SwitchSentryStatus(SentryStatus _newStatus)
    {
        _sentryStatus = _newStatus;

        switch (_sentryStatus)
        {
            case SentryStatus.Ally:
                _sentryLamp.material = _allyLamp;
                break;

            case SentryStatus.Enemy:
                _sentryLamp.material = _enemyLamp;
                break;
        }
    }

    public void Shoot()
    {
        Rigidbody rb = null;

        switch (_sentryStatus)
        {
            case SentryStatus.Ally:
                rb = Instantiate(_allyProjectile, _projectilePoint.position, Quaternion.identity).GetComponent<Rigidbody>();
                break;

            case SentryStatus.Enemy:
                rb = Instantiate(_enemyProjectile, _projectilePoint.position, Quaternion.identity).GetComponent<Rigidbody>();
                break;
        }

        rb.AddForce(_targetDirection * _projectileSpeed, ForceMode.Impulse);
    }

    public void LookAtTarget()
    {
        Vector3 targetPos = _playerTransform.position - _turretHead.transform.position;
        targetPos.y = 0.0f;
        _turretHead.transform.rotation = Quaternion.LookRotation(targetPos);
    }

    public void RotateTowardsEnemy()
    {
        float currentclosestdistance = Mathf.Infinity;
        AI_Agent_Enemy closestEnemy = null;

        Collider[] enemies = Physics.OverlapSphere(_projectilePoint.position, _range, _enemyLayer);
        foreach (Collider enemy in enemies)
        {
            float distanceToEnemy = Vector3.Distance(_projectilePoint.position, enemy.transform.position);
            if (distanceToEnemy > currentclosestdistance)
                continue;

            Vector3 directionToEnemy = enemy.transform.position + Vector3.up - _projectilePoint.position;
            if (!Physics.Raycast(_projectilePoint.position, directionToEnemy, distanceToEnemy, _groundLayer))
            {
                closestEnemy = enemy.GetComponent<AI_Agent_Enemy>();
                currentclosestdistance = distanceToEnemy;
            }
        }

        if (closestEnemy != null)
        {
            Vector3 direction = (closestEnemy.transform.position + Vector3.up) - _turretHead.position;
            Vector3 rotateTowardsDirection = Vector3.RotateTowards(_turretHead.forward, direction, 20 * Time.deltaTime, .0f);
            _turretHead.transform.rotation = Quaternion.LookRotation(rotateTowardsDirection);
            if ((_turretHead.forward - direction.normalized).magnitude <= .1f)
            {
                if (_attackTimer < 0)
                {
                    _attackTimer = _attackRate;
                    _closestEnemy = closestEnemy;

                    _animator.SetBool("isAttacking", true);
                    _animator.SetBool("isChasing", false);
                    _stateMachine.ChangeState(AI_StateID.Attack);
                }

                _attackTimer -= Time.deltaTime;
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, _range);
    }
}
