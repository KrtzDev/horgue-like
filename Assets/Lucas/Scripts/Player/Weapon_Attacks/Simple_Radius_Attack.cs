using System.Collections;
using UnityEngine;

public class Simple_Radius_Attack : MonoBehaviour
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
        startParent = transform.parent.gameObject;
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
            _currentAttackDelay = _attackDelay;
        }
    }

    private void Attack(Enemy enemy)
    {
        _startAttacking = true;
        _isAttacking = true;

        enemyPositionAtAttack = enemy.transform.position;

        // gameObject.transform.parent = null;

        StartCoroutine(LerpPosition(enemy.transform.position, _attackTime));
    }

    IEnumerator LerpPosition(Vector3 endPosition, float duration)
    {
        float time = 0;
        while (time < duration)
        {
            transform.position = Vector3.Lerp(startPosition, endPosition, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        transform.position = endPosition;
        // gameObject.transform.parent = startParent.transform;
        _isAttacking = false;
        _startAttacking = false;

    }

    private void AttackMove()
    {
        transform.position = Vector3.Lerp(startPosition, new Vector3(10, 10, 10), 4500);

        if(transform.position == new Vector3(10, 10, 10))
        {
            _isAttacking = false;
            _startAttacking = false;
            gameObject.transform.parent = startParent.transform;
            transform.position = startPosition;
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
