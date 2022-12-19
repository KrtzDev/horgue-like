using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    [field: SerializeField] public Transform PlayerTarget { get; set; }
    private NavMeshAgent _agent;

    [Header("Ranged Attack")]

    [SerializeField] private Transform[] _projectileSpawnPoints;
    [SerializeField] private Rigidbody _projectilePrefab;
    [SerializeField] private int _projectileSpawnPoint = 0;
    [SerializeField] private float _attackForce = 1f;
    [SerializeField] private float _attackSpeed = 1f;
    [SerializeField] private float _attackRange = 10f;
    [SerializeField] private bool _inRangedAttackRange;
    [SerializeField] private LayerMask _playerLaskMask;
    private float _timeSinceLastAttack = 0f;
    private Vector3 _attackDirection;

    [Header("Attack & Retreat")]
    [SerializeField]  private bool _attackRetreat;
    [SerializeField] private float _retreatTime;
    public Vector3 RetreatPosition { get; set; }
    private bool _useRetreat;


    private void Awake()
    {
        if(PlayerTarget == null)
        {
            PlayerTarget = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        }

        _agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        CheckAttackTime();
        CheckAttackTarget();

        if (_inRangedAttackRange && !_useRetreat)
        {
            if(_timeSinceLastAttack >= _attackSpeed)
            {
                StartCoroutine(StartShooting());
                _timeSinceLastAttack = 0;
            }
        }

        if(_timeSinceLastAttack < _retreatTime)
        {
            _useRetreat = true;
        }
        else
        {
            _useRetreat = false;

            if(_agent.destination != PlayerTarget.position)
            {
                StartChasing(PlayerTarget.position);
            }
        }
    }

    public void StartChasing(Vector3 targetPosition)
    {
        _agent.SetDestination(targetPosition);
    }

    private void StopFollowing()
    {
        _agent.SetDestination(this.transform.position);
    }

    private void Retreat()
    {
        SetRetreatPosition();
        StartChasing(RetreatPosition);
    }

    private void SetRetreatPosition()
    {
        // Set Retreat Position based on current enemy & player positions
    }

    private void Shoot()
    {
        Rigidbody enemyProjectile = Instantiate(_projectilePrefab, _projectileSpawnPoints[_projectileSpawnPoint].transform.position, _projectileSpawnPoints[_projectileSpawnPoint].transform.rotation);
        enemyProjectile.velocity = _attackDirection.normalized * _attackForce;
    }

    private void CheckAttackTime()
    {
        _timeSinceLastAttack += Time.deltaTime;
    }

    private void CheckAttackTarget()
    {
        Vector3 fromPosition = _projectileSpawnPoints[_projectileSpawnPoint].transform.position;
        Vector3 toPosition = PlayerTarget.transform.position + Vector3.up;
        Vector3 direction = (toPosition - fromPosition).normalized;

        Ray ray = new Ray(_projectileSpawnPoints[_projectileSpawnPoint].transform.position, direction);
        Debug.DrawRay(_projectileSpawnPoints[_projectileSpawnPoint].transform.position, direction * _attackRange, Color.green);

        RaycastHit hit;

        if(Physics.Raycast(ray, out hit, _attackRange, _playerLaskMask))
        {
            if (hit.collider.CompareTag("Player"))
            {
                _attackDirection = direction;
                _inRangedAttackRange = true;
            }
        }
        else
        {
            _inRangedAttackRange = false;
        }
    }

    private IEnumerator StartShooting() 
    {
        StopFollowing();

        Shoot();

        StartCoroutine(StopShooting());

        yield return null;
    }

    private IEnumerator StopShooting()
    {
        if (!_attackRetreat && !_useRetreat)
        {
            StartChasing(PlayerTarget.position);
        }
        else
        {
            Retreat();
        }

        yield return null;
    }
}
