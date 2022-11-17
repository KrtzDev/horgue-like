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

        if (inRangedAttackRange)
        {
            if(timeSinceLastAttack >= attackSpeed)
            {
                StartCoroutine(StartShooting());
                timeSinceLastAttack = 0;
            }
        }
    }

    public void StartChasing()
    {
        if(agent.enabled == false)
        {
            agent.enabled = true;
        }

        StartCoroutine(FollowTarget());
    }

    private void StopFollowing()
    {
        agent.SetDestination(this.transform.position);
    }

    private void Shoot()
    {
        Rigidbody enemyProjectile = Instantiate(projectilePrefab, ProjectileSpawnPoints[projectileSpawnPoint].transform.position, ProjectileSpawnPoints[projectileSpawnPoint].transform.rotation);
        enemyProjectile.velocity = attackDirection.normalized * attackForce;
    }

    private void CheckAttackTime()
    {
        if(timeSinceLastAttack <= attackSpeed)
        {
            timeSinceLastAttack += Time.deltaTime;
        }
    }

    private void CheckAttackTarget()
    {
        Vector3 fromPosition = ProjectileSpawnPoints[projectileSpawnPoint].transform.position;
        Vector3 toPosition = playerTarget.transform.position;
        Vector3 direction = toPosition - fromPosition;

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
            else
            {
                inRangedAttackRange = false;
            }
        }
    }

    private IEnumerator FollowTarget()
    {
        WaitForSeconds Wait = new WaitForSeconds(UpdateRate);

        while(gameObject.activeSelf)
        {
            agent.SetDestination(playerTarget.transform.position);
            yield return Wait;
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

        StartChasing();

        yield return Wait;
    }
}
