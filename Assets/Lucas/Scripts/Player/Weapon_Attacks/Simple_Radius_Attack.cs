using System.Collections;
using UnityEngine;

public class Simple_Radius_Attack : MonoBehaviour
{
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

    [SerializeField]
    private Vector3 addRotation;

    [SerializeField]
    private bool fullRadius;

    private float _currentAttackDelay;

    private void Awake()
    {
        startPosition = transform.localPosition;
        startRotation = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z);

        _isAttacking = false;
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
        float currentHighestDistance = 0;
        Enemy enemyAtMaxRange = null;

        Collider[] enemies = Physics.OverlapSphere(transform.position, _attackRange, _enemyLayer);
        foreach (var enemy in enemies)
        {
            float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
            if (distanceToEnemy > currentHighestDistance)
            {
                RaycastHit hit;
                if (Physics.Raycast(transform.position, (enemy.transform.position - transform.position), out hit, distanceToEnemy, _groundLayer))
                {
                }
                else
                {
                    enemyAtMaxRange = enemy.GetComponent<Enemy>();
                    currentHighestDistance = distanceToEnemy;
                }
            }
        }

        if (enemyAtMaxRange)
        {
            Attack(enemyAtMaxRange);
        }
    }

    private void Attack(Enemy enemy)
    {
        _isAttacking = true;

        Vector3 startAttackPos = new Vector3(startPosition.x - _attackRange, startPosition.y, startPosition.z);
        Vector3 middleAttackPos = new Vector3(startPosition.x, startPosition.y, startPosition.z + _attackRange);
        Vector3 endAttackPos = new Vector3(startPosition.x + _attackRange, startPosition.y, startPosition.z);

        this.transform.localPosition = startAttackPos;
        this.transform.localEulerAngles += addRotation;


        StartCoroutine(LerpPosition(startAttackPos, middleAttackPos, endAttackPos, _attackTime));
    }

    IEnumerator LerpPosition(Vector3 startPosition, Vector3 middlePosition, Vector3 endPosition, float duration)
    {
        float time = 0;
        if (fullRadius)
        {
            while (time < duration)
            {
                transform.localPosition = Vector3.Lerp(startPosition, middlePosition, time / duration);
                time += Time.deltaTime;
                yield return null;
            }
            time = 0;
            while (time < duration)
            {
                transform.localPosition = Vector3.Lerp(middlePosition, endPosition, time / duration);
                time += Time.deltaTime;
                yield return null;
            }
            transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, 180, 180);
            middlePosition -= new Vector3(0, 0, 2 * _attackRange);
            time = 0;
            while (time < duration)
            {
                transform.localPosition = Vector3.Lerp(endPosition, middlePosition, time / duration);
                time += Time.deltaTime;
                yield return null;
            }
            time = 0;
            while (time < duration)
            {
                transform.localPosition = Vector3.Lerp(middlePosition, startPosition, time / duration);
                time += Time.deltaTime;
                yield return null;
            }
        }
        else
        {
            while (time < duration)
            {
                transform.localPosition = Vector3.Lerp(startPosition, middlePosition, time / duration);
                time += Time.deltaTime;
                yield return null;
            }
            time = 0;
            while (time < duration)
            {
                transform.localPosition = Vector3.Lerp(middlePosition, endPosition, time / duration);
                time += Time.deltaTime;
                yield return null;
            }
        }

        ResetAttackPosition();
    }

    private void ResetAttackPosition()
    {
        _isAttacking = false;
        this.transform.localPosition = startPosition;
        this.transform.localEulerAngles = startRotation;
        _currentAttackDelay = _attackDelay;
    }

    #region Draw_Gizmos
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, _attackRange);
    }
    #endregion
}
