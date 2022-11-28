using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    public Transform playerTarget;
    private float UpdateRate = 0.1f; // how frequently to recalculate path based on Target transform's position

    private NavMeshAgent agent;

    [Header("Ranged Attack")]

    [SerializeField]
    private Transform[] ProjectileSpawnPoints;
    [SerializeField]
    private int projectileSpawnPoint = 0;
    [SerializeField]
    private Rigidbody projectilePrefab;
    [SerializeField]
    private float attackForce = 1f;
    [SerializeField]
    private float attackSpeed = 1f;
    [SerializeField]
    private float attackRange = 10f;
    private float timeSinceLastAttack = 0f;
    [SerializeField]
    private bool inRangedAttackRange;
    private Vector3 attackDirection;
    [SerializeField]
    private LayerMask playerLaskMask;

    [Header("Attack & Retreat")]
    [SerializeField]
    private bool AttackRetreat;
    private bool useRetreat;
    public Vector3 RetreatPosition;
    [SerializeField]
    private float retreatTime;


    private void Awake()
    {
        if(playerTarget == null)
        {
            playerTarget = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        }

        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        CheckAttackTime();
        CheckAttackTarget();

        if (inRangedAttackRange && !useRetreat)
        {
            if(timeSinceLastAttack >= attackSpeed)
            {
                StartCoroutine(StartShooting());
                timeSinceLastAttack = 0;
            }
        }

        if(timeSinceLastAttack < retreatTime)
        {
            useRetreat = true;
        }
        else
        {
            useRetreat = false;

            if(agent.destination != playerTarget.position)
            {
                StartChasing(playerTarget.position);
            }
        }
    }

    public void StartChasing(Vector3 targetPosition)
    {
        agent.SetDestination(targetPosition);
    }

    private void StopFollowing()
    {
        agent.SetDestination(this.transform.position);
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
        Rigidbody enemyProjectile = Instantiate(projectilePrefab, ProjectileSpawnPoints[projectileSpawnPoint].transform.position, ProjectileSpawnPoints[projectileSpawnPoint].transform.rotation);
        enemyProjectile.velocity = attackDirection.normalized * attackForce;
    }

    private void CheckAttackTime()
    {
        timeSinceLastAttack += Time.deltaTime;
    }

    private void CheckAttackTarget()
    {
        Vector3 fromPosition = ProjectileSpawnPoints[projectileSpawnPoint].transform.position;
        Vector3 toPosition = playerTarget.transform.position + Vector3.up;
        Vector3 direction = (toPosition - fromPosition).normalized;

        Ray ray = new Ray(ProjectileSpawnPoints[projectileSpawnPoint].transform.position, direction);
        Debug.DrawRay(ProjectileSpawnPoints[projectileSpawnPoint].transform.position, direction * attackRange, Color.green);

        RaycastHit hit;

        if(Physics.Raycast(ray, out hit, attackRange, playerLaskMask))
        {
            if (hit.collider.CompareTag("Player"))
            {
                attackDirection = direction;
                inRangedAttackRange = true;
            }
        }
        else
        {
            inRangedAttackRange = false;
        }
    }

    private IEnumerator StartShooting() 
    {
        StopFollowing();

        WaitForSeconds Wait = new WaitForSeconds(UpdateRate);

        Shoot();

        StartCoroutine(StopShooting());

        yield return Wait;
    }

    private IEnumerator StopShooting()
    {
        WaitForSeconds Wait = new WaitForSeconds(UpdateRate);

        if (!AttackRetreat && !useRetreat)
        {
            StartChasing(playerTarget.position);
        }
        else
        {
            Retreat();
        }

        yield return Wait;
    }
}
