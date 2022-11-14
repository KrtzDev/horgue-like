using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField]
    private Transform playerTarget;

    public NavMeshAgent agent;

    public bool canMove;

    private void Start()
    {
        playerTarget = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    private void Update()
    {
        MoveTowardsPlayer();
    }

    private void MoveTowardsPlayer()
    {
        if (canMove)
        {
            agent.SetDestination(playerTarget.transform.position);
        }
    }
}
