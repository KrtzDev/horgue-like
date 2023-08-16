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
        Animator = GetComponent<Animator>();
        Player = GameObject.FindGameObjectWithTag("Player");
        PlayerTransform = Player.GetComponent<Transform>();
        DecoyTransform = GameObject.FindGameObjectWithTag("Decoy").transform;

        StateMachine = new AI_StateMachine(this);
        RegisterStates();
        StateMachine.ChangeState(InitialState);
    }

    protected override void RegisterStates()
    {
        StateMachine.RegisterState(new Sentry_State_ChaseTarget());
        StateMachine.RegisterState(new Sentry_State_Attack());
        StateMachine.RegisterState(new Sentry_State_Off());
    }

    protected virtual void Update()
    {
        StateMachine.Update(GetComponent<AI_Agent>());
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
        Vector3 targetPos = PlayerTransform.position - _turretHead.transform.position;
        targetPos.y = 0.0f;
        _turretHead.transform.rotation = Quaternion.LookRotation(targetPos);
    }

    public void RotateTowardsEnemy()
    {
        float currentclosestdistance = Mathf.Infinity;
        AI_Agent_Enemy closestEnemy = null;

        Collider[] enemies = Physics.OverlapSphere(_projectilePoint.position, _range, EnemyLayer);
        foreach (Collider enemy in enemies)
        {
            float distanceToEnemy = Vector3.Distance(_projectilePoint.position, enemy.transform.position);
            if (distanceToEnemy > currentclosestdistance)
                continue;

            Vector3 directionToEnemy = enemy.transform.position + Vector3.up - _projectilePoint.position;
            if (!Physics.Raycast(_projectilePoint.position, directionToEnemy, distanceToEnemy, GroundLayer))
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
                if (AttackTimer < 0)
                {
                    AttackTimer = _attackRate;
                    _closestEnemy = closestEnemy;

                    Animator.SetBool("isAttacking", true);
                    Animator.SetBool("isChasing", false);
                    StateMachine.ChangeState(AI_StateID.Attack);
                }

                AttackTimer -= Time.deltaTime;
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, _range);
    }
}
