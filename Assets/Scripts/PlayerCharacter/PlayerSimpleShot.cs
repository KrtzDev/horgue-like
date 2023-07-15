using UnityEngine;
using System.Threading.Tasks;

public class PlayerSimpleShot : MonoBehaviour
{
    [field: SerializeField]
    public bool CanShoot { get; set; } = true;

    [SerializeField]
    private EnemyProjectile _enemyProjectile_Prefab;

    [SerializeField]
    private LayerMask _enemyLayer;
    [SerializeField]
    private LayerMask _groundLayer;
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
        if (_currentAttackDelay <= 0 && CanShoot)
        {
            float currentclosestdistance = Mathf.Infinity;
            AI_Agent_Enemy closestEnemy = null;

            // Debug.Log(transform.position);
            // Debug.Log(_range);

            Collider[] enemies = Physics.OverlapSphere(transform.position, _range, _enemyLayer);
            foreach (var enemy in enemies)
            {
                Debug.Log(enemy.name);
                float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
                if (distanceToEnemy < currentclosestdistance)
                {
                    if (!Physics.Raycast(transform.position, (enemy.transform.position - transform.position), distanceToEnemy, _groundLayer))
                    {
                        closestEnemy = enemy.GetComponent<AI_Agent_Enemy>();
                        currentclosestdistance = distanceToEnemy;
                    }
                }
            }

            if (closestEnemy)
            {
                ShootAt(closestEnemy);
                _currentAttackDelay = _attackDelay;
            }
        }
    }

    private void ShootAt(AI_Agent_Enemy enemy)
    {
        Vector3 direction = enemy.transform.position - transform.position;
        EnemyProjectile newProjectile = Instantiate(_enemyProjectile_Prefab, transform.position + Vector3.up + new Vector3(direction.x, 0, direction.z).normalized, Quaternion.identity);
        newProjectile.GetComponent<Rigidbody>().velocity = new Vector3(direction.x, direction.y - 1, direction.z).normalized * _attackForce;
    }

    #region Draw_Gizmos
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, _range);
    }
    #endregion
}
