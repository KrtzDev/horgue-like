using UnityEngine;

public class Player_Simple_Shot : MonoBehaviour
{
    [SerializeField]
    private EnemyProjectile _enemyProjectile_Prefab;

    [SerializeField]
    private LayerMask _enemyLayer;
    [SerializeField]
    private float _range;
    [SerializeField]
    private float _attackDelay;
    [SerializeField]
    private float _attackForce;

    private float _currentAttackDelay;

    private void FixedUpdate()
    {
        _currentAttackDelay -= Time.deltaTime;
        if (_currentAttackDelay <= 0)
        {
            float currentclosestdistance = Mathf.Infinity;
            Enemy closestEnemy = null;


            Collider[] enemies = Physics.OverlapSphere(transform.position, _range, _enemyLayer);
            foreach (var enemy in enemies)
            {
                float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
                if (distanceToEnemy < currentclosestdistance)
                {
                    closestEnemy = enemy.GetComponent<Enemy>();
                    currentclosestdistance = distanceToEnemy;
                }
            }

            if (closestEnemy)
            {
                ShootAt(closestEnemy);
                _currentAttackDelay = _attackDelay;
            }
        }
    }

    private void ShootAt(Enemy enemy)
    {
        Vector3 direction = enemy.transform.position - transform.position;
        EnemyProjectile newProjectile = Instantiate(_enemyProjectile_Prefab, transform.position + Vector3.up + new Vector3(direction.x, 0, direction.z).normalized, Quaternion.identity);
        newProjectile.GetComponent<Rigidbody>().velocity = new Vector3(direction.x, 0, direction.z).normalized * _attackForce;
    }

    #region Draw_Gizmos
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, _range);
    }
    #endregion
}
