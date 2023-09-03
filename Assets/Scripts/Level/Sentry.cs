using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sentry : MonoBehaviour
{
    [SerializeField] private SentryStatus _sentryStatus;

    [SerializeField] private float _range = 10f;

    [SerializeField] private GameObject _projectile;

    [SerializeField] private Transform _turretHead;

    [SerializeField] private Transform _projectilePoint;
    [SerializeField] private float _projectileSpeed;

    [SerializeField] private Transform _playerPosition;

    private float _attackTimer;
    [SerializeField] private float _attackRate;

    private Vector3 _targetDirection;

    [SerializeField] private LayerMask _enemyLayer;
    [SerializeField] private LayerMask _groundLayer;

    private void Start()
    {
        _attackTimer = _attackRate;
    }

    private void Update()
    {
        /*
        switch (_sentryStatus)
        {
            case SentryStatus.Ally:
                RotateTowardsEnemy();
                break;

            case SentryStatus.Enemy:
                float _distance = Vector3.Distance(transform.position, _playerPosition.position);

                if (_distance < _range)
                {
                    LookAtTarget();

                    if (_attackTimer < 0)
                    {
                        _attackTimer = _attackRate;
                        _targetDirection = (_playerPosition.position - transform.position).normalized;

                        Shoot();
                    }

                    _attackTimer -= Time.deltaTime;
                }
                break;

            case SentryStatus.Off:

                break;
        }
        */
    }

    public void SwitchSentryStatus(SentryStatus _newStatus)
    {
        _sentryStatus = _newStatus;
    }

    public void Shoot()
    {
        Rigidbody rb = Instantiate(_projectile, _projectilePoint.position, Quaternion.identity).GetComponent<Rigidbody>();
        rb.AddForce(_targetDirection * _projectileSpeed, ForceMode.Impulse);
    }

    private void LookAtTarget()
    {
        Vector3 targetPos = _playerPosition.transform.position - _turretHead.transform.position;
        targetPos.y = 0.0f;
        _turretHead.transform.rotation = Quaternion.LookRotation(targetPos);
    }

    private void RotateTowardsEnemy()
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

        Debug.Log("Hallo, ich bin der closest enemy: " + closestEnemy);

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
                    Shoot();
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
