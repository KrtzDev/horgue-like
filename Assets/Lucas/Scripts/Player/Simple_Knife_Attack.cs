using System.Collections;
using UnityEngine;

public class Simple_Knife_Attack : MonoBehaviour
{
    [SerializeField]
    private GameObject startParent;
    private Transform startTransform;

    [SerializeField]
    private LayerMask _enemyLayer;
    [SerializeField]
    private LayerMask _groundLayer;
    [SerializeField]
    private float _attackRange;
    [SerializeField]
    private float _attackDelay;
    private bool _isAttacking;

    private Vector3 enemyPositionAtAttack;

    private float _currentAttackDelay;

    private void Awake()
    {
        startTransform = transform;
        startParent = gameObject.transform.parent.gameObject;
        _isAttacking = false;
    }

    private void FixedUpdate()
    {
        _currentAttackDelay -= Time.deltaTime;
        if (_currentAttackDelay <= 0 && !_isAttacking)
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
                _currentAttackDelay = _attackDelay;
            }
        }
        else if (_isAttacking)
        {
            AttackMove();
        }

        /* if (_isAttacking)
        {
            transform.position = Vector3.MoveTowards(transform.position, enemyPositionAtAttack, 10 * Time.deltaTime);

            if(transform.position == enemyPositionAtAttack)
            {
                _isAttacking = false;
            }
        }
        else
        {
            if (gameObject.transform.parent == null)
            {
                gameObject.transform.parent = startParent.transform;
            }
            else if (transform.position != startTransform.localPosition)
            {
                transform.position = Vector3.MoveTowards(transform.position, startTransform.localPosition, 10 * Time.deltaTime);
            }
            else if (transform.rotation != startTransform.rotation)
            {
                transform.rotation = startTransform.rotation;
            }
        } */
    }
    private void Attack(Enemy enemy)
    {
        // _isAttacking = true;
        enemyPositionAtAttack = enemy.transform.position;

        gameObject.transform.parent = null;
    }

    private void AttackMove()
    {
            if(transform.position != enemyPositionAtAttack)
            {
                transform.position = Vector3.MoveTowards(transform.position, enemyPositionAtAttack, 10 * Time.deltaTime);
            }
            else
            {
                Debug.Log("Range");
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
