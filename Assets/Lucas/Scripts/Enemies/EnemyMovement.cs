using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    public Transform playerTarget;
    public float UpdateRate = 0.1f; // how frequently to recalculate path based on Target transform's position

    private NavMeshAgent agent;

    private Coroutine FollowCoroutine;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        playerTarget = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    public void StartChasing()
    {
        if(FollowCoroutine == null)
        {
            FollowCoroutine = StartCoroutine(FollowTarget());
        }
        else
        {
            Debug.LogWarning("Called StartChasing on Enemy that is already chasing! This is likely a bug in some calling class.");
        }
    }

    private IEnumerator FollowTarget()
    {
        WaitForSeconds Wait = new WaitForSeconds(UpdateRate);

        while (gameObject.activeSelf)
        {
            agent.SetDestination(playerTarget.transform.position);
            yield return Wait;
        }
    }
}
