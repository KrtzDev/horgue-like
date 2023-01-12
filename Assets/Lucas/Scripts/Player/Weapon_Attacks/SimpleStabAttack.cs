using System.Collections;
using UnityEngine;

public class SimpleStabAttack : MonoBehaviour
{
    [SerializeField]
    private GameObject startParent;
    [SerializeField]
    private Vector3 startPosition;
    [SerializeField]
    private Vector3 startRotation;

    [SerializeField]
    private LayerMask _enemyLayer;
    [SerializeField]
    private LayerMask _groundLayer;
    [SerializeField]
    private float _attackRange;
    [SerializeField]
    private float _attackDelay;
    [SerializeField]
    private float _attackTime;
    public bool _isAttacking;
    private bool _startAttacking;

    private Vector3 enemyPositionAtAttack;

    private float _currentAttackDelay;

    private void Awake()
    {
        startParent = transform.parent.transform.gameObject;
        startPosition = transform.localPosition;
        startRotation = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z);

        _isAttacking = false;
        _startAttacking = false;
    }

    private void FixedUpdate()
    {
        _currentAttackDelay -= Time.deltaTime;
        if (_currentAttackDelay <= 0 && !_isAttacking)
        {
            LookForClosestEnemy();
        }
        else if (_isAttacking)
        {
            AttackMove();
        }
    }

    private void LookForClosestEnemy()
    {
        float currentclosestdistance = Mathf.Infinity;
        Enemy closestEnemy = null;

        Collider[] enemies = Physics.OverlapSphere(transform.position, _attackRange, _enemyLayer);
        foreach (var enemy in enemies)
        {
            float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
            if (distanceToEnemy < currentclosestdistance)
            {
                RaycastHit hit;
                if (Physics.Raycast(transform.position, (enemy.transform.position - transform.position), out hit, distanceToEnemy, _groundLayer))
                {
                }
                else
                {
                    closestEnemy = enemy.GetComponent<Enemy>();
                    currentclosestdistance = distanceToEnemy;
                }
            }
        }

        if (closestEnemy)
        {
            Attack(closestEnemy);
        }
    }

    private void Attack(Enemy enemy)
    {
        _startAttacking = true;
        _isAttacking = true;

        enemyPositionAtAttack = enemy.transform.position;

        gameObject.transform.parent = null;
    }

    private void AttackMove()
    {
        if (_startAttacking)
        {
            if (transform.position != enemyPositionAtAttack)
            {
                transform.position = Vector3.MoveTowards(transform.position, enemyPositionAtAttack, _attackTime * Time.deltaTime);
                transform.LookAt(enemyPositionAtAttack);
            }
            else
            {
                _startAttacking = false;
            }
        }
        else
        {
            if(gameObject.transform.parent != startParent.transform)
            {
                gameObject.transform.parent = startParent.transform;
            }
            
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, startPosition, _attackTime * Time.deltaTime);

            if (transform.localPosition == startPosition)
            {
                transform.localEulerAngles = startRotation;

                if (_isAttacking)
                {
                    _currentAttackDelay = _attackDelay;
                }

                _isAttacking = false;
            }
        }
    }

    #region Draw_Gizmos
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, _attackRange);
    }
    #endregion
}
